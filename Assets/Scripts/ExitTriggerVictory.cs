using PixeLadder.EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTriggerVictory : MonoBehaviour
{
    [SerializeField] private string victorySceneName = "Lobby"; 
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player")) 
        {
            triggered = true;

            if (HUDManager.Instance != null && HUDManager.Instance.TimeRemaining > 0f)
            {
                // Guardar tiempo y pasar de escena
                //HUDManager.Instance.SaveVictoryTime();                
                //PlayerPrefs.SetInt("EndGameResult", 1); // 1 = victoria
                //HUDManager.Instance.StopTimer();                        
                //SceneTransitioner.Instance.LoadScene("EndGame", null);
                HUDManager.Instance.ShowVictoryPanel();
            }
            else
            {
                
            }
        }
    }
}