using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goliath : MonoBehaviour, EnemyInterface
{
    private Rigidbody2D _rb;

    [SerializeField] private int floor;
    [SerializeField] private GameObject attackObj;

    GameObject player;
    PlayerData playerData;
    SpriteRenderer _sr;

    private Vector2[] _moveVector = { new Vector2(1, 0), new Vector2(0, 1) };

    private const float maxHp = 1000;
    private const float detectionRange = 10;
    private float hp;

    private int direction = 0;
    private float attackTimer = 0;
    private float lookAround = 0;
    private float timer = 0;

    private const float deadCooldown = 3;

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

    public void getDamage(float damage, float stunCooldownSet)
    {
        if (state != State.Dead)
        {
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
    }

    void OnEnable()
    {
        hp = maxHp;
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
                Move();
                break;
            case State.Detection:
                break;
            case State.Chasing:
                break;
            case State.Attack:
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
}
