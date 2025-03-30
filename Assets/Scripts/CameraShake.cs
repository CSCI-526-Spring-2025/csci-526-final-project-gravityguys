using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.4f;

    private Vector3 originalPos;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void TriggerShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = originalPos + randomOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}