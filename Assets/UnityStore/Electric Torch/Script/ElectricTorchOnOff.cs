using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Light))]
public class ElectricTorchOnOff : MonoBehaviour
{
    // === Seguimiento de la cámara ===
    [Header("Seguimiento")]
    [Tooltip("Normalmente la Main Camera o la cámara del jugador")]
    [SerializeField] private Transform followTarget;
    [Tooltip("Desfase local respecto a la cámara")]
    [SerializeField] private Vector3 localOffset = new Vector3(0f, -0.05f, 0.25f);
    [Tooltip("Rotación adicional (grados)")]
    [SerializeField] private Vector3 localEulerOffset = Vector3.zero;
    [Tooltip("Suavizado de posición/rotación")]
    [SerializeField] private float followLerp = 20f;

    // === Entrada ===
    [Header("Entrada")]
    [SerializeField] private string onOffLightKey = "F"; // acepta nombres de KeyCode ("F","E","Mouse1", etc.)
    private KeyCode _toggleKey = KeyCode.F;

    // === Intensidad y fade ===
    [Header("Luz")]
    [Min(0f)][SerializeField] private float onIntensity = 2.5f;
    [SerializeField] private float fadeSpeed = 15f; // mayor = más rápido

    // === Audio (opcional) ===
    [Header("Audio (opcional)")]
    [SerializeField] private AudioSource audioSource; // si no hay, se crea uno
    [SerializeField] private AudioClip onClip;
    [SerializeField] private AudioClip offClip;
    [Range(0f, 1f)] public float audioVolume = 0.9f;

    // === Estado interno ===
    private Light _light;
    private bool _isOn;
    private float _currentIntensity;
    private LightShadows _originalShadows;

    private void Reset()
    {
        // Se llama al crear/añadir el componente
        _light = GetComponent<Light>();
        _light.type = LightType.Spot;
        _light.intensity = 0f;
        _light.spotAngle = 55f;   // valores razonables
        _light.range = 15f;
        _light.shadows = LightShadows.Soft;

        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f; // 3D
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.maxDistance = 12f;
        }
    }

    private void Awake()
    {
        _light = GetComponent<Light>();
        _originalShadows = _light.shadows;

        // Inicia apagada
        _isOn = false;
        _currentIntensity = 0f;
        _light.intensity = 0f;
        _light.shadows = LightShadows.None;

        TryParseKey(onOffLightKey);

        // Si no asignaste la cámara, intenta auto-detectar la principal
        if (!followTarget && Camera.main) followTarget = Camera.main.transform;

        // Asegurar AudioSource si el usuario no lo puso
        if (!audioSource)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (!audioSource)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.spatialBlend = 1f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.maxDistance = 12f;
            }
        }
    }

    private void OnValidate()
    {
        // Mantener claves y límites correctos incluso en edición
        if (!_light) _light = GetComponent<Light>();
        onIntensity = Mathf.Max(0f, onIntensity);
        TryParseKey(onOffLightKey);
    }

    private void Update()
    {
        // 1) Seguir cámara/mouse
        FollowCamera();

        // 2) Toggle
        if (Input.GetKeyDown(_toggleKey))
        {
            _isOn = !_isOn;
            PlayToggleSfx(_isOn);
            _light.shadows = _isOn ? _originalShadows : LightShadows.None;
        }

        // 3) Fade de intensidad
        float target = _isOn ? onIntensity : 0f;
        _currentIntensity = Mathf.Lerp(_currentIntensity, target, Time.deltaTime * fadeSpeed);
        _light.intensity = _currentIntensity;
    }

    // ===== Utilidades =====

    private void FollowCamera()
    {
        if (!followTarget) return;

        // Posición con offset local de la cámara
        Vector3 targetPos = followTarget.TransformPoint(localOffset);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followLerp);

        // Rotación hacia donde mira la cámara, con offset
        Quaternion targetRot = followTarget.rotation * Quaternion.Euler(localEulerOffset);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * followLerp);
    }

    private void PlayToggleSfx(bool turningOn)
    {
        if (!audioSource) return;
        AudioClip clip = turningOn ? onClip : offClip;
        if (clip) audioSource.PlayOneShot(clip, audioVolume);
    }

    private void TryParseKey(string key)
    {
        // Permite escribir "F", "E", "Mouse1", etc. Si falla, usa F.
        if (System.Enum.TryParse(key, out KeyCode parsed))
            _toggleKey = parsed;
        else
            _toggleKey = KeyCode.F;
    }
}
