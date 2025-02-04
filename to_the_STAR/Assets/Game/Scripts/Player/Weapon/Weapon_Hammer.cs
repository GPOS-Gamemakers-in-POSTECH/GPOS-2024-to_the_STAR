using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Hammer : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    [SerializeField] AudioClip hammerAttackSound;

    GameObject Camera;
    PlayerMovementController _pmc;
    PlayerData playerData;

    Vector3 playerPos = new Vector3(1, 0, 0);
    bool hammerEnabled = false;
    float hammerCooldown = 0;
    float hammerCharge = 0;
    float hammerChargeTime = 0; // 스태미나를 다 사용해도 계속 늘어나는 값
    const float hammerDamageConst = 50f;
    const float hammerDamageBase = 25f;
    const float hammerCooldownSet = 2.5f;
    const float attackDuration = 0.2f;
    const float hammerChargeMax = 10.0f;
    const float hammerStamina = 0.66f;
    const float hammerRange = 0.7f;
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
    public float getHammerChargeTime()
    {
        return Mathf.Min(hammerChargeTime, hammerChargeMax) / hammerChargeMax;
    }

    public bool isEnabledHammer()
    {
        return hammerEnabled;
    }
    void Start()
    {
        Camera = GameObject.Find("Main Camera");
        playerData = GetComponent<PlayerData>();
        _pmc = GetComponent<PlayerMovementController>();
        _ani = GetComponent<Animator>();
    }

    public bool isMouseInputted()
    {
        return Input.GetMouseButton(0);
    }

    void Update()
    {
        Vector2 playerTmp = _pmc.GetMoveVector();
        if (playerTmp.x != 0 || playerTmp.y != 0)
        {
            playerPos = playerTmp;
        }

        if (hammerEnabled && hammerCooldown < 0 && hammerCharge < hammerChargeMax && Input.GetMouseButton(0))
        {
            if(_pmc.GetStamina() > 0)
            {
                _ani.SetBool("isCharge", true);
                hammerCharge += Time.deltaTime * 6.66f;
                _pmc.SetStamina(_pmc.GetStamina() - Time.deltaTime * hammerStamina);
            }
            hammerChargeTime += Time.deltaTime * 6.66f;
        }
        if(hammerCharge > 0 && Input.GetMouseButtonUp(0))
        {
            // if (_pmc.GetStamina() == 0) _pmc.SetStaminaCool(true);
            _ani.SetBool("isCharge", false);
            _ani.SetTrigger("Attack_Hammer");
            GameObject Attack = Instantiate(attackObj, transform.position + playerPos * hammerRange, Quaternion.identity);
            Attack.GetComponent<PlayerAttackObj>().init(attackDuration, hammerDamageConst * hammerCharge / 10.0f + hammerDamageBase, new Vector2(0, 0), 0, stunCooldownSet * hammerCharge / 10);
            Camera.GetComponent<ShakeCamera>().singleShakeCamera(0.2f + getHammerCharge() * 0.8f, playerData.getRotateDir());
            GetComponent<AudioSource>().PlayOneShot(hammerAttackSound, 1.0f);
            hammerCooldown = hammerCooldownSet;
            hammerCharge = 0;
            hammerChargeTime = 0;
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
