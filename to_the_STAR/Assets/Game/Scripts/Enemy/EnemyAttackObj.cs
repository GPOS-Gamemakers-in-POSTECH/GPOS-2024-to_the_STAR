using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackObj : MonoBehaviour
{
    public enum EnemyType
    {
        Dustpan,
        Drone
    };

    private EnemyType enemyType;

    float duration = 0;
    float maxDuration = 1;
    float damage = 0;
    Vector2 moveVector;

    public void init(float t, float d, Vector2 m, EnemyType tp)
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
        transform.position = new Vector3(transform.position.x + moveVector.x * Time.deltaTime, transform.position.y + moveVector.y * Time.deltaTime, transform.position.z);
        if (duration < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Game.Player.PlayerData _pd;
        if (collision.CompareTag("Player"))
        {
            _pd = collision.GetComponent<Game.Player.PlayerData>();
            _pd.playerDamage(damage);
            Destroy(gameObject);
        }
    }
}
