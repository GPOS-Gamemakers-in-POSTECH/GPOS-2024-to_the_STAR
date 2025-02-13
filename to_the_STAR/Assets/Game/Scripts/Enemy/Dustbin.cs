using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dustbin : MonoBehaviour, EnemyInterface
{
    private Rigidbody2D _rb;

    [SerializeField] private int floor = 0;
    [SerializeField] private EnemyStat stat;
    [SerializeField] private GameObject attackObj;
    [SerializeField] private AudioClip enemyHitSound;
    [SerializeField] private AudioClip enemyAttackSound;

    GameObject player;
    PlayerData playerData;
    SpriteRenderer _sr;
    Animator _ani;

    private Vector2[] _moveVector = { new Vector2(1, 0), new Vector2(0, 1) };

    private float hp;

    private int direction = 0;
    private int preDir = 0; // �� �浹�� �� �������� ���� �ʰ� �ϱ� ���� �� (�浹���� �ʾҴٸ� 0, �浹�ߴٸ� �浹���� ����� �̵� ����)
    private float attackTimer = 0;
    private float lookAround = 0;
    private float timer = 0;

    private Vector2 playerPosition;
    private float xDis;
    private float yDis;

    private bool attacked;
    private bool isMoving = false;
    private bool vanishing = false;

    const float TILE = 1;

    enum State
    {
        Idle,
        Detection,
        Chasing,
        Attack,
        Stunned,
        Dead
    }

    State state = State.Idle;
    public int getFloor()
    {
        return floor;
    }
    public void getDamage(float damage, float stunCooldownSet)
    {
        if (state != State.Dead)
        {
            GetComponent<AudioSource>().PlayOneShot(enemyHitSound, 1.0f);
            _ani.SetTrigger("Damaged");
            if (hp < damage) hp = 0;
            else hp -= damage;

            if (hp > 0)
            {
                state = State.Stunned;
                timer = stunCooldownSet;
            }
            else
            {
                state = State.Dead;
            }
        }
        return;
    }

    public float hpRatio()
    {
        return hp / stat.hp;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _ani = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerData = player.GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        hp = stat.hp;
    }

    void Update()
    {
        Vector2 currPosition = transform.position;

        isMoving = false;

        Vector3 tmp = floor % 2 == 0 ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0);

        RaycastHit2D ray1 = Physics2D.Raycast(currPosition, tmp, 1.0f, LayerMask.GetMask("Map"));
        RaycastHit2D ray2 = Physics2D.Raycast(currPosition, -tmp, 1.0f, LayerMask.GetMask("Map"));

        if ((ray1.collider != null && ray1.distance < 0.5f && direction == 1) || (ray2.collider != null && ray2.distance < 0.5f && direction == -1))
        {
            timer = Random.Range(2.0f, 4.0f);
            preDir = direction;
            direction = 0;
        }

        playerPosition = player.transform.position;
        xDis = Mathf.Abs(currPosition.x - playerPosition.x);
        yDis = Mathf.Abs(currPosition.y - playerPosition.y);


        switch (state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Detection:
                Detect();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Stunned:
                Stunned();
                break;
            case State.Dead:
                StartCoroutine(Dead());
                if (vanishing) _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, (_sr.color.a - 0.005f) > 0 ? (_sr.color.a - 0.005f) : 0);
                break;
        }

        if (((floor + 1) % 4) / 2 == 0)
        {
            if (direction == 1) _sr.flipX = false;
            else if (direction == -1) _sr.flipX = true;
        }
        else
        {
            if (direction == 1) _sr.flipX = true;
            else if (direction == -1) _sr.flipX = false;
        }

        if (timer > 0) timer -= Time.deltaTime;
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        _ani.SetBool("Move", isMoving);
    }

    private void Idle()
    {
        if (timer <= 0)
        {
            timer = Random.Range(2.0f, 4.0f);
            if(preDir == 0)
            {
                if (direction != 0) direction = 0;
                else direction = Random.Range(-1, 2);
            }
            else
            {
                direction = -preDir;
                preDir = 0;
            }
        }

        if (detectPlayer())
        {
            state = State.Detection;
            timer = stat.detectionCooldown;
            lookingAround();
            direction = 0;
        }
        else if(direction != 0)
        {
            Move();
        }

    }

    private void Move()
    {
        isMoving = true;
        Vector2 currPosition = transform.position;
        transform.position = currPosition + direction * _moveVector[floor % 2] * (stat.speed * Time.deltaTime);
    }

    private void Detect()
    {
        if (timer < lookAround && timer > 0.4f)
        {
            _sr.flipX = !_sr.flipX;
            lookingAround();
        }


        if (timer <= 0)
        {
            if (detectPlayer()) state = State.Chasing;
            else state = State.Idle;
        }
    }

    private void Chase()
    {
        float distance = floor % 2 == 0 ? xDis : yDis;
        setDirection();

        if (detectPlayer())
        {
            if (distance <= stat.attackRange)
            {
                if (attackTimer <= 0)
                {
                    attackTimer = stat.attackMotion1Cooldown;
                    attacked = false;
                    state = State.Attack;
                    _ani.SetTrigger("Attack"); 
                }
            }
            else
            {
                Move();
            }
        }
        else
        {
            state = State.Detection;
            timer = stat.detectionCooldown;
            lookingAround();
            direction = 0;
        }
    }

    private void Attack()
    {
        _ani.SetFloat("AttackMotion", _ani.GetFloat("AttackMotion") + Time.deltaTime);
        if (_ani.GetFloat("AttackMotion") >= 0.33f && attacked == false)
        {
            Vector3 move = floor % 2 == 0 ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0);
            move *= direction;
            GameObject Attack = Instantiate(attackObj, transform.position + move, Quaternion.identity);
            Attack.GetComponent<EnemyAttackObj>().init(stat.attackDuration, stat.attackPower, new Vector2(0, 0), EnemyAttackObj.EnemyType.Dustpan);
            GetComponent<AudioSource>().PlayOneShot(enemyAttackSound, 1.0f);
            attacked = true;
            attackTimer = stat.attackMotion2Cooldown;
        }
        else if (_ani.GetFloat("AttackMotion") >= 0.667f && attacked)
        {
            attackTimer = stat.attackCooldown;
            attacked = false;
            state = State.Chasing;
            _ani.SetFloat("AttackMotion", 0);
        }
    }

    public void Stunned()
    {
        if (timer <= 0)
        {
            _ani.SetFloat("AttackMotion", 0);
            attackTimer = stat.attackCooldown;
            if (detectPlayer()) state = State.Chasing;
            else
            {
                state = State.Detection;
                timer = stat.detectionCooldown;
                lookingAround();
                direction = 0;
            }
        }
    }

    IEnumerator Dead()
    {
        _ani.SetFloat("AttackMotion", 0);
        _ani.SetTrigger("Dead");
        yield return new WaitForSeconds(2f);
        vanishing = true;
        yield return new WaitForSeconds(stat.deadCooldown);
        Destroy(gameObject);
    }

    private bool compareRotation(PlayerRotateDirection _prd)
    {
        return (_prd == PlayerRotateDirection.Up && floor == 0) || (_prd == PlayerRotateDirection.Right && floor == 1)
            || (_prd == PlayerRotateDirection.Down && floor == 2) || (_prd == PlayerRotateDirection.Left && floor == 3);
    }

    private bool detectPlayer()
    {
        bool flag;
        float distance;

        if (floor % 2 == 0)
        {
            distance = xDis;
            flag = xDis <= stat.detectionRange && yDis < TILE;
        }
        else
        {
            distance = yDis;
            flag = xDis < TILE && yDis <= stat.detectionRange;
        }

        return flag && compareRotation(playerData.RotateDir);
    }

    private void setDirection()
    {
        if (floor % 2 == 0) direction = playerPosition.x > transform.position.x ? 1 : -1;
        else direction = playerPosition.y > transform.position.y ? 1 : -1;
    }

    private void lookingAround()
    {
        lookAround = timer - Random.Range(0.8f, 1.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurningPoint") && collision.GetComponent<TurningPointSet>().getType() < 5)
        {
            direction *= -1;
            timer = Random.Range(0.5f, 1.0f);
        }
    }
}
