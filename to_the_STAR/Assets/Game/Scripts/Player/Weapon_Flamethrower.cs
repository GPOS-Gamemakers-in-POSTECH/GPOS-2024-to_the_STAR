using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Flamethrower : MonoBehaviour
{
    [SerializeField] GameObject attackObj;
    bool flameEnabled = true;
    bool flameCooldown = false;
    bool flameTurnedOn = false;
    int flameFever = 0;
    Vector2 playerPos = new Vector2(1, 0);
    const float flameDamageConst = 0.0f;
    const float flameSpeed = 0.1f;
    const float flameDegRange = 30 * Mathf.Deg2Rad;
    const int flameFrequency = 20;
    const int flameFeverMax = 400;
    const int attackDuration = 200;

    void Start()
    {
        
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
            flameFever++;
            if(flameFever%flameFrequency == 0)
            {
                Vector2 mousePos = new Vector2(Input.mousePosition.x - transform.position.x, Input.mousePosition.y - transform.position.y);
                float angle = Vector2.SignedAngle(mousePos, playerPos) * Mathf.Deg2Rad;
                Debug.Log(angle * Mathf.Rad2Deg);
                Vector2 flameMove = new Vector2(playerPos.x * Mathf.Cos(angle) - playerPos.y * Mathf.Sin(angle),
                                                playerPos.x * Mathf.Sin(angle) + playerPos.y * Mathf.Cos(angle));
                GameObject Attack = Instantiate(attackObj, transform.position, Quaternion.identity);
                Attack.GetComponent<PlayerAttackObj>().init(attackDuration, flameDamageConst, flameMove * flameSpeed);
            }
            if(flameFever == flameFeverMax)
            {
                flameTurnedOn = false;
                flameCooldown = true;
            }
        }
        if (!flameTurnedOn)
        {
            flameFever--;
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
