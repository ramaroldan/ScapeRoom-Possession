using UnityEngine;

public class OpenDoors:MonoBehaviour
{
    [SerializeField] private Animator[] doors;
    [SerializeField] private Animator[] camillas;
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private GameObject[] mistPrefabs;
    [SerializeField] private Light[] lightsToActivate;
    private bool hasTriggered = false;
    [SerializeField] private Renderer[] emissiveObjects;
    [SerializeField] private Color emissionColor = Color.magenta;


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            foreach (Animator door in doors)
            {
                if (door != null)
                    door.SetTrigger("Open");
            }
            foreach (Animator camilla in camillas)
            {
                if (camilla != null)
                {
                    camilla.SetTrigger("Open");
                    AudioSource camillaAudio = camilla.GetComponent<AudioSource>();
                    if (camillaAudio != null)
                        camillaAudio.Play();
                }
                   
            }
            foreach (GameObject mist in mistPrefabs)
            {
                if (mist != null)
                    mist.SetActive(true);
            }
            foreach (Light luz in lightsToActivate)
            {
                if (luz != null)
                    luz.gameObject.SetActive(true);
            }
            foreach (Renderer rend in emissiveObjects)
            {
                if (rend != null)
                {
                    Material mat = rend.material;

                    // Activar Emission (clave en URP)
                    mat.EnableKeyword("_EMISSION"); // en HDRP no hace falta, pero en URP sí
                    mat.SetColor("_EmissionColor", emissionColor);
                }
            }


        }
    }

}
