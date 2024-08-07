using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    
    float hp;
    Vector3 barPrintPos = new Vector3(0, -0.5f);

    void Start()
    {
        
    }

    void Update()
    {
        hp = GetComponent<EnemyInterface>().hpRatio();
        Vector3 position = transform.position + barPrintPos;

    }
}
