using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackObj : MonoBehaviour
{
    float duration = 0;
    float maxDuration = 1;
    float damage = 0;
    float stunCooldownSet = 0;
    int attackType = 0; //0: 해머, 1: 화염방사기
    Vector2 moveVector;
    Transform lightObj;

    public void init(float t, float d, Vector2 m, int tp, float sc)
    {
        duration = t;
        maxDuration = t;
        damage = d;
        moveVector = m;
        attackType = tp;
        stunCooldownSet = sc;
        if (tp == 1)
        {
            lightObj = transform.GetChild(0);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        transform.position = new Vector3(transform.position.x + moveVector.x * Time.deltaTime, transform.position.y + moveVector.y * Time.deltaTime, transform.position.z);
        if(attackType == 1)
        {
            transform.localScale = new Vector3(duration / maxDuration + 0.1f, duration / maxDuration + 0.1f, 1);
            lightObj.position = transform.position;
        }
        if (duration < 0)
        {
            if(attackType == 1) Destroy(lightObj.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyInterface EI;
        if (collision.CompareTag("Enemy"))
        {
            EI = collision.GetComponent<EnemyInterface>();
            EI.getDamage(damage, stunCooldownSet);
            if (attackType == 1) Destroy(lightObj.gameObject);
            Destroy(gameObject);
        }
    }
}
