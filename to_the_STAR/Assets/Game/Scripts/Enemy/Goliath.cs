using Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class Goliath : MonoBehaviour, EnemyInterface
{
    private Rigidbody2D _rb;

    [SerializeField] private int floor;
    [SerializeField] private float startY;
    [SerializeField] private GameObject attackObj1;
    [SerializeField] private GameObject attackObj2;
    [SerializeField] private GameObject walkingAttackObj;
    [SerializeField] private AudioClip goliathAttackSound;
    [SerializeField] private AudioClip goliathWalkSound;
    [SerializeField] private AudioClip enemyHitSound;

    [SerializeField] private GameObject[] Legs;

    GameObject LeftLeg1;
    GameObject LeftLeg2;
    GameObject RightLeg1;
    GameObject RightLeg2;
    GameObject weakPoint;

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
    private const float detectionRange = 15.0f;
    private const float detectionCooldown = 2.0f;
    private const float attackRange = 2.0f;
    private const float attack2Range = 9.0f;
    private const float attackMotion1Cooldown = 0.8f;
    private const float attackMotion2Cooldown = 0.05f;
    private const float attack2MotionCooldown = 0.5f;
    private const float attackDuration = 1.5f;
    private const float attackCooldown = 4.0f;
    private const float rangedAttackCooldown = 10.0f;
    private const float attackPower = 60.0f;
    private float hp = 1000;

    private int direction = 0;
    private int preDir = 0;
    private float attackTimer = 0;
    private float rangedAttackTimer = 0;
    private float timer = 0;
    private int attackType = 0;
    private bool rangedAttack = false;
    //private float attack2count = 0;

    private const float deadCooldown = 3;

    private const float legMovementX = 1.6f;
    private const float legMovementY = 0.7f; // 0.5f
    private const float walkCooldown = 0.3f;
    private const float yCorrection = 3.635f; // y 좌표 보정값은 플레이어 기준..
    private const float size = 1.5f;

    private Vector2 playerPosition;
    private float xDis;
    private float yDis;

    private bool isAttacking = false;
    private bool isWalking = false;
    private int walkN = 0;
    private int walkState = 0;

    const float TILE = 4;
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
            GetComponent<AudioSource>().PlayOneShot(enemyHitSound, 1.0f);
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

    private bool IsPlayerClose()
    {
        return (player.transform.position - transform.position).sqrMagnitude <= 400f && abs(player.transform.position.y - transform.position.y) <= 10.0f;
    }

    private void Shake(float mag, float dur)
    {
        if(IsPlayerClose()) Camera.GetComponent<ShakeCamera>().shakeCamera(dur, mag);
    }

    void Awake()
    {
        weakPoint = transform.GetChild(0).gameObject;
        startY += yCorrection;
        transform.position = new Vector3(transform.position.x, startY, transform.position.z);
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerData = player.GetComponent<PlayerData>();
        Camera = GameObject.Find("Main Camera");

        _up = VectorRotate(_up, transform.rotation.z);
        _down = VectorRotate(_down, transform.rotation.z);
        _right = VectorRotate(_right, transform.rotation.z);
        _left = VectorRotate(_left, transform.rotation.z);

        LeftLeg1 = Instantiate(Legs[0], transform.position + VectorAdd(-0.4583f, -0.4375f, 3) * size, Quaternion.identity); // 왼쪽 다리 뒷부분
        LeftLeg2 = Instantiate(Legs[0], transform.position + VectorAdd(-0.2916f, -0.4375f, 1) * size, Quaternion.identity); // 왼쪽 다리 앞부분
        RightLeg1 = Instantiate(Legs[1], transform.position + VectorAdd(0.2916f, -0.4375f, 3) * size, Quaternion.identity); // 오른쪽 다리 뒷부분
        RightLeg2 = Instantiate(Legs[1], transform.position + VectorAdd(0.4583f, -0.4375f, 1) * size, Quaternion.identity); // 오른쪽 다리 앞부분

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

        RaycastHit2D rightRay = Physics2D.Raycast(transform.position, _right, 3.0f * size, LayerMask.GetMask("Map"));
        RaycastHit2D leftRay = Physics2D.Raycast(transform.position, _left, 3.0f * size, LayerMask.GetMask("Map"));

        if (isWalking && ((rightRay.collider != null && rightRay.distance <= 6.0f && direction == 1) || (leftRay.collider != null && leftRay.distance <= 8.0f && direction == -1)))
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
        if (rangedAttackTimer > 0) rangedAttackTimer -= Time.deltaTime;
    }

    private void Move()
    {
        transform.position = transform.position + VectorAdd(walkState % 3 == 0 ? direction * legMovementX * 0.5f : 0, walkState % 3 == 0 ? legMovementY * 0.5f : (walkState % 3 == 1 ? -legMovementY * 10 : 0)) * Time.deltaTime * faster * size;
        switch (walkState)
        {
            case 0:
                LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster * size;
                RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster * size;
                if (timer <= 0) {
                    walkState++; timer = walkCooldown / faster / 20.0f;
                }
                break;
            case 1:
                LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20 * size;
                RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20 * size;
                if (timer <= 0) {
                    GameObject Attack1 = Instantiate(walkingAttackObj, LeftLeg1.transform.position + VectorAdd(-1.02f, -2.34f) * size, Quaternion.identity);
                    GameObject Attack2 = Instantiate(walkingAttackObj, RightLeg2.transform.position + VectorAdd(1.02f, -2.34f) * size, Quaternion.identity);
                    Attack1.GetComponent<EnemyAttackObj>().init(0.2f, attackPower * 0.1f, new Vector3(), EnemyAttackObj.EnemyType.Goliath);
                    Attack2.GetComponent<EnemyAttackObj>().init(0.2f, attackPower * 0.1f, new Vector3(), EnemyAttackObj.EnemyType.Goliath);
                    transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                    LeftLeg1.transform.position = new Vector3(LeftLeg1.transform.position.x, startY - 0.4375f * size, LeftLeg1.transform.position.z);
                    RightLeg2.transform.position = new Vector3(RightLeg2.transform.position.x, startY - 0.4375f * size, RightLeg2.transform.position.z);
                    if (IsPlayerClose()) GetComponent<AudioSource>().PlayOneShot(goliathWalkSound, 1.0f);
                    Shake(0.1f, 0.1f); walkState++; timer = walkCooldown / faster;
                }
                break;
            case 2:
                if (timer <= 0) { walkState++; timer = walkCooldown / faster; }
                break;
            case 3:
                LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster * size;
                RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(direction * legMovementX, legMovementY) * Time.deltaTime * faster * size;
                if (timer <= 0) {
                    walkState++; timer = walkCooldown / faster / 20.0f;
                }
                break;
            case 4:
                LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20 * size;
                RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, -legMovementY) * Time.deltaTime * faster * 20 * size;
                if (timer <= 0) {
                    GameObject Attack1 = Instantiate(walkingAttackObj, LeftLeg2.transform.position + VectorAdd(-1.02f, -2.34f) * size, Quaternion.identity);
                    GameObject Attack2 = Instantiate(walkingAttackObj, RightLeg1.transform.position + VectorAdd(1.02f, -2.34f) * size, Quaternion.identity);
                    Attack1.GetComponent<EnemyAttackObj>().init(0.2f, attackPower * 0.1f, new Vector3(), EnemyAttackObj.EnemyType.Goliath);
                    Attack2.GetComponent<EnemyAttackObj>().init(0.2f, attackPower * 0.1f, new Vector3(), EnemyAttackObj.EnemyType.Goliath);
                    transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                    LeftLeg2.transform.position = new Vector3(LeftLeg2.transform.position.x, startY - 0.4375f * size, LeftLeg2.transform.position.z);
                    RightLeg1.transform.position = new Vector3(RightLeg1.transform.position.x, startY - 0.4375f * size, RightLeg1.transform.position.z);
                    if (IsPlayerClose()) GetComponent<AudioSource>().PlayOneShot(goliathWalkSound, 1.0f);
                    Shake(0.1f, 0.1f); walkState++; timer = walkCooldown / faster;
                }
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
                if (distance <= attackRange * size)
                {
                    if (attackTimer <= 0)
                    {
                        rangedAttack = false;
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
                }else if (distance <= attack2Range && rangedAttackTimer <= 0)
                {
                    rangedAttack = true;
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
                        legCenter += VectorAdd(direction * 1.1f, -2.3325f) * size;
                        Shake(0.15f, 0.5f);
                        if(IsPlayerClose()) GetComponent<AudioSource>().PlayOneShot(goliathAttackSound, 1.0f);

                        GameObject Attack = Instantiate(attackObj1, legCenter, Quaternion.identity);
                        Attack.GetComponent<EnemyAttackObj>().init(0.2f, attackPower, new Vector3(), EnemyAttackObj.EnemyType.Goliath);

                        GameObject Attack1 = Instantiate(attackObj2, legCenter + VectorAdd(0.895f, 0) * size, Quaternion.identity);
                        GameObject Attack2 = Instantiate(attackObj2, legCenter + VectorAdd(-0.895f, 0) * size, Quaternion.identity);
                        Attack1.GetComponent<EnemyAttackObj>().init(attackDuration, attackPower * 0.4f, VectorAdd(6, 0), EnemyAttackObj.EnemyType.Goliath);
                        Attack2.GetComponent<EnemyAttackObj>().init(attackDuration, attackPower * 0.4f, VectorAdd(-6, 0), EnemyAttackObj.EnemyType.Goliath);
                        Attack1.GetComponent<SpriteRenderer>().flipX = true;

                        state = State.Chasing;
                        isAttacking = false;
                        if (rangedAttack) rangedAttackTimer = rangedAttackCooldown;
                        else attackTimer = attackCooldown;
                    }
                    else
                    {
                        transform.Rotate(0, 0, 20 * ang * Time.deltaTime * direction);
                        if (direction == 1)
                        {
                            RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime * size;
                            RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime * size;
                        }
                        else
                        {
                            LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime * size;
                            LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, -legMovementY * 40) * Time.deltaTime * size;
                        }
                    }
                }
                else
                {
                    if (attackTimer <= 0)
                    {

                        attackTimer = attackMotion2Cooldown;
                        isAttacking = true;
                    }
                    else
                    {
                        transform.Rotate(0, 0, ang * Time.deltaTime * direction * faster * 1.5f);
                        if (direction == 1)
                        {
                            RightLeg1.transform.position = RightLeg1.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster * size;
                            RightLeg2.transform.position = RightLeg2.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster * size;
                        }
                        else
                        {
                            LeftLeg1.transform.position = LeftLeg1.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster * size;
                            LeftLeg2.transform.position = LeftLeg2.transform.position + VectorAdd(0, legMovementY * 3) * Time.deltaTime * faster * size;
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
            LeftLeg1.GetComponent<GoliathLeg>().Dead();
            LeftLeg2.GetComponent<GoliathLeg>().Dead();
            RightLeg1.GetComponent<GoliathLeg>().Dead();
            RightLeg2.GetComponent<GoliathLeg>().Dead();
            weakPoint.GetComponent<GoliathWeakPoint>().Dead();

            if (_sr.color.a == 0) gameObject.SetActive(false);
            _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, (_sr.color.a - 0.005f) > 0 ? (_sr.color.a - 0.005f) : 0);
        }
    }

    private void init()
    {
        transform.position = new Vector3(transform.position.x, startY, 0);
        LeftLeg1.transform.position = transform.position + VectorAdd(-0.4583f, -0.4375f, 3) * size;
        LeftLeg2.transform.position = transform.position + VectorAdd(-0.2916f, -0.4375f, 2) * size;
        RightLeg1.transform.position = transform.position + VectorAdd(0.2916f, -0.4375f, 3) * size;
        RightLeg2.transform.position = transform.position + VectorAdd(0.4583f, -0.4375f, 2) * size;
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
