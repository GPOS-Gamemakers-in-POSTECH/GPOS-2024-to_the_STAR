using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hammer : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    bool hammerEnabled = false;
    float hammerCooldown = 0;
    float hammerCharge = 0;
    const float hammerDamageConst = 100.0f;
    const float hammerCooldownSet = 2.5f;
    const float attackDuration = 1.0f;
    const float hammerChargeMax = 10.0f;

    const float stunCooldownSet = 3.0f;

    Animator _ani;

    public void enable()
    {
        hammerEnabled = true;
    }

    public void disable()
    {
        hammerEnabled = false;
    }

    public bool isCharging()
    {
        return hammerCharge != 0;
    }

    public float getHammerCooldown()
    {
        return hammerCooldown / hammerCooldownSet;
    }
    public float getHammerCharge()
    {
        return hammerCharge / hammerChargeMax;
    }

    public bool isEnabledHammer()
    {
        return hammerEnabled;
    }
    void Start()
    {
        _ani = GetComponent<Animator>();
    }

    void Update()
    {
        if(hammerEnabled && hammerCooldown < 0 && hammerCharge < hammerChargeMax && Input.GetMouseButton(0))
        {
            hammerCharge += Time.deltaTime;
        }
        if(hammerCharge > 0 && Input.GetMouseButtonUp(0))
        {
            _ani.SetTrigger("Attack_Hammer");
            GameObject Attack = Instantiate(attackObj, transform.position, Quaternion.identity);
            Attack.GetComponent<PlayerAttackObj>().init(attackDuration, hammerDamageConst * hammerCharge / 10.0f, new Vector2(0, 0), 0, stunCooldownSet * hammerCharge / 10);
            hammerCooldown = hammerCooldownSet;
            hammerCharge = 0;
        }
        hammerCooldown -= Time.deltaTime;
    }
}