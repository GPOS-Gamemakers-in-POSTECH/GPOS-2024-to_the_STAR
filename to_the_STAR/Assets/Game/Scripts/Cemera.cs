using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Transform shakeCamera;
    public bool shakeRotate = false;
    private Vector3 originPos;
    private Quaternion originRot;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Shake());
        }
    }

    public IEnumerator Shake(float duration = 1f, float magnitudePos = 0.03f, float magnitudeRot = 0.1f)
    {
        float passedTime = 0.0f;

        while(passedTime < duration)
        {
            Vector2 shakePos2D = Random.insideUnitCircle;
            Vector3 shakePos = new Vector3(shakePos2D.x, shakePos2D.y, 0);
            shakeCamera.localPosition = originPos + shakePos * magnitudePos;

            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
            }

            passedTime += Time.deltaTime;
            yield return null;
        }

        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;
    }

}
