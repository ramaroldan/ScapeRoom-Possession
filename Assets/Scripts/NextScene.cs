using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene:MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Nombre exacto de la escena a cargar (debe estar en Build Settings)")]
    public string nextSceneName;

    [Tooltip("Etiqueta del jugador que activa la puerta")]
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Cambiar a la siguiente escena
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
