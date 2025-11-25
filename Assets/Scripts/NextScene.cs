using PixeLadder.EasyTransition;
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
            // SceneManager.LoadScene(nextSceneName);

            //if (transitionEffects.Length == 0) return;

            // The "next scene" is just this same scene, which we will reload.
            string sceneToLoad = SceneManager.GetActiveScene().name;
           // TransitionEffect effectToUse = transitionEffects[currentSceneIndex];

            // Call the SceneTransitioner to start the transition.
            SceneTransitioner.Instance.LoadScene(nextSceneName, null);
        }
    }

    public void SiguienteEscena()
    {
        
        SceneManager.LoadScene(nextSceneName);
        
    }

    
}
