using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hammer : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    Vector3 playerPos = new Vector3(1, 0, 0);
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

    public bool isMouseInputted()
    {
        return Input.GetMouseButton(0);
    }

    void Update()
    {
        Vector2 playerTmp = GetComponent<PlayerMovementController>().getMoveVector();
        if (playerTmp.x != 0 || playerTmp.y != 0)
        {
            playerPos = playerTmp;
        }

        if (hammerEnabled && hammerCooldown < 0 && hammerCharge < hammerChargeMax && Input.GetMouseButton(0))
        {
            hammerCharge += Time.deltaTime;
        }
        if(hammerCharge > 0 && Input.GetMouseButtonUp(0))
        {
            _ani.SetTrigger("Attack_Hammer");
            GameObject Attack = Instantiate(attackObj, transform.position + playerPos, Quaternion.identity);
            Attack.GetComponent<PlayerAttackObj>().init(attackDuration, hammerDamageConst * hammerCharge / 10.0f, new Vector2(0, 0), 0, stunCooldownSet * hammerCharge / 10);
            hammerCooldown = hammerCooldownSet;
            hammerCharge = 0;
        }
        hammerCooldown -= Time.deltaTime;
    }

    private Vector2 VectorRotate(Vector2 tmp, float angle)
    {
        return new Vector2(tmp.x * Mathf.Cos(angle) - tmp.y * Mathf.Sin(angle), tmp.x * Mathf.Sin(angle) + tmp.y * Mathf.Cos(angle));
    }

    public void DirVectorUpdate(float angle)
    {
        playerPos = VectorRotate(playerPos, angle * Mathf.Deg2Rad);
    }
}
