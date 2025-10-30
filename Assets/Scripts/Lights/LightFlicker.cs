using UnityEngine;

/// Parpadeo fr�o y sucio para luces (Point/Spot).
/// Requiere que la luz sea Realtime o Mixed (no funciona en Baked pura).
[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Intensidad base y variaci�n")]
    [Tooltip("Intensidad media alrededor de la que vibra la luz.")]
    public float baseIntensity = 0.8f;
    [Tooltip("Cu�nto puede subir/bajar desde la base.")]
    public float flickerAmount = 0.6f;

    [Header("Ruido")]
    [Tooltip("Velocidad del parpadeo (Perlin).")]
    public float noiseSpeed = 18f;
    [Tooltip("Semilla para que cada luz sea distinta.")]
    public float noiseSeed = 0f;

    [Header("Apagones cortos (opcional)")]
    public bool randomBlackouts = true;
    public Vector2 blackoutInterval = new Vector2(6f, 14f); // cada cu�nto
    public Vector2 blackoutDuration = new Vector2(0.08f, 0.35f); // cu�nto duran

    [Header("Jitter extra (opcional)")]
    public bool jitterRange = false;
    public float rangeAmount = 0.3f;

    Light _light;
    float _t;
    float _blackoutEnd;
    float _nextBlackout;

    void Awake()
    {
        _light = GetComponent<Light>();
        if (_light.lightmapBakeType == LightmapBakeType.Baked)
            Debug.LogWarning($"{name}: esta luz es Baked: el parpadeo no ser� visible en runtime. Usa Mixed/Realtime o el script de Emission.", this);

        if (noiseSeed == 0f) noiseSeed = Random.value * 1000f;
        ScheduleNextBlackout();
    }

    void Update()
    {
        _t += Time.deltaTime * noiseSpeed;

        // Ruido Perlin en [-1,1]
        float n = (Mathf.PerlinNoise(_t, noiseSeed) * 2f) - 1f;
        float intensity = baseIntensity + n * flickerAmount;

        // Apag�n breve
        if (randomBlackouts)
        {
            if (Time.time >= _nextBlackout) StartBlackout();
            if (Time.time < _blackoutEnd) intensity *= 0.05f;
        }

        _light.intensity = Mathf.Max(0f, intensity);

        if (jitterRange && _light.type != LightType.Rectangle)
            _light.range = Mathf.Max(0.1f, _light.range + n * rangeAmount * Time.deltaTime);
    }

    void StartBlackout()
    {
        _blackoutEnd = Time.time + Random.Range(blackoutDuration.x, blackoutDuration.y);
        ScheduleNextBlackout();
    }

    void ScheduleNextBlackout()
    {
        _nextBlackout = Time.time + Random.Range(blackoutInterval.x, blackoutInterval.y);
    }
}
