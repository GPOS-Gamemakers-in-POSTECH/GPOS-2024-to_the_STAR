using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackObj : MonoBehaviour
{
    int duration = 0;
    float damage = 0;
    int attackType = 0;
    Vector2 moveVector;

    public void init(int t, float d, Vector2 m)
    {
        duration = t;
        damage = d;
        moveVector = m;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        duration--;
        transform.position = new Vector3(transform.position.x + moveVector.x, transform.position.y + moveVector.y, transform.position.z);
        if (duration < 0)
        {
            Destroy(gameObject);
        }
    }
}
