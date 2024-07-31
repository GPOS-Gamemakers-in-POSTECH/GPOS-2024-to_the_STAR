using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    Transform playerTransform;
    Vector3 cameraPosition = new Vector3(0, 0, -10);
    int cameraMoveSpeed = 2;
    float cameraRotationSpeed = 0.1f;
    float height, width;
    public Vector2 mapSizeMin, mapSizeMax;

    void Start() {
        playerTransform = GameObject.Find("Player").transform;
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);
        float cx = Mathf.Clamp(transform.position.x, mapSizeMin.x, mapSizeMax.x);
        float cy = Mathf.Clamp(transform.position.y, mapSizeMin.y, mapSizeMax.y);
        transform.position = new Vector3(cx, cy, transform.position.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTransform.rotation, cameraRotationSpeed);
    }
}
