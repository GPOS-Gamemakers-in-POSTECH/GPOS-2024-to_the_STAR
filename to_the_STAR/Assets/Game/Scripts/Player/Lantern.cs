using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lantern : MonoBehaviour
{
    GameObject lightComponent;
    GameObject player;
    Vector2 playerMoveVector;
    Vector2 lanternFloatingVectorBase;

    float lanternFloating = 0.5f;
    float playerRot = 0;

    float lanternLightBase = 0.5f;
    float lanternLightChangeRange = 0.05f;

    float hardLightBase = 3.5f;
    float hardLightChangeRange = 0.5f;

    float lightChangeSpeed = 0.66f;
    int lightChangeTimer = 0;

    void Start()
    {
        lightComponent = transform.GetChild(0).gameObject;
        player = GameObject.Find("Player");
        playerMoveVector.x = 0; playerMoveVector.y = 0;
        lanternFloatingVectorBase.x = 0; lanternFloatingVectorBase.y = lanternFloating;
    }

    void Update()
    {
        playerRot = player.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 lanternFloatingVector = new Vector2(lanternFloatingVectorBase.x * Mathf.Cos(playerRot) - lanternFloatingVectorBase.y * Mathf.Sin(playerRot),
                                                    lanternFloatingVectorBase.x * Mathf.Sin(playerRot) + lanternFloatingVectorBase.y * Mathf.Cos(playerRot));
        Vector2 tempV = player.GetComponent<PlayerMovementController>().GetMoveVector();
        if(tempV.x!=0 || tempV.y!=0) playerMoveVector = tempV;
        transform.position = Vector3.Lerp(transform.position, player.transform.position
            - new Vector3(playerMoveVector.x - lanternFloatingVector.x, playerMoveVector.y - lanternFloatingVector.y, 0) * 0.5f, 
            Time.deltaTime * player.GetComponent<PlayerMovementController>().getMoveSpeed());
        transform.rotation = player.transform.rotation;
        lightComponent.transform.position = transform.position;
        lightChangeTimer++;
        lightComponent.GetComponent<Light2D>().falloffIntensity = lanternLightBase - lanternLightChangeRange * Mathf.Sin(lightChangeTimer * lightChangeSpeed * Mathf.Deg2Rad);
        lightComponent.GetComponent<HardLight2D>().Range = hardLightBase + hardLightChangeRange * Mathf.Sin(lightChangeTimer * lightChangeSpeed * Mathf.Deg2Rad);
    }
}
