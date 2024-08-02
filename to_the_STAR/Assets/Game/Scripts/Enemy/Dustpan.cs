using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dustpan : MonoBehaviour, EnemyInterface
{
    private Rigidbody2D _rb;

    private Vector2 _moveVector;
    private float hp = 10.0f;
    private float attackPower = 1.0f;
    private float range = 3.0f;
    private float speed = 0.3f;
    private int floor = 0;

    public void getDamage(float damage)
    {
        hp -= damage;
    }

    public float attack()
    {
        return 0;
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 currPosition = transform.position;
        _rb.MovePosition(currPosition + _moveVector * (speed * Time.deltaTime));
    }

    void Update()
    {
        
    }
}
