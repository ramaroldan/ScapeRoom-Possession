using UnityEngine;

public class JumpscareController : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject scareObject;      // por ej. un personaje que aparece de golpe
    public AudioSource scareSFX;        // grito / golpe / sonido
    public CameraShake cameraShake;     // tu script de sacudida de cámara (si lo tenés)
    public Light flickerLight;          // luz que parpadea (opcional)

    public float flickerDuration = 0.5f;

    private bool alreadyTriggered = false;

    public void PlayJumpscare()
    {
        if (alreadyTriggered) return;
        alreadyTriggered = true;

        if (scareObject != null)
            scareObject.SetActive(true);

        if (scareSFX != null)
            scareSFX.Play();

        if (cameraShake != null)
            cameraShake.Shake(0.4f, 0.5f); // duración e intensidad, ajustá a gusto

        if (flickerLight != null)
            StartCoroutine(FlickerCoroutine());
    }

    private System.Collections.IEnumerator FlickerCoroutine()
    {
        float t = 0f;
        bool on = true;

        while (t < flickerDuration)
        {
            on = !on;
            flickerLight.enabled = on;
            t += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        flickerLight.enabled = true;
    }
}
