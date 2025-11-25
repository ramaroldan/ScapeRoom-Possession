using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[ExecuteInEditMode]
public class RevealURP : MonoBehaviour
{
    [SerializeField] Material Mat;
    //[SerializeField] Light SpotLight;
    private Light SpotLight;

    void Start()
    {
        TryFindFlashlight();
    }
    private void TryFindFlashlight()
    {
        // 1) BUSCAR POR NOMBRE (más directo)
        var obj = GameObject.Find("Torch Light");
        if (obj != null)
        {
            SpotLight = obj.GetComponent<Light>();
            if (SpotLight != null) return;
        }

        // 2) BUSCAR POR TAG (si querés ponerle tag tipo "Flashlight")
        var tagged = GameObject.FindGameObjectWithTag("Flashlight");
        if (tagged != null)
        {
            SpotLight = tagged.GetComponent<Light>();
            if (SpotLight != null) return;
        }

        
    }
    void Update()
    {
        if (Mat != null && SpotLight != null)
        {

            // URP shader uses "_LightPosition", "_LightDirection", and "_LightAngle"
            Mat.SetVector("_LightPosition", SpotLight.transform.position);
            Mat.SetVector("_LightDirection", -SpotLight.transform.forward);
            Mat.SetFloat("_LightAngle", SpotLight.spotAngle);
            // ⭐ CLAVE: usar intensidad de la linterna
            Mat.SetFloat("_Intensity", SpotLight.enabled ? SpotLight.intensity : 0f);
        }
    }
}