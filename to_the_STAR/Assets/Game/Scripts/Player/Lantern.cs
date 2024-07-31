using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    Transform lanternLight;
    GameObject player;
    Vector2 playerMoveVector;
    void Start()
    {
        lanternLight = transform.GetChild(0).gameObject.transform;
        player = GameObject.Find("Player");
        playerMoveVector.x = 0; playerMoveVector.y = 0;
    }

    void Update()
    {
        Vector2 tempV = player.GetComponent<PlayerMovementController>()._moveVector;
        if(tempV.x!=0 || tempV.y!=0) playerMoveVector = tempV;
        transform.position = Vector3.Lerp(transform.position, player.transform.position  - new Vector3(playerMoveVector.x, playerMoveVector.y, 0), 
            Time.deltaTime * player.GetComponent<PlayerMovementController>().moveSpeed);
        lanternLight.position = transform.position;
        lanternLight.rotation = Quaternion.Inverse(player.transform.rotation);
    }
}
