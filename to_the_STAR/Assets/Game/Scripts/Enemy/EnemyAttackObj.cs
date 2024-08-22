using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackObj : MonoBehaviour
{
    public enum EnemyType
    {
        Dustpan,
        Drone,
        Goliath
    };

    private EnemyType enemyType;

    float duration = 0;
    float maxDuration = 1;
    float damage = 0;
    Vector3 moveVector;

    public void init(float t, float d, Vector3 m, EnemyType tp)
    {
        duration = t;
        maxDuration = t;
        damage = d;
        moveVector = m;
        enemyType = tp;
    }

    void Update()
    {
        duration -= Time.deltaTime;
        transform.position = transform.position + moveVector * Time.deltaTime;
        if (duration < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerData _pd = collision.GetComponent<PlayerData>();
            _pd.playerDamage(damage);
            Destroy(gameObject);
        }

        if (enemyType == EnemyType.Drone)
        {
            if (collision.CompareTag("Map") || collision.CompareTag("Door")) Destroy(gameObject);
        }
    }
}
