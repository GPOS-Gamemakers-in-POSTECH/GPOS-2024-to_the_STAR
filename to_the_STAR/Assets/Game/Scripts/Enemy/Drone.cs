using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour, EnemyInterface
{
    [SerializeField] private int floor = 1; //¹Ù´Ú ±âÁØ, 0: up, 1: right, 2: down, 3: left
    float hp = 100;
    float attackPower = 100;
    float detectionRange = 5;
    float speed = 5;

    int direction = 0;
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
        transform.rotation = Quaternion.Euler(0, 0, 90 * floor);
    }

    void Update()
    {
        
    }
}
