// - ElectricTorchOnOff - Script by Marcelli Michele
// Modificado: control con mouse + sonido on/off (por Mentor de Unity y C#)

using UnityEngine;

public class ElectricTorchOnOff : MonoBehaviour
{
    EmissionMaterialGlassTorchFadeOut _emissionMaterialFade;
    BatteryPowerPickup _batteryPower;

    // --- NUEVO: Rotación por mouse ---
    [Header("Mouse Look (Opcional)")]
    [Tooltip("Si se asigna, este transform rotará con el mouse. Si se deja vacío, rota este mismo objeto.")]
    public Transform mousePivot;
    [Tooltip("Sensibilidad de movimiento del mouse.")]
    public float mouseSensitivity = 120f;
    [Tooltip("Límite vertical de la linterna en grados.")]
    public float pitchClamp = 80f;
    [Tooltip("Bloquear y ocultar el cursor al iniciar?")]
    public bool lockCursor = false;

    float _yaw;
    float _pitch;

    // --- NUEVO: Sonidos on/off ---
    [Header("Audio")]
    public AudioClip onClip;
    public AudioClip offClip;
    [Range(0f, 1f)] public float audioVolume = 0.9f;
    AudioSource _audio;

    // --- Original ---
    public enum LightChoose { noBattery, withBattery }
    public LightChoose modoLightChoose;

    [Space]
    public string onOffLightKey = "F";
    private KeyCode _kCode;

    [Space]
    public bool _PowerPickUp = false;

    [Space]
    public float intensityLight = 2.5F;
    private bool _flashLightOn = false;

    [SerializeField] float _lightTime = 0.05f;

    // Cache del Light para evitar GetComponent repetidos
    Light _light;

    private void Awake()
    {
        _batteryPower = FindObjectOfType<BatteryPowerPickup>();
        _light = GetComponent<Light>();
        _audio = GetComponent<AudioSource>();

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Start()
    {
        GameObject _scriptControllerEmissionFade = GameObject.Find("default");

        if (_scriptControllerEmissionFade != null)
        {
            _emissionMaterialFade = _scriptControllerEmissionFade.GetComponent<EmissionMaterialGlassTorchFadeOut>();
        }
        if (_scriptControllerEmissionFade == null) { Debug.Log("Cannot find 'EmissionMaterialGlassTorchFadeOut' script"); }

        _kCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), onOffLightKey);

        // Si no hay pivot, usamos este mismo transform
        if (mousePivot == null) mousePivot = transform;

        // Inicializamos yaw/pitch con la rotación actual
        Vector3 e = mousePivot.rotation.eulerAngles;
        _yaw = e.y;
        _pitch = e.x;
    }

    void Update()
    {
        // detecting parse error keyboard type
        if (System.Enum.TryParse(onOffLightKey, out _kCode))
        {
            _kCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), onOffLightKey);
        }

        // --- NUEVO: mover linterna con el mouse ---
        MouseLook();

        switch (modoLightChoose)
        {
            case LightChoose.noBattery:
                NoBatteryLight();
                break;
            case LightChoose.withBattery:
                WithBatteryLight();
                break;
        }
    }

    void InputKey()
    {
        if (Input.GetKeyDown(_kCode) && _flashLightOn == true)
        {
            _flashLightOn = false;
            PlayToggle(false); // NUEVO: sonido apagado
        }
        else if (Input.GetKeyDown(_kCode) && _flashLightOn == false)
        {
            _flashLightOn = true;
            PlayToggle(true); // NUEVO: sonido encendido
        }
    }

    // --- NUEVO: control de mouse tipo FPS sencillo ---
    void MouseLook()
    {
        // Usa los ejes clásicos de Input Manager (Mouse X/Y)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _yaw += mouseX * mouseSensitivity * Time.deltaTime;
        _pitch -= mouseY * mouseSensitivity * Time.deltaTime;
        _pitch = Mathf.Clamp(_pitch, -pitchClamp, pitchClamp);

        mousePivot.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
    }

    // --- NUEVO: reproducir audio al alternar ---
    void PlayToggle(bool turnOn)
    {
        if (_audio == null) return;
        AudioClip clip = turnOn ? onClip : offClip;
        if (clip != null)
        {
            _audio.PlayOneShot(clip, audioVolume);
        }
    }

    void NoBatteryLight()
    {
        if (_flashLightOn)
        {
            _light.intensity = intensityLight;
            if (_emissionMaterialFade != null) _emissionMaterialFade.OnEmission();
        }
        else
        {
            _light.intensity = 0.0f;
            if (_emissionMaterialFade != null) _emissionMaterialFade.OffEmission();
        }
        InputKey();
    }

    void WithBatteryLight()
    {
        if (_flashLightOn)
        {
            _light.intensity = intensityLight;
            intensityLight -= Time.deltaTime * _lightTime;
            if (_emissionMaterialFade != null) _emissionMaterialFade.TimeEmission(_lightTime);

            if (intensityLight < 0) { intensityLight = 0; }
            if (_PowerPickUp == true && _batteryPower != null)
            {
                intensityLight = _batteryPower.PowerIntensityLight;
            }
        }
        else
        {
            _light.intensity = 0.0f;
            if (_emissionMaterialFade != null) _emissionMaterialFade.OffEmission();

            if (_PowerPickUp == true && _batteryPower != null)
            {
                intensityLight = _batteryPower.PowerIntensityLight;
            }
        }

        InputKey();
    }
}
