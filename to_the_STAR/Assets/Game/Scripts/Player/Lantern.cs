using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    Transform lanternLight;
    GameObject player;
    Vector2 playerMoveVector;
    Vector2 lanternFloatingVectorBase;
    float lanternFloating = 0.5f;
    float playerRot = 0;
    void Start()
    {
        lanternLight = transform.GetChild(0).gameObject.transform;
        player = GameObject.Find("Player");
        playerMoveVector.x = 0; playerMoveVector.y = 0;
        lanternFloatingVectorBase.x = 0; lanternFloatingVectorBase.y = lanternFloating;
    }

    void Update()
    {
        playerRot = player.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 lanternFloatingVector = new Vector2(lanternFloatingVectorBase.x * Mathf.Cos(playerRot) - lanternFloatingVectorBase.y * Mathf.Sin(playerRot),
                                                    lanternFloatingVectorBase.x * Mathf.Sin(playerRot) + lanternFloatingVectorBase.y * Mathf.Cos(playerRot));
        Vector2 tempV = player.GetComponent<PlayerMovementController>().getMoveVector();
        if(tempV.x!=0 || tempV.y!=0) playerMoveVector = tempV;
        transform.position = Vector3.Lerp(transform.position, player.transform.position
            - new Vector3(playerMoveVector.x - lanternFloatingVector.x, playerMoveVector.y - lanternFloatingVector.y, 0), 
            Time.deltaTime * player.GetComponent<PlayerMovementController>().getMoveSpeed());
        lanternLight.position = transform.position;
    }
}
