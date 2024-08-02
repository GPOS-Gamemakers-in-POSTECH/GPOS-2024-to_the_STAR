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
    private int dirTimer = 0;
    private int detectTimer = 0;
    private int timer = 0;

    const float TILE = 1;
    const int DETECT = 150;

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
        _rb.MovePosition(currPosition + direction * _moveVector[floor%2] * (speed * Time.deltaTime));

        switch (state)
        {
            case State.Idle:
                Move();
                break;
            case State.Detection:
                Detect();
                break;
            case State.Chasing:
                Chase();
                break;
            case State.Attack: break;
            case State.Stunned: break;
            case State.Dead: break;
        }
        if (hp < 0) state = State.Dead;

        timer--;
    }

    void Update()
    {

    }

    private void Move()
    {
        if (dirTimer >= timer)
        {
            dirTimer = timer - Random.Range(150, 301);
            direction = Random.Range(-1, 2);
        }

        Vector2 playerPosition = player.transform.position;
        if (compareRotation(playerData.RotateDir))
        {
            float xDis = Mathf.Abs(playerPosition.x - transform.position.x);
            float yDis = Mathf.Abs(playerPosition.y - transform.position.y);

            bool flag;

            if (floor % 2 == 0) flag = xDis <= detectionRange && yDis < TILE;
            else flag = xDis < TILE && yDis <= detectionRange;

            if (flag)
            {
                state = State.Detection;
                detectTimer = timer - DETECT;
                direction = 0;
            }
        }
    }

    private void Detect()
    {
        Vector2 playerPosition = player.transform.position;
        float xDis = Mathf.Abs(playerPosition.x - transform.position.x);
        float yDis = Mathf.Abs(playerPosition.y - transform.position.y);

        bool flag;

        if (floor % 2 == 0) flag = xDis <= detectionRange && yDis < TILE;
        else flag = xDis < TILE && yDis <= detectionRange;

        if (!flag) state = State.Idle;
        else if(detectTimer == timer) state = State.Chasing;
    }

    private void Chase()
    {
        Vector2 playerPosition = player.transform.position;
        float xDis = Mathf.Abs(playerPosition.x - transform.position.x);
        float yDis = Mathf.Abs(playerPosition.y - transform.position.y);

        float distance = floor % 2 == 0 ? xDis : yDis;

        bool flag;

        if (floor % 2 == 0) flag = xDis <= detectionRange && yDis < TILE;
        else flag = xDis < TILE && yDis <= detectionRange;

        if (!flag)
        {
            state = State.Detection;
            detectTimer = timer - DETECT;
            direction = 0;
        }
        else if (distance <= attackRange) state = State.Attack;
    }

    private bool compareRotation(PlayerRotateDirection _prd)
    {
        return (_prd == PlayerRotateDirection.Up && floor == 0) || (_prd == PlayerRotateDirection.Right && floor == 1)
            || (_prd == PlayerRotateDirection.Down && floor == 2) || (_prd == PlayerRotateDirection.Left && floor == 3);
    }
}
