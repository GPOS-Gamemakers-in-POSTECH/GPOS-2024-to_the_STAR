using Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Goliath : MonoBehaviour, EnemyInterface
{
    private Rigidbody2D _rb;

    [SerializeField] private int floor;
    [SerializeField] private GameObject attackObj1;
    [SerializeField] private GameObject attackObj2;

    [SerializeField] private GameObject[] Legs;

    GameObject LeftLeg1;
    GameObject LeftLeg2;
    GameObject RightLeg1;
    GameObject RightLeg2;

    GameObject player;
    PlayerData playerData;
    SpriteRenderer _sr;

    private Vector3 _up = new Vector3(0, 1, 0);
    private Vector3 _down = new Vector3(0, -1, 0);
    private Vector3 _right = new Vector3(1, 0, 0);
    private Vector3 _left = new Vector3(-1, 0, 0);

    private const float maxHp = 1000;
    private const float detectionRange = 4.0f;
    private const float detectionCooldown = 3.0f;
    private const float attackRange = 2.0f;
    private const float attackMotion1Cooldown = 1.0f;
    private const float attackMotion2Cooldown = 0.1f;
    private const float attackDuration = 2.0f;
    private const float attackCooldown = 4.0f;
    private const float attackPower = 10.0f;
    private float hp;
    private float speed = 0.3f;

    private int direction = 0;
    private float attackTimer = 0;
    private float lookAround = 0;
    private float timer = 0;

    private const float deadCooldown = 3;

    private const float legMovementX = 0.6f;
    private const float legMovementY = 0.4f;
    private const float walkCooldown = 0.3f;

    private Vector2 playerPosition;
    private float xDis;
    private float yDis;

    private bool isAttacking = false;
    private bool isWalking = false;
    private int walkN = 0;
    private int walkState = 0;

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
            if (hp < damage) hp = 0;
            else hp -= damage;

            if (hp <= 0)
            {
                state = State.Dead;
                timer = deadCooldown;
            }
        }
        return;
    }

    public float hpRatio()
    {
        return hp / maxHp;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerData = player.GetComponent<PlayerData>();

        _up = VectorRotate(_up, transform.rotation.z);
        _down = VectorRotate(_down, transform.rotation.z);
        _right = VectorRotate(_right, transform.rotation.z);
        _left = VectorRotate(_left, transform.rotation.z);

        LeftLeg1 = Instantiate(Legs[0], transform.position + VectorAdd(-0.6f, -0.2f, 0.1f), Quaternion.identity); // 왼쪽 다리 뒷부분
        LeftLeg2 = Instantiate(Legs[0], transform.position + VectorAdd(-0.4f, -0.2f, -0.1f), Quaternion.identity); // 왼쪽 다리 앞부분
        RightLeg1 = Instantiate(Legs[1], transform.position + VectorAdd(0.4f, -0.2f, 0.1f), Quaternion.identity); // 오른쪽 다리 뒷부분
        RightLeg2 = Instantiate(Legs[1], transform.position + VectorAdd(0.6f, -0.2f, -0.1f), Quaternion.identity); // 오른쪽 다리 앞부분

        LeftLeg1.GetComponent<GoliathLeg>().init(this);
        LeftLeg2.GetComponent<GoliathLeg>().init(this);
        RightLeg1.GetComponent<GoliathLeg>().init(this);
        RightLeg2.GetComponent<GoliathLeg>().init(this);
    }

    void OnEnable()
    {
        hp = maxHp;
    }

    void Update()
    {
        Vector2 currPosition = transform.position;

        /*RaycastHit2D rightRay = Physics2D.Raycast(transform.position, _right, 1.0f, LayerMask.GetMask("Map"));
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, _left, 1.0f, LayerMask.GetMask("Map"));

        if(isWalking && ((rightRay.distance < 0.7f && direction == 1) || (leftRay.distance < 0.7f && direction == -1)))
        {
            Debug.Log(rightRay.distance);
            Debug.Log(leftRay.distance);
            timer = Random.Range(3.0f, 5.0f);
            isWalking = false;
            walkN = 0;
            init();
        }*/

        currPosition = transform.position;
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
                break;
            case State.Dead:
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
    }

    private void Move()
    {
        Debug.Log("m");
        transform.position = transform.position + VectorAdd(speed, 0) * Time.deltaTime * direction;
        switch (walkState)
        {
            case 0:
                LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime;
                RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime;
                if (timer <= 0) { walkState++; timer = walkCooldown; }
                break;
            case 1:
                LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(direction * legMovementX, -legMovementY) * Time.deltaTime;
                RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(direction * legMovementX, -legMovementY) * Time.deltaTime;
                if (timer <= 0) { walkState++; timer = walkCooldown; }
                break;
            case 2:
                LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime;
                RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime;
                if (timer <= 0) { walkState++; timer = walkCooldown; }
                break;
            case 3:
                LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(direction * legMovementX, -legMovementY) * Time.deltaTime;
                RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(direction * legMovementX, -legMovementY) * Time.deltaTime;
                if (timer <= 0) { walkState++; }
                break;
            case 4:
                isWalking = false;
                walkState = 0;
                walkN--;
                init();
                break;
        }
    }

    private void Idle()
    {
        if (isWalking)
        {
            Move();
        }
        else
        {
            if (timer <= 0)
            {
                if (detectPlayer())
                {
                    walkN = 0;
                    state = State.Detection;
                    timer = detectionCooldown;
                    lookingAround();
                    direction = 0;
                }
                else
                {
                    if(walkN == 0) direction = Random.Range(-1, 2);
                    if (direction != 0)
                    {
                        if(walkN == 0) walkN = Random.Range(3, 7);
                        timer = walkCooldown;
                        isWalking = true;
                    }
                    else
                    {
                        timer = Random.Range(3.0f, 5.0f);
                    }
                }
            }
        }
    }

    private void Detect()
    {
        if (timer < lookAround && timer > 0.4f)
        {
            _sr.flipX = !_sr.flipX;
            lookingAround();
        }


        if (timer < 0)
        {
            if (detectPlayer())
            {
                walkN = 1;
                state = State.Chasing;
            }
            else state = State.Idle;
        }
    }

    private void Chase()
    {
        if (isWalking)
        {
            Move();
        }
        else
        {
            float distance = floor % 2 == 0 ? xDis : yDis;
            setDirection();

            if (detectPlayer())
            {
                if (distance <= attackRange)
                {
                    if (attackTimer <= 0)
                    {
                        attackTimer = attackMotion1Cooldown;
                        isAttacking = false;
                        state = State.Attack;
                    }
                }
                else
                {
                    if (attackTimer <= attackCooldown - attackDuration)
                    {
                        isWalking = true;
                        walkN = 1;
                        timer = walkCooldown;
                    }
                }
            }
            else
            {
                state = State.Detection;
                timer = detectionCooldown;
                lookingAround();
                direction = 0;
            }
        }
    }

    private void Attack()
    {
        if (isAttacking)
        {
            if(attackTimer <= 0)
            {
                init();

                Vector3 legCenter = direction == 1 ? RightLeg1.transform.position : LeftLeg2.transform.position;
                legCenter += VectorAdd(direction * 0.57f, -1.8f);
                GameObject Attack1 = Instantiate(attackObj2, legCenter + VectorAdd(0.5f, 0), Quaternion.identity);
                GameObject Attack2 = Instantiate(attackObj2, legCenter + VectorAdd(-0.5f, 0), Quaternion.identity);
                Attack1.GetComponent<EnemyAttackObj>().init(attackDuration, attackPower * 0.3f, VectorAdd(legMovementX * 1.5f, 0), EnemyAttackObj.EnemyType.Goliath);
                Attack2.GetComponent<EnemyAttackObj>().init(attackDuration, attackPower * 0.3f, VectorAdd(-legMovementX * 1.5f, 0), EnemyAttackObj.EnemyType.Goliath);

                state = State.Chasing;
                isAttacking = false;
                attackTimer = attackCooldown;
            }
            else
            {
                if (direction == 1)
                {
                    RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, -legMovementY * 20) * Time.deltaTime;
                    RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, -legMovementY * 20) * Time.deltaTime;
                }
                else
                {
                    LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, -legMovementY * 20) * Time.deltaTime;
                    LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, -legMovementY * 20) * Time.deltaTime;
                }
            }
        }
        else
        {
            if (attackTimer <= 0)
            {
                Vector3 legPos = direction == 1 ? RightLeg1.transform.position : LeftLeg2.transform.position;
                Vector3 tmp = VectorAdd(direction * 0.57f, -1.8f);
                GameObject Attack = Instantiate(attackObj1, legPos + tmp, Quaternion.identity);
                Attack.GetComponent<EnemyAttackObj>().init(attackMotion2Cooldown, attackPower, VectorAdd(0, -legMovementY * 20), EnemyAttackObj.EnemyType.Goliath);

                attackTimer = attackMotion2Cooldown;
                isAttacking = true;
            }
            else
            {
                if(direction == 1)
                {
                    RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, legMovementY * 2) * Time.deltaTime;
                    RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, legMovementY * 2) * Time.deltaTime;
                }
                else
                {
                    LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, legMovementY * 2) * Time.deltaTime;
                    LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, legMovementY * 2) * Time.deltaTime;
                }
            }
        }
    }

    private void init()
    {
        LeftLeg1.transform.position = transform.position + VectorAdd(-0.6f, -0.2f, 0.1f);
        LeftLeg2.transform.position = transform.position + VectorAdd(-0.4f, -0.2f, -0.1f);
        RightLeg1.transform.position = transform.position + VectorAdd(0.4f, -0.2f, 0.1f);
        RightLeg2.transform.position = transform.position + VectorAdd(0.6f, -0.2f, -0.1f);
    }

    private bool detectPlayer()
    {
        bool flag;
        float distance;

        if (floor % 2 == 0)
        {
            distance = xDis;
            flag = xDis <= detectionRange && yDis < TILE;
        }
        else
        {
            distance = yDis;
            flag = xDis < TILE && yDis <= detectionRange;
        }

        return flag && compareRotation(playerData.RotateDir);
    }

    private bool compareRotation(PlayerRotateDirection _prd)
    {
        return (_prd == PlayerRotateDirection.Up && floor == 0) || (_prd == PlayerRotateDirection.Right && floor == 1)
            || (_prd == PlayerRotateDirection.Down && floor == 2) || (_prd == PlayerRotateDirection.Left && floor == 3);
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

    private Vector3 VectorRotate(Vector3 tmp, float angle)
    {
        return new Vector3(tmp.x * Mathf.Cos(angle) - tmp.y * Mathf.Sin(angle), tmp.x * Mathf.Sin(angle) + tmp.y * Mathf.Cos(angle), 0);
    }

    private Vector3 VectorAdd(float x, float y, float z = 0)
    {
        return x * _right + y * _up + new Vector3(0, 0, z);
    }
}
