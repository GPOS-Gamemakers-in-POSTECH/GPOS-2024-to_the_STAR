using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    Transform playerTransform;
    GameObject player;
    Vector3 cameraPosition = new Vector3(0, 0, -10);
    int cameraMoveSpeed = 2;
    float cameraRotationSpeed = 0.1f;
    float height, width;
    public Vector2 mapSizeMin, mapSizeMax;

    void Start() {
        player = GameObject.Find("Player");
        playerTransform = player.transform;
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);
        float playerRot = playerTransform.rotation.eulerAngles.z;
        float rotateWidth = width;
        float rotateHeight = height;
        if (playerRot > 45 && playerRot < 135 || playerRot > 225 && playerRot < 315) { float tmp = rotateWidth; rotateWidth = rotateHeight; rotateHeight = tmp; }
        float cx = Mathf.Clamp(transform.position.x, mapSizeMin.x + rotateWidth, mapSizeMax.x - rotateWidth);
        float cy = Mathf.Clamp(transform.position.y, mapSizeMin.y + rotateHeight, mapSizeMax.y - rotateHeight);
        transform.position = new Vector3(cx, cy, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, cameraRotationSpeed);
    }
}
