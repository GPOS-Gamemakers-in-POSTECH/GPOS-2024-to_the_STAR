using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dustpan : MonoBehaviour, EnemyInterface
{
    private Rigidbody2D _rb;

    [SerializeField] private int floor = 0;
    [SerializeField] private EnemyStat stat;
    [SerializeField] private GameObject attackObj;

    GameObject player;
    PlayerMovementController playerMovementController;
    PlayerData playerData;

    private Vector2[] _moveVector = { new Vector2(1, 0), new Vector2(0, 1) };

    private float hp;

    private int direction = 0;
    private float attackTimer = 0;
    private float timer = 0;

    private Vector2 playerPosition;
    private float xDis;
    private float yDis;

    private bool attacked;

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
    public void getDamage(float damage)
    {
        hp -= damage;
        return;
    }

    public float hpRatio()
    {
        return hp / stat.hp;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerMovementController = player.GetComponent<PlayerMovementController>();
        playerData = player.GetComponent<PlayerData>();
    }

    void OnEnable()
    {
        hp = stat.hp;
    }

    void FixedUpdate()
    {
        
    }

    void Update()
    {
        Vector2 currPosition = transform.position;

        currPosition = transform.position;
        playerPosition = player.transform.position;
        xDis = Mathf.Abs(currPosition.x - playerPosition.x);
        yDis = Mathf.Abs(currPosition.y - playerPosition.y);


        switch (state)
        {
            case State.Idle:
                Idle();
                Debug.Log("Idle");
                break;
            case State.Detection:
                Detect();
                Debug.Log("Detect");
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            case State.Stunned: break;
            case State.Dead: break;
        }
        if (hp < 0) state = State.Dead;

        if (timer > 0) timer -= Time.deltaTime;
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    private void Idle()
    {
        if (timer < 0)
        {
            timer = Random.Range(3.0f, 6.0f);
            direction = Random.Range(-1, 2);
        }

        if (compareRotation(playerData.RotateDir))
        {
            bool flag;

            if (floor % 2 == 0) flag = xDis <= stat.detectionRange && yDis < TILE;
            else flag = xDis < TILE && yDis <= stat.detectionRange;

            if (flag)
            {
                state = State.Detection;
                timer = stat.detectionCooldown;
            }
            else
            {
                Move();
            }
        }
    }

    private void Move()
    {
        Vector2 currPosition = transform.position;
        _rb.MovePosition(currPosition + direction * _moveVector[floor % 2] * (stat.speed * Time.deltaTime));
    }

    private void Detect()
    {
        bool flag;

        if (floor % 2 == 0) flag = xDis <= stat.detectionRange && yDis < TILE;
        else flag = xDis < TILE && yDis <= stat.detectionRange;

        if (timer < 0)
        {
            if (flag) state = State.Chasing;
            else state = State.Idle;
        }
    }

    private void Chase()
    {
        bool flag;
        float distance;

        if(floor % 2 == 0)
        {
            distance = xDis;
            flag = xDis <= stat.detectionRange && yDis < TILE;
            direction = playerPosition.x > transform.position.x ? 1 : -1;
        }
        else
        {
            distance = yDis;
            flag = xDis < TILE && yDis <= stat.detectionRange;
            direction = playerPosition.y > transform.position.y ? 1 : -1;
        }

        if (flag)
        {
            if(distance <= stat.attackRange)
            {
                if (attackTimer <= 0)
                {
                    attackTimer = stat.attackMotion1Cooldown;
                    attacked = false;
                    state = State.Attack;
                }
            }
            else
            {
                if(attackTimer <= stat.attackCooldown - stat.attackDuration) Move();
            }
        }
        else
        {
            state = State.Detection;
            timer = stat.detectionCooldown;
        }
    }

    private void Attack()
    {
        if (attackTimer <= 0 && attacked == false)
        {
            Debug.Log("Attack2");
            GameObject Attack = Instantiate(attackObj, transform.position, Quaternion.identity);
            Attack.GetComponent<EnemyAttackObj>().init(stat.attackDuration, stat.attackPower, new Vector2(0, 0), EnemyAttackObj.EnemyType.Dustpan);
            attacked = true;
            attackTimer = stat.attackMotion2Cooldown;
        }
        else if(attackTimer <= 0 && attacked)
        {
            attackTimer = stat.attackCooldown;
            attacked = false;
            state = State.Chasing;
        }
    }

    public void Stunned()
    {

    }

    private void Dead()
    {

    }

    private bool compareRotation(PlayerRotateDirection _prd)
    {
        return (_prd == PlayerRotateDirection.Up && floor == 0) || (_prd == PlayerRotateDirection.Right && floor == 1)
            || (_prd == PlayerRotateDirection.Down && floor == 2) || (_prd == PlayerRotateDirection.Left && floor == 3);
    }
}
