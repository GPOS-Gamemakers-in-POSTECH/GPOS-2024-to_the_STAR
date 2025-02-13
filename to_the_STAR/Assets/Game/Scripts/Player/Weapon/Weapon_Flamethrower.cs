using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class Weapon_Flamethrower : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    [SerializeField] GameObject lightObj;
    [SerializeField] AudioClip flameAttackSound;

    GameObject player;
    bool flameEnabled = true;
    bool flameCooldown = false;
    bool flameTurnedOn = false;
    float flameFever = 0;
    float flameShotCooldown = 0;
    Vector2 playerPos = new Vector2(1, 0);
    Vector2 flamethrowerTipPosX = new Vector2(0.75f, 0);
    Vector2 flamethrowerTipPosY = new Vector2(0, 0.25f);
    const float flameDamageConst = 1.25f;
    const float flameSpeed = 6.0f;
    const float flameDegRange = 30;
    const float flameFrequency = 0.1f;
    const float flameFeverMax = 2.5f;
    const float attackDuration = 0.8f;
    const float sightLineLenght = 4.0f;
    const float flameLightRange = 0.67f;
    const float flameLightIntensity = 0.5f;
    const float flameStamina = 0.5f;

    const float stunCooldownSet = 0;

    PlayerMovementController _pmc;
    LineRenderer lineRenderer;
    Animator[] _ani;
    public void enable()
    {
        flameEnabled = true;
    }

    public void disable()
    {
        flameEnabled = false;
        flameTurnedOn = false;
    }

    public bool isTurnOn()
    {
        return flameTurnedOn;
    }
    public bool isEnabledFlame()
    {
        return flameEnabled;
    }
    public float getFlameFever()
    {
        return flameFever / flameFeverMax;
    }
    public float getFlameCooldown()
    {
        return flameShotCooldown / flameFrequency;
    }

    void Start()
    {
        _pmc = GetComponent<PlayerMovementController>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        player = GameObject.Find("Player");
        _ani = GetComponentsInChildren<Animator>();
    }

    void Update()
    {

        if(GamePause.isGamePaused) return; // If the game is Paused
        Vector2 playerTmp = GetComponent<PlayerMovementController>().GetMoveVector();
        if (playerTmp.x != 0 || playerTmp.y != 0)
        {
            playerPos = playerTmp;
        }
        if (flameEnabled && !flameCooldown && Input.GetMouseButtonDown(0) && !_pmc.IsStaminaCool() && _pmc.GetStamina() > 0)
        {
            if (flameFever < 0)
            {
                flameFever = 0;
            }
            flameTurnedOn = true;
        }
        if(flameTurnedOn && Input.GetMouseButton(0))
        {
            flameFever += Time.deltaTime;
            _pmc.SetStamina(_pmc.GetStamina() - Time.deltaTime * flameStamina);
            if(flameShotCooldown < 0)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = new Vector2(mousePos.x - player.transform.position.x, mousePos.y - player.transform.position.y);

                float angle = Vector2.SignedAngle(playerPos, mousePos);
                if ((playerPos.x + playerPos.y > 0) ^ (transform.rotation.eulerAngles.z == 180 || transform.rotation.eulerAngles.z == 270)) angle = Mathf.Clamp(angle, -flameDegRange, 90);
                else angle = Mathf.Clamp(angle, -90, flameDegRange);
                angle *= Mathf.Deg2Rad;
                Vector2 flameMove = VectorRotate(playerPos, angle);

                GetComponent<AudioSource>().PlayOneShot(flameAttackSound, 1.0f);
                Vector3 flameTip = 
                    VectorRotate(flamethrowerTipPosX, (transform.rotation.eulerAngles.z + (GetComponent<PlayerMovementController>().flipX ? 0 : 180)) * Mathf.Deg2Rad + angle)
                  + VectorRotate(flamethrowerTipPosY, transform.rotation.eulerAngles.z * Mathf.Deg2Rad + angle);
                GameObject Attack = Instantiate(attackObj, transform.position + flameTip, Quaternion.Euler(transform.eulerAngles));
                GameObject Light = Instantiate(lightObj, transform.position + flameTip, Quaternion.identity);
                Light.transform.parent = Attack.transform;
                Light.GetComponent<HardLight2D>().Range = flameLightRange;
                Light.GetComponent<HardLight2D>().Intensity = flameLightIntensity;
                Light.GetComponent<Light2D>().pointLightInnerRadius = 0;
                Light.GetComponent<Light2D>().pointLightOuterRadius = flameLightRange;
                Light.GetComponent<Light2D>().intensity = flameLightIntensity;
                Attack.GetComponent<PlayerAttackObj>().init(attackDuration, flameDamageConst, flameMove * flameSpeed, 1, stunCooldownSet);
                flameShotCooldown = flameFrequency;
            }
            if (flameFever > flameFeverMax)
            {
                flameTurnedOn = false;
                flameCooldown = true;
            }
            if (_pmc.GetStamina() == 0)
            {
                flameTurnedOn = false;
                _pmc.SetStaminaCool(true);
            }
            flameShotCooldown -= Time.deltaTime;
        }
        if (!flameTurnedOn)
        {
            for (int i = 0; i < _ani.Length; i++)
                _ani[i].SetBool("Attack_Flamethrower", false);
            lineRenderer.enabled = false;
            flameFever -= Time.deltaTime;
        }
        else
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector2(mousePos.x - player.transform.position.x, mousePos.y - player.transform.position.y);
            float angle = Vector2.SignedAngle(playerPos, mousePos);
            if ((playerPos.x + playerPos.y > 0) ^ (transform.rotation.eulerAngles.z == 180 || transform.rotation.eulerAngles.z == 270)) angle = Mathf.Clamp(angle, -flameDegRange, 90);
            else angle = Mathf.Clamp(angle, -90, flameDegRange);
            for (int i = 0; i < _ani.Length; i++)
            {
                _ani[i].SetBool("Attack_Flamethrower", true);
                int angleNeg = GetComponent<PlayerMovementController>().flipX ? 1 : -1;
                _ani[i].SetFloat("Flamethrower_Angle", angleNeg * angle);
            }
            angle *= Mathf.Deg2Rad;
            Vector2 flameMove = VectorRotate(playerPos, angle);               
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, transform.position + new Vector3(0, 0, 1));
            lineRenderer.SetPosition(1, transform.position + new Vector3(flameMove.x * sightLineLenght, flameMove.y * sightLineLenght, 1));
        }
        if(flameFever < 0)
        {
            flameCooldown = false;
        }
        if (Input.GetMouseButtonUp(0) && flameTurnedOn)
        {
            flameTurnedOn = false;
        }
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
