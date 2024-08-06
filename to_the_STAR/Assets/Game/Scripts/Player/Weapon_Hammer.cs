using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hammer : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    bool hammerEnabled = false;
    int hammerCooldown = 0;
    int hammerCharge = 0;
    const float hammerDamageConst = 0.0f;
    const int hammerCooldownSet = 100;
    const int attackDuration = 100;

    void Start()
    {

    }

    
    void Update()
    {
        if(hammerEnabled && hammerCooldown < 0 && hammerCharge < 100 && Input.GetMouseButton(0))
        {
            hammerCharge++;
        }
        if(hammerCharge > 0 && Input.GetMouseButtonUp(0))
        {
            GameObject Attack = Instantiate(attackObj, transform.position, Quaternion.identity);
            Attack.GetComponent<PlayerAttackObj>().init(attackDuration, hammerDamageConst * hammerCharge / 100, new Vector2(0, 0));
            hammerCooldown = hammerCooldownSet;
            hammerCharge = 0;
        }
        hammerCooldown--;
    }
}
