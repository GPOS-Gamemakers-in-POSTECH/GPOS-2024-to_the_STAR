using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Transform playerTransform;
    Vector3 cameraPosition = new Vector3(0, 0, -10);
    int cameraMoveSpeed = 2;

    // Start is called before the first frame update
    void Start() {
        playerTransform = GameObject.Find("Player").transform;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraPosition, Time.deltaTime * cameraMoveSpeed);
    }
}
