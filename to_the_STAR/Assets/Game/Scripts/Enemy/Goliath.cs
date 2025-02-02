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
    [SerializeField] private GameObject walkingAttackObj;
    [SerializeField] AudioClip goliathAttackSound;

    [SerializeField] private GameObject[] Legs;

    GameObject LeftLeg1;
    GameObject LeftLeg2;
    GameObject RightLeg1;
    GameObject RightLeg2;

    GameObject player;
    PlayerData playerData;
    GameObject Camera;
    SpriteRenderer _sr;

    private Vector3 _up = new Vector3(0, 1, 0);
    private Vector3 _down = new Vector3(0, -1, 0);
    private Vector3 _right = new Vector3(1, 0, 0);
    private Vector3 _left = new Vector3(-1, 0, 0);

    private Vector3 dis;

    private float faster = 0.2f;

    private const float maxHp = 1000;
    private const float detectionRange = 10.0f;
    private const float detectionCooldown = 3.0f;
    private const float attackRange = 2.0f;
    private const float attackMotion1Cooldown = 0.8f;
    private const float attackMotion2Cooldown = 0.05f;
    private const float attack2MotionCooldown = 0.5f;
    private const float attackDuration = 1.0f;
    private const float attackCooldown = 4.0f;
    private const float attackPower = 15.0f;
    private float hp = 1000;

    private int direction = 0;
    private int preDir = 0;
    private float attackTimer = 0;
    private float timer = 0;
    private int attackType = 0;
    //private float attack2count = 0;

    private const float deadCooldown = 3;

    private const float legMovementX = 1.6f;
    private const float legMovementY = 0.7f; // 0.5f
    private const float walkCooldown = 0.3f;
    private const float startY = 17.78f;
    //private const float startY = 2.33f;

    private Vector2 playerPosition;
    private float xDis;
    private float yDis;

    private bool isAttacking = false;
    private bool isWalking = false;
    private int walkN = 0;
    private int walkState = 0;

    const float TILE = 1;
    const float ang = 10;

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

    private float abs(float a)
    {
        return a >= 0 ? a : -a;
    }

    public void Shake(float mag, float dur)
    {
        if((player.transform.position - transform.position).sqrMagnitude <= 1600.0f && abs(player.transform.position.y - transform.position.y) <= 10.0f) 
        Camera.GetComponent<ShakeCamera>().shakeCamera(dur, mag);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerData = player.GetComponent<PlayerData>();
        Camera = GameObject.Find("Main Camera");

        _up = VectorRotate(_up, transform.rotation.z);
        _down = VectorRotate(_down, transform.rotation.z);
        _right = VectorRotate(_right, transform.rotation.z);
        _left = VectorRotate(_left, transform.rotation.z);

        LeftLeg1 = Instantiate(Legs[0], transform.position + VectorAdd(-0.4583f, -0.4375f, 3), Quaternion.identity); // 왼쪽 다리 뒷부분
        LeftLeg2 = Instantiate(Legs[0], transform.position + VectorAdd(-0.2916f, -0.4375f, 1), Quaternion.identity); // 왼쪽 다리 앞부분
        RightLeg1 = Instantiate(Legs[1], transform.position + VectorAdd(0.2916f, -0.4375f, 3), Quaternion.identity); // 오른쪽 다리 뒷부분
        RightLeg2 = Instantiate(Legs[1], transform.position + VectorAdd(0.4583f, -0.4375f, 1), Quaternion.identity); // 오른쪽 다리 앞부분

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

        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, _right, 3.0f, LayerMask.GetMask("Map"));
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, _left, 3.0f, LayerMask.GetMask("Map"));

        if (isWalking && ((rightRay.collider != null && rightRay.distance < 2.5f && direction == 1) || (leftRay.collider != null && leftRay.distance < 2.5f && direction == -1)))
        {
            preDir = direction;
            timer = Random.Range(3.0f, 5.0f);
            isWalking = false;
            walkN = 0;
            walkState = 0;
            init();
        }

        playerPosition = player.transform.position;
        dis = currPosition - playerPosition;
        dis += VectorAdd(0, -2);
        xDis = Mathf.Abs(dis.x);
        yDis = Mathf.Abs(dis.y);

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
                Dead();
                break;
        }

        if (timer > 0) timer -= Time.deltaTime;
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    private void Move()
    {
        transform.position = transform.position + VectorAdd(walkState % 3 == 0 ? direction * legMovementX * 0.5f : 0, walkState % 3 == 0 ? legMovementY * 0.5f : (walkState % 3 == 1 ? -legMovementY * 10 : 0)) * Time.deltaTime * faster;
        switch (walkState)
        {
            case 0:
                LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster;
                RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster;
                if (timer <= 0) {
                    walkState++; timer = walkCooldown / faster / 20.0f;
                    /*
                    GameObject Attack1 = Instantiate(walkingAttackObj, LeftLeg1.transform.position + VectorAdd(-1.02f, -2.34f), Quaternion.identity);
                    GameObject Attack2 = Instantiate(walkingAttackObj, RightLeg2.transform.position + VectorAdd(1.02f, -2.34f), Quaternion.identity);
                    Attack1.GetComponent<EnemyAttackObj>().init(walkCooldown, attackPower * 0.2f, VectorAdd(direction * legMovementX, -legMovementY) * faster, EnemyAttackObj.EnemyType.Goliath);
                    Attack2.GetComponent<EnemyAttackObj>().init(walkCooldown, attackPower * 0.2f, VectorAdd(direction * legMovementX, -legMovementY) * faster, EnemyAttackObj.EnemyType.Goliath);
                    */
                }
                break;
            case 1:
                LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20;
                RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20;
                if (timer <= 0) { Shake(0.1f, 0.1f); walkState++; timer = walkCooldown / faster; }
                break;
            case 2:
                if (timer <= 0) { walkState++; timer = walkCooldown / faster; }
                break;
            case 3:
                LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster;
                RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster;
                if (timer <= 0) {
                    walkState++; timer = walkCooldown / faster / 20.0f;
                    /*
                    GameObject Attack1 = Instantiate(walkingAttackObj, LeftLeg2.transform.position + VectorAdd(-1.02f, -2.34f), Quaternion.identity);
                    GameObject Attack2 = Instantiate(walkingAttackObj, RightLeg1.transform.position + VectorAdd(1.02f, -2.34f), Quaternion.identity);
                    Attack1.GetComponent<EnemyAttackObj>().init(walkCooldown, attackPower * 0.2f, VectorAdd(direction * legMovementX, -legMovementY) * faster, EnemyAttackObj.EnemyType.Goliath);
                    Attack2.GetComponent<EnemyAttackObj>().init(walkCooldown, attackPower * 0.2f, VectorAdd(direction * legMovementX, -legMovementY) * faster, EnemyAttackObj.EnemyType.Goliath);
                    */
                }
                break;
            case 4:
                LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20;
                RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20;
                if (timer <= 0) { Shake(0.1f, 0.1f); walkState++; timer = walkCooldown / faster; }
                break;
            case 5:
                if (timer <= 0) { walkState++; }
                break;
            case 6:
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
                    direction = 0;
                }
                else
                {
                    if (preDir == 0)
                    {
                        if (walkN == 0)
                        {
                            if (direction == 0) direction = Random.Range(-1, 2);
                            else direction = 0;
                        }
                        if (direction != 0)
                        {
                            if (walkN == 0) walkN = Random.Range(3, 7);
                            timer = walkCooldown / faster;
                            isWalking = true;
                        }
                        else
                        {
                            timer = Random.Range(3.0f, 5.0f);
                        }
                    }
                    else
                    {
                        direction = -preDir;
                        walkN = Random.Range(3, 7);
                        timer = walkCooldown / faster;
                        isWalking = true;
                        preDir = 0;
                    }
                }
            }
        }
    }

    private void Detect()
    {
        if (timer < 0)
        {
            if (detectPlayer())
            {
                walkN = 1;
                state = State.Chasing;
            }
            else
            {
                walkN = 0;
                state = State.Idle;
            }
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
                        isAttacking = false;
                        attackType = Random.Range(1, 2);
                        switch (attackType)
                        {
                            case 1:
                                attackTimer = attackMotion1Cooldown / faster / 1.5f;
                                break;
                            case 2:
                                attackTimer = attack2MotionCooldown;
                                //attack2count = 4;
                                break;
                        }
                        state = State.Attack;
                    }
                }
                else
                {
                    if (attackTimer <= attackCooldown - attackDuration)
                    {
                        isWalking = true;
                        walkN = 1;
                        timer = walkCooldown / faster;
                    }
                }
            }
            else
            {
                state = State.Detection;
                timer = detectionCooldown;
                direction = 0;
            }
        }
    }

    private void Attack()
    {
        switch (attackType)
        {
            case 1:
                if (isAttacking)
                {
                    if (attackTimer <= 0)
                    {
                        init();

                        Vector3 legCenter = direction == 1 ? RightLeg1.transform.position : LeftLeg2.transform.position;
                        legCenter += VectorAdd(direction * 1.1f, -2.3325f);
                        Shake(0.15f, 0.5f);

                        GetComponent<AudioSource>().PlayOneShot(goliathAttackSound, 1.0f);
                        GameObject Attack1 = Instantiate(attackObj2, legCenter + VectorAdd(0.895f, 0), Quaternion.identity);
                        GameObject Attack2 = Instantiate(attackObj2, legCenter + VectorAdd(-0.895f, 0), Quaternion.identity);
                        Attack1.GetComponent<EnemyAttackObj>().init(attackDuration, attackPower * 0.3f, VectorAdd(6, 0), EnemyAttackObj.EnemyType.Goliath);
                        Attack2.GetComponent<EnemyAttackObj>().init(attackDuration, attackPower * 0.3f, VectorAdd(-6, 0), EnemyAttackObj.EnemyType.Goliath);
                        Attack1.GetComponent<SpriteRenderer>().flipX = true;

                        state = State.Chasing;
                        isAttacking = false;
                        attackTimer = attackCooldown;
                    }
                    else
                    {
                        transform.Rotate(0, 0, 20 * ang * Time.deltaTime * direction);
                        if (direction == 1)
                        {
                            RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime;
                            RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime;
                        }
                        else
                        {
                            LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime;
                            LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime;
                        }
                    }
                }
                else
                {
                    if (attackTimer <= 0)
                    {
                        Vector3 legPos = direction == 1 ? RightLeg1.transform.position : LeftLeg2.transform.position;
                        Vector3 tmp = VectorAdd(direction * 1.1f, -2.3325f);
                        GameObject Attack = Instantiate(attackObj1, legPos + tmp, Quaternion.identity);
                        Attack.GetComponent<EnemyAttackObj>().init(attackMotion2Cooldown, attackPower, VectorAdd(0, -legMovementY * 40), EnemyAttackObj.EnemyType.Goliath);

                        attackTimer = attackMotion2Cooldown;
                        isAttacking = true;
                    }
                    else
                    {
                        transform.Rotate(0, 0, ang * Time.deltaTime * direction * faster * 1.5f);
                        if (direction == 1)
                        {
                            RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster;
                            RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster;
                        }
                        else
                        {
                            LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster;
                            LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster;
                        }
                    }
                }
                break;
            case 2:
                
                break;
        }
        
    }

    private void Dead()
    {
        if(timer <= 0)
        {
            Destroy(LeftLeg1);
            Destroy(LeftLeg2);
            Destroy(RightLeg1);
            Destroy(RightLeg2);
            Destroy(GetComponentInChildren<Transform>());
            gameObject.SetActive(false);
        }
    }

    private void init()
    {
        transform.position = new Vector3(transform.position.x, startY, 0);
        LeftLeg1.transform.position = transform.position + VectorAdd(-0.4583f, -0.4375f, 3);
        LeftLeg2.transform.position = transform.position + VectorAdd(-0.2916f, -0.4375f, 2);
        RightLeg1.transform.position = transform.position + VectorAdd(0.2916f, -0.4375f, 3);
        RightLeg2.transform.position = transform.position + VectorAdd(0.4583f, -0.4375f, 2);
        transform.rotation = Quaternion.Euler(0, 0, 0);
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

    private Vector3 VectorRotate(Vector3 tmp, float angle)
    {
        return new Vector3(tmp.x * Mathf.Cos(angle) - tmp.y * Mathf.Sin(angle), tmp.x * Mathf.Sin(angle) + tmp.y * Mathf.Cos(angle), 0);
    }

    private Vector3 VectorAdd(float x, float y, float z = 0)
    {
        return x * _right + y * _up + new Vector3(0, 0, z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TurningPoint") && collision.GetComponent<TurningPointSet>().getType() < 5)
        {
            timer = Random.Range(3.0f, 5.0f);
            isWalking = false;
            walkN = 0;
            init();
        }
    }
}
