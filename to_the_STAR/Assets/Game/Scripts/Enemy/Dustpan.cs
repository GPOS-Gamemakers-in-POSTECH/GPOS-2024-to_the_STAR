using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Dustpan : MonoBehaviour, EnemyInterface
{
    private Rigidbody2D _rb;

    [SerializeField] private int floor = 0;

    GameObject player;
    PlayerMovementController playerMovementController;
    PlayerData playerData;

    private Vector2[] _moveVector = { new Vector2(1, 0), new Vector2(0, 1) };
    private float hp = 20;
    private float attackPower = 2;
    private float detectionRange = 5;
    private float attackRange = 1;
    private float speed = 0.5f;

    private int direction = 0;
    private int attackTimer = 0;
    private int timer = 0;

    private Vector2 playerPosition;
    private float xDis;
    private float yDis;

    const float TILE = 1;
    const int DETECT = 150;
    const int ATTACK = 50;

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

    public void getDamage(float damage)
    {
        hp -= damage;
        return;
    }

    public float attack()
    {
        return attackPower;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerMovementController = player.GetComponent<PlayerMovementController>();
        playerData = player.GetComponent<PlayerData>();
    }

    void Start()
    {

    }

    private void FixedUpdate()
    {
        Vector2 currPosition = transform.position;
        playerPosition = player.transform.position;
        _rb.MovePosition(currPosition + direction * _moveVector[floor%2] * (speed * Time.deltaTime));

        currPosition = transform.position;
        xDis = Mathf.Abs(currPosition.x - playerPosition.x);
        yDis = Mathf.Abs(currPosition.y - playerPosition.y);


        switch (state)
        {
            case State.Idle:
                Move();
                Debug.Log("Idle");
                break;
            case State.Detection:
                Detect();
                Debug.Log("Detection");
                break;
            case State.Chasing:
                Chase();
                Debug.Log("Chasing");
                break;
            case State.Attack:
                Attack();
                Debug.Log("Attack");
                break;
            case State.Stunned: break;
            case State.Dead: break;
        }
        if (hp < 0) state = State.Dead;

        if (timer >= 0) timer--;
        if (attackTimer >= 0) attackTimer--;
    }

    void Update()
    {

    }

    private void Move()
    {
        if (timer < 0)
        {
            timer = Random.Range(150, 301);
            direction = Random.Range(-1, 2);
        }

        if (compareRotation(playerData.RotateDir))
        {
            bool flag;

            if (floor % 2 == 0) flag = xDis <= detectionRange && yDis < TILE;
            else flag = xDis < TILE && yDis <= detectionRange;

            if (flag)
            {
                state = State.Detection;
                timer = DETECT;
                direction = 0;
            }
        }
    }

    private void Detect()
    {
        bool flag;

        if (floor % 2 == 0) flag = xDis <= detectionRange && yDis < TILE;
        else flag = xDis < TILE && yDis <= detectionRange;

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
            flag = xDis <= detectionRange && yDis < TILE;
            direction = playerPosition.x > transform.position.x ? 1 : -1;
        }
        else
        {
            distance = yDis;
            flag = xDis < TILE && yDis <= detectionRange;
            direction = playerPosition.y > transform.position.y ? 1 : -1;
        }

        if (!flag)
        {
            state = State.Detection;
            timer = DETECT;
            direction = 0;
        }
        else if (distance <= attackRange)
        {
            if(attackTimer < 0) state = State.Attack;
            direction = 0;
        }
    }

    private void Attack()
    {
        attackTimer = ATTACK;
        state = State.Chasing;
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
