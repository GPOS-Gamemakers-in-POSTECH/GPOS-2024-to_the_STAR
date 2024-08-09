using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hammer : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    bool hammerEnabled = true;
    float hammerCooldown = 0;
    float hammerCharge = 0;
    const float hammerDamageConst = 100.0f;
    const float hammerCooldownSet = 2.5f;
    const float attackDuration = 1.0f;

    void Start()
    {

    }

    
    void Update()
    {
        if(hammerEnabled && hammerCooldown < 0 && hammerCharge < 10 && Input.GetMouseButton(0))
        {
            hammerCharge += Time.deltaTime;
        }
        if(hammerCharge > 0 && Input.GetMouseButtonUp(0))
        {
            GameObject Attack = Instantiate(attackObj, transform.position, Quaternion.identity);
            Attack.GetComponent<PlayerAttackObj>().init(attackDuration, hammerDamageConst * hammerCharge / 10.0f, new Vector2(0, 0), 0);
            hammerCooldown = hammerCooldownSet;
            hammerCharge = 0;
        }
        hammerCooldown -= Time.deltaTime;
    }
}
