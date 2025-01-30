using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour
{
    private ShakeCamera shakeCameraScript;

    // shaking setting variables
    public float duration = 1f;
    public float magnitude = 0.03f;
    public float singleMagnitude = 1f;

    void Start()
    {
        shakeCameraScript = GameObject.Find("Main Camera").GetComponent<ShakeCamera>();
    }

    void Update()
    {
        // Test : If press O, shake once. If press P, shake longer.
        if(Input.GetKeyDown(KeyCode.O))
        {
            shakeCameraScript.singleShakeCamera(singleMagnitude);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            shakeCameraScript.shakeCamera(duration, magnitude);
        }                
    }
}
