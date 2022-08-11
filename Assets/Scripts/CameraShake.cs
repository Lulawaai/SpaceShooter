using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraShake : MonoBehaviour
{
    private Vector3 _originPos = new Vector3(0, 0, -10);

    private void OnEnable()
    {
        Player.OnDamageCameraShake += CameraMove;
    }

    private void CameraMove(float magnitude, float duration)
    {
        StartCoroutine(ShakeRoutine(magnitude, duration)); 
    }

    private IEnumerator ShakeRoutine(float magnitude, float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float xOffset = UnityEngine.Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = UnityEngine.Random.Range(-0.5f, 0.5f) * magnitude;

            transform.position = new Vector3(xOffset, yOffset, -10);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = _originPos;
    }

    private void OnDisable()
    {
        Player.OnDamageCameraShake -= CameraMove;
    }
}
