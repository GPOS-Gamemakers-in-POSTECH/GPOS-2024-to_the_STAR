using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private Transform camera;
    private Vector3 originalPos;
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    void Update()
    {
        originalPos = camera.localPosition;
    }

    // Shake camera one time with given magnitude
    private IEnumerator singleShake(float magnitude)
    {
        // Get one shaked position using random method
        Vector2 shakePos2D = Random.insideUnitCircle;
        Vector3 shakePos = new Vector3(shakePos2D.x, shakePos2D.y, 0);
        camera.localPosition = originalPos + shakePos * magnitude;
        yield return null;
        camera.localPosition = originalPos;
    }

    // Shake camera during given duration and with give magnitude
    private IEnumerator Shake(float duration, float magnitudePos)
    {
        float passedTime = 0.0f;

        // During duration, shake camera using same method of singleShake
        while(passedTime < duration)
        {
            Vector2 shakePos2D = Random.insideUnitCircle;
            Vector3 shakePos = new Vector3(shakePos2D.x, shakePos2D.y, 0);
            camera.localPosition = originalPos + shakePos * magnitudePos;
            passedTime += Time.deltaTime;
            yield return null;
        }
        camera.localPosition = originalPos;
    }

    // Function that calls singleShake
    public void singleShakeCamera(float magnitude = 1f)
    {
        StartCoroutine(singleShake(magnitude));
    }

    // Function that calls Shake
    public void shakeCamera(float duration = 0.1f, float magnitude = 0.03f)
    {
        StartCoroutine(Shake(duration, magnitude));
    }
}
