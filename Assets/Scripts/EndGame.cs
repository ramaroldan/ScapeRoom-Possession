using PixeLadder.EasyTransition;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour 
{

    [Header("Panels")]
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota;

    [Header("Victoria - Tiempo")]
    [SerializeField] private TextMeshProUGUI textTiempoVictoria;

    private void Start()
    {
        Time.timeScale = 1f; // Por si quedó pausado

        // Destruimos el HUD del juego anterior
        if (HUDManager.Instance != null)
            Destroy(HUDManager.Instance.gameObject);

        // Destruimos el Player si sobrevivió
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            Destroy(player);

        // Leemos qué panel mostrar
        bool isVictory = PlayerPrefs.GetInt("EndGameResult", 0) == 1;

        panelVictoria.SetActive(isVictory);
        panelDerrota.SetActive(!isVictory);

        if (isVictory)
            MostrarTiempoVictoria();
    }
    private void MostrarTiempoVictoria()
    {
        if (textTiempoVictoria == null) return;

        float victoryTime = PlayerPrefs.GetFloat("VictoryTime", 0f);
        int minutes = Mathf.FloorToInt(victoryTime / 60f);
        int seconds = Mathf.FloorToInt(victoryTime % 60f);

        textTiempoVictoria.text = $"{minutes:00}:{seconds:00}";
    }
    public void Click_MainMenu()
    {
        // Limpiamos la prefs usada
        PlayerPrefs.DeleteKey("EndGameResult");
        PlayerPrefs.DeleteKey("VictoryTime");
        // Destruimos el HUDManager si sobrevivió
        if (HUDManager.Instance != null)
            Destroy(HUDManager.Instance.gameObject);

        SceneTransitioner.Instance.LoadScene("MainMenu", null);
    }

    public void Click_Exit()
    {
        Application.Quit();
    }
}
