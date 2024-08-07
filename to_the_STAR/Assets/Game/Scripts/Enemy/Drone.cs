using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, EnemyInterface
{
    [SerializeField] private int floor = 1; //바닥 기준, 0: up, 1: right, 2: down, 3: left
    [SerializeField] private EnemyStat stat;

    float hp;

    float halfBlockSize = 0.5f;
    int direction = 1; //-1 후진 0 정지 1 전진
    float timer = 0;
    float attackCooldownTimer = 0;
    GameObject playerObj;

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

    public void getDamage(float damage)
    {
        hp -= damage;
        return;
    }
    public float attack()
    {
        return stat.attackPower;
    }
    public float hpRatio()
    {
        return hp / stat.hp;
    }
    void Move()
    {
        transform.position = new Vector3(transform.position.x + moveVector.x * direction * Time.deltaTime, transform.position.y + moveVector.y * direction* Time.deltaTime, transform.position.z);
    }

    void Start()
    {
        hp = stat.hp;
        transform.rotation = Quaternion.Euler(0, 0, 180 - 90 * floor);
        moveVector.x = -(floor % 2 - 1) * stat.speed; moveVector.y = (floor % 2) * stat.speed;
        playerObj = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        
    }

    void Update()
    {
        float distanceToPlayer = Mathf.Sqrt((playerObj.transform.position.x - transform.position.x) * (playerObj.transform.position.x - transform.position.x) +
           (playerObj.transform.position.y - transform.position.y) * (playerObj.transform.position.y - transform.position.y));
        bool playerDetection = distanceToPlayer < stat.detectionRange && Mathf.Abs(playerObj.transform.rotation.eulerAngles.z - transform.rotation.eulerAngles.z) < 15;
        if (floor % 2 == 0 && Mathf.Abs(playerObj.transform.position.y - transform.position.y) > halfBlockSize) playerDetection = false;
        if (floor % 2 == 1 && Mathf.Abs(playerObj.transform.position.x - transform.position.x) > halfBlockSize) playerDetection = false;
        switch (state)
        {
            case State.Idle:
                if (playerDetection)
                {
                    timer = stat.detectionCooldown;
                    state = State.Detection;
                }
                else if (timer < 0)
                {
                    direction = Random.Range(-1, 2);
                    timer = Random.Range(3.0f, 5.0f);
                }
                Move();
                break;
            case State.Detection:
                if (timer < 0)
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
                if (distanceToPlayer <= stat.attackRange && attackCooldownTimer < 0) state = State.Attack;
                else if (!playerDetection)
                {
                    timer = stat.detectionCooldown;
                    state = State.Detection;
                }
                if (distanceToPlayer > stat.attackRange) Move();
                break;
            case State.Attack:
                //Todo: 공격용 오브젝트 생성해야함
                attackCooldownTimer = stat.attackCooldown;
                state = State.Chasing;
                break;
            case State.Stunned:
                if (timer < 0) state = State.Idle;
                break;
            case State.Dead: break;
        }
        if (hp < 0) state = State.Dead;
        attackCooldownTimer -= Time.deltaTime;
        timer -= Time.deltaTime;
    }
}
