using UnityEngine;

public class ElectricTorchOnOff : MonoBehaviour
{
    EmissionMaterialGlassTorchFadeOut _emissionMaterialFade;
    BatteryPowerPickup _batteryPower;
    //

    public enum LightChoose
    {
        noBattery,
        withBattery
    }

    public LightChoose modoLightChoose;
    [Space]
    [Space]
    public string onOffLightKey = "F";
    private KeyCode _kCode;
    [Space]
    [Space]
    public bool _PowerPickUp = false;
    [Space]
    public float intensityLight = 2.5F;
    private bool _flashLightOn = false;
    [SerializeField] float _lightTime = 0.05f;

    private Light _light; // Referencia al componente Light

    private void Awake()
    {
        _batteryPower = FindObjectOfType<BatteryPowerPickup>();
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

        _light = GetComponent<Light>(); // Obtener el componente Light
    }

    void Update()
    {
        // Detectar error de análisis de la tecla
        if (System.Enum.TryParse(onOffLightKey, out _kCode))
        {
            _kCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), onOffLightKey);
        }
        //

        switch (modoLightChoose)
        {
            case LightChoose.noBattery:
                NoBatteryLight();
                break;
            case LightChoose.withBattery:
                WithBatteryLight();
                break;
        }

        FollowMouse(); // Llamar al método para seguir el mouse
    }

    void InputKey()
    {
        if (Input.GetKeyDown(_kCode) && _flashLightOn == true)
        {
            _flashLightOn = false;

        }
        else if (Input.GetKeyDown(_kCode) && _flashLightOn == false)
        {
            _flashLightOn = true;

        }
    }

    void NoBatteryLight()
    {
        if (_flashLightOn)
        {
            _light.intensity = intensityLight;
            _emissionMaterialFade.OnEmission();
        }
        else
        {
            _light.intensity = 0.0f;
            _emissionMaterialFade.OffEmission();
        }
        InputKey();
    }

    void WithBatteryLight()
    {

        if (_flashLightOn)
        {
            _light.intensity = intensityLight;
            intensityLight -= Time.deltaTime * _lightTime;
            _emissionMaterialFade.TimeEmission(_lightTime);

            if (intensityLight < 0)
            {
                intensityLight = 0;
            }
            if (_PowerPickUp == true)
            {
                intensityLight = _batteryPower.PowerIntensityLight;
            }
        }
        else
        {
            _light.intensity = 0.0f;
            _emissionMaterialFade.OffEmission();

            if (_PowerPickUp == true)
            {
                intensityLight = _batteryPower.PowerIntensityLight;
            }
        }

        InputKey();
    }

    void FollowMouse()
    {
        if (_flashLightOn) // Solo seguir el mouse si la linterna está encendida
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Crear un rayo desde la posición del mouse
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 direction = hit.point - transform.position; // Calcular la dirección hacia el punto de impacto
                transform.forward = direction.normalized; // Ajustar la dirección del objeto
            }
        }
    }
}