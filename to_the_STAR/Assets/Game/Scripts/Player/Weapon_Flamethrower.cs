using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Weapon_Flamethrower : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    [SerializeField] GameObject lightObj;
    bool flameEnabled = true;
    bool flameCooldown = false;
    bool flameTurnedOn = false;
    float flameFever = 0;
    float flameShotCooldown = 0;
    Vector2 playerPos = new Vector2(1, 0);
    const float flameDamageConst = 1.0f;
    const float flameSpeed = 12.0f;
    const float flameDegRange = 30 * Mathf.Deg2Rad;
    const float flameFrequency = 0.1f;
    const float flameFeverMax = 2.5f;
    const float attackDuration = 2.5f;
    const float sightLineLenght = 4.0f;
    const float flameLightRange = 1.0f;

    const float stunCooldownSet = 0;

    LineRenderer lineRenderer;

    public float getFlameFever()
    {
        return flameFever;
    }
    public float getFlameCooldown()
    {
        return flameShotCooldown;
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        Vector2 playerTmp = GetComponent<PlayerMovementController>().getMoveVector();
        if (playerTmp.x != 0 || playerTmp.y != 0)
        {
            playerPos = playerTmp;
        }
        if (flameEnabled && !flameCooldown && Input.GetMouseButtonDown(0))
        {
            flameFever = 0;
            flameTurnedOn = true;
        }
        if(flameTurnedOn && Input.GetMouseButton(0))
        {
            flameFever += Time.deltaTime;
            if(flameShotCooldown < 0)
            {
                Vector2 mousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, - Input.mousePosition.y + Screen.height / 2);
                float angle = Vector2.SignedAngle(mousePos, playerPos) * Mathf.Deg2Rad;
                angle = Mathf.Clamp(angle, -flameDegRange, flameDegRange);
                Vector2 flameMove = new Vector2(playerPos.x * Mathf.Cos(angle) - playerPos.y * Mathf.Sin(angle),
                                                playerPos.x * Mathf.Sin(angle) + playerPos.y * Mathf.Cos(angle));
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position + new Vector3(flameMove.x * sightLineLenght, flameMove.y * sightLineLenght, 0));
                GameObject Attack = Instantiate(attackObj, transform.position, Quaternion.identity);
                GameObject Light = Instantiate(lightObj, transform.position, Quaternion.identity);
                Light.transform.parent = Attack.transform;
                Light.GetComponent<HardLight2D>().Range = flameLightRange;
                Light.GetComponent<Light2D>().pointLightInnerRadius = 0;
                Light.GetComponent<Light2D>().pointLightOuterRadius = flameLightRange;
                Attack.GetComponent<PlayerAttackObj>().init(attackDuration, flameDamageConst, flameMove * flameSpeed, 1, stunCooldownSet);
                flameShotCooldown = flameFrequency;
            }
            if(flameFever > flameFeverMax)
            {
                flameTurnedOn = false;
                flameCooldown = true;
            }
            flameShotCooldown -= Time.deltaTime;
        }
        if (!flameTurnedOn)
        {
            lineRenderer.enabled = false;
            flameFever -= Time.deltaTime;
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
}
