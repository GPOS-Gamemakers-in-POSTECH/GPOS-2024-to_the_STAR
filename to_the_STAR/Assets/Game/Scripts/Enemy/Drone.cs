using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, EnemyInterface
{
    [SerializeField] private int floor = 1; //바닥 기준, 0: up, 1: right, 2: down, 3: left
    [SerializeField] private EnemyStat stat;
    [SerializeField] private GameObject attackObj;
    [SerializeField] private bool fix = false;

    float hp;

    float halfBlockSize = 0.5f;
    int direction = 1; //-1 후진 0 정지 1 전진
    int preDir = 0;
    float timer = 0;
    float attackCooldownTimer = 0;
    float lookAround = 0;

    bool isMoving = false;
    bool wait = true;
    bool vanishing = false;

    GameObject playerObj;
    SpriteRenderer _sr;
    Animator _ani;

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
    Vector2 moveVector;

    public int getFloor()
    {
        return floor;
    }
    public void getDamage(float damage, float stunCooldownSet)
    {
        if (state != State.Dead)
        {
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
                hp = 0;
                state = State.Dead;
                timer = stat.deadCooldown;
            }
        }
        return;
    }
    public float hpRatio()
    {
        return hp / stat.hp;
    }
    void Move()
    {
        if(direction != 0) isMoving = true;
        transform.position = new Vector3(transform.position.x + moveVector.x * direction * Time.deltaTime, transform.position.y + moveVector.y * direction * Time.deltaTime, transform.position.z);
    }
    IEnumerator Dead()
    {
        _ani.SetTrigger("Dead");
        yield return new WaitForSeconds(2f);
        vanishing = true;
        yield return new WaitForSeconds(stat.deadCooldown);
        Destroy(gameObject);
    }

    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        hp = stat.hp;
        transform.rotation = Quaternion.Euler(0, 0, 180 - 90 * floor);
        moveVector.x = -(floor % 2 - 1) * stat.speed; moveVector.y = (floor % 2) * stat.speed;
        playerObj = GameObject.Find("Player");
        _sr = GetComponent<SpriteRenderer>();
        _ani = GetComponent<Animator>();
    }

    void Update()
    {
        isMoving = false;
        float distanceToPlayer = Mathf.Sqrt((playerObj.transform.position.x - transform.position.x) * (playerObj.transform.position.x - transform.position.x) +
           (playerObj.transform.position.y - transform.position.y) * (playerObj.transform.position.y - transform.position.y));
        bool playerDetection = distanceToPlayer <= stat.detectionRange;

        Vector3 currPosition = transform.position;
        Vector3 tmp = floor % 2 == 0 ? new Vector3(1, 0, 0) : new Vector3(0, 1, 0);

        RaycastHit2D ray1 = Physics2D.Raycast(currPosition, tmp, 1.0f, LayerMask.GetMask("Map"));
        RaycastHit2D ray2 = Physics2D.Raycast(currPosition, -tmp, 1.0f, LayerMask.GetMask("Map"));

        if ((ray1.collider != null && ray1.distance < 0.6f && direction == 1) || (ray2.collider != null && ray2.distance < 0.6f && direction == -1))
        {
            timer = Random.Range(2.0f, 4.0f);
            preDir = direction;
            direction = 0;
        }

        if (!fix)
        {
            playerDetection &= Mathf.Abs(playerObj.transform.rotation.eulerAngles.z - transform.rotation.eulerAngles.z) <= 15;
            if (floor % 2 == 0 && Mathf.Abs(playerObj.transform.position.y - transform.position.y) > halfBlockSize) playerDetection = false;
            if (floor % 2 == 1 && Mathf.Abs(playerObj.transform.position.x - transform.position.x) > halfBlockSize) playerDetection = false;
        }

        switch (state)
        {
            case State.Idle:
                if (playerDetection)
                {
                    if (fix)
                    {
                        if (floor % 2 == 0)
                        {
                            if (playerObj.transform.position.x > transform.position.x) direction = 1;
                            else direction = -1;
                        }
                        else
                        {
                            if (playerObj.transform.position.y > transform.position.y) direction = 1;
                            else direction = -1;
                        }

                        if (attackCooldownTimer <= 0)
                        {
                            if (wait)
                            {
                                attackCooldownTimer = stat.attackCooldown;
                                wait = false;
                            }
                            else
                            {
                                state = State.Attack;
                            }
                        }
                    }
                    else
                    {
                        timer = stat.detectionCooldown;
                        state = State.Detection;
                        lookingAround();
                        direction = 0;
                    }
                }
                else if (!fix)
                {
                    if(timer <= 0)
                    {
                        timer = Random.Range(2.0f, 4.0f);
                        if (preDir == 0)
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
                    Move();
                }
                else
                {
                    wait = true;
                }
                break;
            case State.Detection:
                if (timer < lookAround && timer > 0.4f)
                {
                    _sr.flipX = !_sr.flipX;
                    lookingAround();
                }
                if (timer <= 0)
                {
                    if (!playerDetection) state = State.Idle;
                    else state = State.Chasing;
                }
                break;
            case State.Chasing:
                if (floor % 2 == 0)
                {
                    if (playerObj.transform.position.x > transform.position.x) direction = 1;
                    else direction = -1;
                }
                else
                {
                    if (playerObj.transform.position.y > transform.position.y) direction = 1;
                    else direction = -1;
                }
                if (distanceToPlayer <= stat.attackRange && attackCooldownTimer <= 0) state = State.Attack;
                else if (!playerDetection)
                {
                    timer = stat.detectionCooldown;
                    state = State.Detection;
                    lookingAround();
                    direction = 0;
                }
                if (distanceToPlayer > stat.attackRange && !fix) Move();
                break;
            case State.Attack:
                _ani.SetTrigger("Attack");
                Vector2 bulletDir = (playerObj.transform.position - transform.position) / distanceToPlayer;
                float rotZ = Mathf.Atan2(bulletDir.y, bulletDir.x) * Mathf.Rad2Deg;
                GameObject Attack = Instantiate(attackObj, transform.position, Quaternion.Euler(0, 0, rotZ));
                Attack.GetComponent<EnemyAttackObj>().init(stat.attackDuration, stat.attackPower, bulletDir * stat.bulletSpeed, EnemyAttackObj.EnemyType.Drone);
                attackCooldownTimer = stat.attackCooldown;
                if (fix) state = State.Idle;
                else state = State.Chasing;
                break;
            case State.Stunned:
                if (timer <= 0)
                {
                    if (fix)
                    {
                        attackCooldownTimer = stat.attackCooldown;
                        state = State.Idle;
                    }
                    else if (playerDetection) state = State.Chasing;
                    else
                    {
                        timer = stat.detectionCooldown;
                        state = State.Detection;
                        lookingAround();
                        direction = 0;
                    }
                }
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

        if (attackCooldownTimer > 0) attackCooldownTimer -= Time.deltaTime;
        if (timer > 0) timer -= Time.deltaTime;
        _ani.SetBool("Move", isMoving);
    }

    private void lookingAround()
    {
        lookAround = timer - Random.Range(0.8f, 1.2f);
    }
}