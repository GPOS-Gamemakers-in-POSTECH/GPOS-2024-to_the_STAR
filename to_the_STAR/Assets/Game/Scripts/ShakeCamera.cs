using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private Transform camera;
    private Vector3 originPos;
    void Start()
    {
        camera = GameObject.Find("Main Camera").GetComponent<Transform>();  // automatically detect camera object
    }

    void Update()
    {
        originPos = camera.localPosition;   // save current camera position in order to return
    }

    public IEnumerator singleShake(float magnitudePos)
    {
        Vector2 shakePos2D = Random.insideUnitCircle;
        Vector3 shakePos = new Vector3(shakePos2D.x, shakePos2D.y, 0);
        camera.localPosition = originPos + shakePos * magnitudePos;
        yield return null;
        camera.localPosition = originPos;        
    }

    public IEnumerator Shake(float duration, float magnitudePos)
    {
        float passedTime = 0.0f;

        while(passedTime < duration)
        {
            Vector2 shakePos2D = Random.insideUnitCircle;
            Vector3 shakePos = new Vector3(shakePos2D.x, shakePos2D.y, 0);
            camera.localPosition = originPos + shakePos * magnitudePos;
            passedTime += Time.deltaTime;
            yield return null;
        }
        camera.localPosition = originPos;
    }

    public void shakeCamera(float duration = 0.1f, float magnitudePos = 0.03f)
    {
        StartCoroutine(Shake(duration, magnitudePos));
    }

    public void singleShakeCamera(float magnitudePos = 0.03f)
    {
        StartCoroutine(singleShake(magnitudePos));
    }
}
