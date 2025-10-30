using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class FlashlightController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform flashlight;
    [SerializeField] private Light flashlightLight;
    [SerializeField] private Camera cam;

    [Header("Toggle")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    [SerializeField] private bool isOnAtStart = false;

    [Header("Aim")]
    [SerializeField] private LayerMask aimMask = ~0;
    [SerializeField] private float maxAimDistance = 40f;
    [Tooltip("Suavizado exponencial de rotación (más alto = sigue más rápido).")]
    [SerializeField, Range(4f, 40f)] private float rotateSpeed = 14f;
    [Tooltip("Distancia mínima del punto de mira para evitar pegarnos a paredes.")]
    [SerializeField, Range(0.1f, 2f)] private float minTargetDistance = 0.9f;

    [Header("Light Tuning (horror/URP-BuiltIn)")]
    [SerializeField] private float range = 22f;
    [SerializeField, Tooltip("URP/Built-in: ≈ 3–4. HDRP físico: usar lux altos.")]
    private float baseIntensity = 3.5f;
    [SerializeField, Range(5f, 60f)] private float spotAngle = 30f;
    [SerializeField, Range(0f, 59f)] private float innerSpotAngle = 20f;
    [SerializeField] private Color color = new Color(1f, 0.95f, 0.85f);
    [SerializeField] private bool castShadows = true;

    [Header("Flicker (suave)")]
    [SerializeField] private bool flickerEnabled = true;
    [SerializeField] private float noiseFrequency = 9f;
    [SerializeField, Range(0f, 0.6f)] private float intensityAmplitude = 0.08f;
    [SerializeField, Range(0f, 6f)] private float angleJitter = 0.5f;
    [SerializeField, Range(0f, 1f)] private float dropoutChancePerSec = 0.03f;
    [SerializeField] private Vector2 dropoutDurationRange = new Vector2(0.05f, 0.12f);
    [SerializeField, Range(0f, 1f)] private float aimJitter = 0.0f; // 0 para quitar “tembleque”

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sOn;
    [SerializeField] private AudioClip sOff;
    [SerializeField] private AudioClip sHumLoop;
    [SerializeField, Range(0f, 1f)] private float onOffVolume = 0.9f;
    [SerializeField, Range(0.5f, 1.5f)] private float onOffPitchVar = 0.07f;
    [SerializeField, Range(0f, 1f)] private float humVolume = 0.22f;

    [SerializeField, Tooltip("Separa el objetivo de la superficie para evitar giros bruscos.")]
    private float surfaceOffset = 0.08f;


    // Internos
    float _tNoise;
    float _intensity0;
    float _angle0;
    bool _inDropout;
    Quaternion _targetRot;

    void Reset() { TryAutoWire(); }

    void Awake()
    {
        TryAutoWire();

        // Preset recomendado para horror (URP/Built-in). Ajusta si usas HDRP físico.
        range = Mathf.Clamp(range, 18f, 26f);
        baseIntensity = Mathf.Clamp(baseIntensity, 2.5f, 5f);
        spotAngle = Mathf.Clamp(spotAngle, 26f, 34f);
        innerSpotAngle = Mathf.Clamp(innerSpotAngle, 16f, spotAngle - 2f);

        _intensity0 = baseIntensity;
        _angle0 = spotAngle;
        ApplyLightSettings();
        SetActive(isOnAtStart, playSound: false);
        _tNoise = Random.Range(0f, 1000f);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            SetActive(!IsActive(), playSound: true);

        if (!IsActive()) return;

        AimAtMouse();
        if (flickerEnabled) DoFlicker();

        if (audioSource && sHumLoop && audioSource.isPlaying)
            audioSource.volume = humVolume * Mathf.InverseLerp(0f, _intensity0, flashlightLight.intensity);
    }

    // ---------- Toggle & refs ----------
    private bool IsActive()
    {
        return flashlight != null && flashlight.gameObject.activeSelf &&
               (flashlightLight == null || flashlightLight.enabled);
    }

    private void SetActive(bool on, bool playSound = true)
    {
        if (flashlight != null) flashlight.gameObject.SetActive(on);
        if (flashlightLight != null) flashlightLight.enabled = on;

        if (!audioSource) return;

        if (on)
        {
            if (playSound && sOn) PlayOneShotVar(sOn, onOffVolume, onOffPitchVar);
            if (sHumLoop)
            {
                if (audioSource.clip != sHumLoop) audioSource.clip = sHumLoop;
                audioSource.loop = true;
                audioSource.volume = humVolume;
                if (!audioSource.isPlaying) audioSource.Play();
            }
        }
        else
        {
            if (playSound && sOff) PlayOneShotVar(sOff, onOffVolume, onOffPitchVar);
            if (audioSource.isPlaying && audioSource.clip == sHumLoop) audioSource.Stop();
        }
    }

    private void PlayOneShotVar(AudioClip clip, float vol, float pitchVar)
    {
        if (!clip) return;
        float prevPitch = audioSource.pitch;
        audioSource.pitch = 1f + Random.Range(-pitchVar, pitchVar);
        audioSource.PlayOneShot(clip, vol);
        audioSource.pitch = prevPitch;
    }

    private void TryAutoWire()
    {
        if (flashlight == null)
        {
            var childLight = GetComponentInChildren<Light>(true);
            if (childLight != null) { flashlightLight = childLight; flashlight = childLight.transform; }
        }
        else if (flashlightLight == null) flashlightLight = flashlight.GetComponentInChildren<Light>(true);

        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (cam == null) cam = Camera.main;
    }

    // ---------- Aim ----------
    private void AimAtMouse()
    {
        if (cam == null || flashlight == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out var hit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
        {
            // Empuja el objetivo fuera de la superficie para evitar que el forward se quede “sin longitud”
            targetPoint = hit.point + hit.normal * surfaceOffset;

            // Si ese objetivo queda “detrás” del forward de la cámara, forzamos un fallback estable
            Vector3 toTarget = targetPoint - flashlight.position;
            if (Vector3.Dot(toTarget, cam.transform.forward) < 0f || toTarget.sqrMagnitude < 0.0004f)
            {
                targetPoint = cam.transform.position + cam.transform.forward * Mathf.Max(minTargetDistance, 0.5f);
            }
        }
        else
        {
            targetPoint = ray.GetPoint(maxAimDistance);
        }

        Vector3 dir = (targetPoint - flashlight.position);
        if (dir.sqrMagnitude < 0.0001f) return;

        // Sin jitter para evitar “tembleque”
        _targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);

        // Suavizado exponencial (estable con FPS variables)
        float a = 1f - Mathf.Exp(-rotateSpeed * Time.deltaTime);
        flashlight.rotation = Quaternion.Slerp(flashlight.rotation, _targetRot, a);
    }


    // ---------- Light setup ----------
    private void ApplyLightSettings()
    {
        if (!flashlightLight) return;

        flashlightLight.type = LightType.Spot;
        flashlightLight.range = range;
        flashlightLight.intensity = baseIntensity; // URP/Built-in “unitless”
        flashlightLight.spotAngle = spotAngle;
#if UNITY_2020_1_OR_NEWER
        flashlightLight.innerSpotAngle = innerSpotAngle;
#endif
        flashlightLight.color = color;
        flashlightLight.shadows = castShadows ? LightShadows.Soft : LightShadows.None;

        // Bias bajos para que ilumine bien en cercanías (evita gaps y “peter panning”).
        flashlightLight.shadowBias = 0.02f;
        flashlightLight.shadowNormalBias = 0.1f;

        flashlightLight.renderMode = LightRenderMode.ForcePixel;
        flashlightLight.cookieSize = 7f; // si usas cookie de haz, esto define el tamaño
    }

    // ---------- Flicker ----------
    private void DoFlicker()
    {
        if (!flashlightLight) return;

        if (!_inDropout && dropoutChancePerSec > 0f)
        {
            float p = dropoutChancePerSec * Time.deltaTime;
            if (Random.value < p) StartCoroutine(Dropout());
        }

        _tNoise += Time.deltaTime * noiseFrequency;
        float n = Mathf.PerlinNoise(_tNoise, 0.0f) * 2f - 1f;
        float amp = intensityAmplitude * (_inDropout ? 0.4f : 1f);

        float targetIntensity = _intensity0 * (1f + n * amp);
        flashlightLight.intensity = Mathf.Max(0f, targetIntensity);

        if (angleJitter > 0f)
        {
            float jitter = (Mathf.PerlinNoise(0.0f, _tNoise * 0.8f) * 2f - 1f) * angleJitter;
            flashlightLight.spotAngle = Mathf.Clamp(_angle0 + jitter, 5f, 60f);
        }
    }

    private IEnumerator Dropout()
    {
        _inDropout = true;
        float dur = Random.Range(dropoutDurationRange.x, dropoutDurationRange.y);
        float timer = 0f;
        float start = flashlightLight.intensity;

        while (timer < dur)
        {
            timer += Time.deltaTime;
            float t = timer / dur;
            float k = Mathf.SmoothStep(0.0f, 0.15f, 1f - Mathf.Pow(1f - t, 2f));
            flashlightLight.intensity = Mathf.Lerp(start, _intensity0 * 0.12f, k);
            yield return null;
        }

        _inDropout = false;
    }

    // Utilidad rápida para aplicar preset desde el menú contextual del componente
    [ContextMenu("Aplicar preset horror (URP/Built-in)")]
    private void ApplyHorrorPreset()
    {
        range = 22f;
        baseIntensity = 3.5f;
        spotAngle = 30f;
        innerSpotAngle = 19f;
        color = new Color(1f, 0.95f, 0.85f);
        intensityAmplitude = 0.08f;
        angleJitter = 0.5f;
        dropoutChancePerSec = 0.03f;
        aimJitter = 0f;
        ApplyLightSettings();
    }
}
