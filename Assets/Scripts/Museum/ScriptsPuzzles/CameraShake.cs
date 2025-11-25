using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPos;
    private float shakeDuration;
    private float shakeMagnitude;

    private void Awake()
    {
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;
            randomPoint.z = originalPos.z; // por si es cámara 2D/3D fija

            transform.localPosition = randomPoint;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
