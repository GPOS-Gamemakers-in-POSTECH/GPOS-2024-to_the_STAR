using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, EnemyInterface
{
    [SerializeField] private int floor = 1; //바닥 기준, 0: up, 1: right, 2: down, 3: left
    float hp = 100;
    float attackPower = 100;
    float detectionRange = 5;
    float speed = 5;

    int direction = 0; //-1 후진 0 정지 1 전진
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
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * floor - 180);
        moveVector.x = -(floor % 2 - 1) * speed; moveVector.y = (floor % 2) * speed;
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle: break;
            case State.Detection: break;
            case State.Chasing: break;
            case State.Attack: break;
            case State.Stunned: break;
            case State.Dead: break;
        }
        if (hp < 0) state = State.Dead;
    }
}
