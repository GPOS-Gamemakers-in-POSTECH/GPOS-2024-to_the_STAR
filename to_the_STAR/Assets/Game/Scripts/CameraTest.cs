using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    private ShakeCamera shakeCameraScript;

    // shaking setting variables
    public float duration = 1f;
    public float magnitudePos = 0.03f;
    public float singleMagnitude = 1f;

    void Start()
    {
        shakeCameraScript = GameObject.Find("Main Camera").GetComponent<ShakeCamera>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            shakeCameraScript.shakeCamera(duration, magnitudePos);
        }                
        if(Input.GetKeyDown(KeyCode.O))
        {
            shakeCameraScript.singleShakeCamera(singleMagnitude);
        }
    }
}
