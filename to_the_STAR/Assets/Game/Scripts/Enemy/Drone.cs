using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, EnemyInterface
{
    [SerializeField] private int floor = 1; //바닥 기준, 0: up, 1: right, 2: down, 3: left
    float hp = 100;
    float attackPower = 100;
    float detectionRange = 5;
    float attackRange = 2;
    float speed = 0.02f;

    float halfBlockSize = 0.5f;
    int direction = 1; //-1 후진 0 정지 1 전진
    int timer = 0;
    int attackCooldown = 100;
    int attackCooldownTimer = 0;
    int detectionCooldown = 100;
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
        return attackPower;
    }
    void Move()
    {
        transform.position = new Vector3(transform.position.x + moveVector.x * direction, transform.position.y + moveVector.y * direction, transform.position.z);
    }

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 180 - 90 * floor);
        moveVector.x = -(floor % 2 - 1) * speed; moveVector.y = (floor % 2) * speed;
        playerObj = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        float distanceToPlayer = Mathf.Sqrt((playerObj.transform.position.x - transform.position.x) * (playerObj.transform.position.x - transform.position.x) +
           (playerObj.transform.position.y - transform.position.y) * (playerObj.transform.position.y - transform.position.y));
        bool playerDetection = distanceToPlayer < detectionRange && Mathf.Abs(playerObj.transform.rotation.eulerAngles.z - transform.rotation.eulerAngles.z) < 15;
        if (floor % 2 == 0 && Mathf.Abs(playerObj.transform.position.y - transform.position.y) > halfBlockSize) playerDetection = false;
        if (floor % 2 == 1 && Mathf.Abs(playerObj.transform.position.x - transform.position.x) > halfBlockSize) playerDetection = false;
        switch (state)
        {
            case State.Idle:
                if(playerDetection)
                {
                    timer = detectionCooldown;
                    state = State.Detection;
                }
                else if (timer < 0)
                {
                    direction = Random.Range(-1, 2);
                    timer = Random.Range(120, 240);
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
                if(floor % 2 == 0)
                {
                    if (playerObj.transform.position.x > transform.position.x) direction = 1;
                    else direction = -1;
                }
                else
                {
                    if (playerObj.transform.position.y > transform.position.y) direction = 1;
                    else direction = -1;
                }
                if (distanceToPlayer < attackRange && attackCooldownTimer < 0) state = State.Attack;
                else if (!playerDetection)
                {
                    timer = detectionCooldown;
                    state = State.Detection;
                }
                Move();
                break;
            case State.Attack: 
                //Todo: 공격용 오브젝트 생성해야함
                attackCooldownTimer = attackCooldown;
                state = State.Chasing;
                break;
            case State.Stunned:
                if (timer < 0) state = State.Idle;
                break;
            case State.Dead: break;
        }
        if (hp < 0) state = State.Dead;
        attackCooldownTimer--;
        timer--;
    }
}
