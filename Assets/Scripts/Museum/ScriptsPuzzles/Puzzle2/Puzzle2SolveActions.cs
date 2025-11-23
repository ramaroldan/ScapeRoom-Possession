using UnityEngine;

public class Puzzle2SolveActions : MonoBehaviour
{
    [Header("Objetos que aparecerán al resolver")]
    public GameObject[] objectsToActivate;

    [Header("Objetos que desaparecerán al resolver")]
    public GameObject[] objectsToDeactivate;

    [Header("Audio")]
    [Tooltip("Sonido de la puerta abriéndose")]
    public AudioSource doorAudioSource;   // debe tener asignado el clip del sonido de puerta

    public void OnPuzzle2Solved()
    {
        Debug.Log("Puzzle2SolveActions: Ejecutando acciones de puzzle resuelto");

        // Activar objetos
        if (objectsToActivate != null)
        {
            foreach (var obj in objectsToActivate)
            {
                if (obj != null)
                    obj.SetActive(true);
            }
        }

        // Desactivar objetos
        if (objectsToDeactivate != null)
        {
            foreach (var obj in objectsToDeactivate)
            {
                if (obj != null)
                    obj.SetActive(false);
            }
        }

        // 🔊 Reproducir sonido de puerta abriéndose
        if (doorAudioSource != null)
            doorAudioSource.Play();

        Debug.Log("Puzzle2: sonido de puerta reproducido.");
    }
}
