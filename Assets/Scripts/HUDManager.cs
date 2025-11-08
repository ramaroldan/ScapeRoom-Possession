using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Timer Settings")]
    public TextMeshProUGUI timerText;
    public GameObject hudPanel;
    public int startingMinutes = 30;

    private float timeRemaining;
    private bool timerRunning = false;

    private void Awake()
    {
        // Singleton para evitar duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Inicializa el tiempo
        timeRemaining = startingMinutes * 60f;
    }

    private void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                timerRunning = false;
                timeRemaining = 0;
                UpdateTimerUI();
                HideHUD();
                SceneManager.LoadScene("Lost");
            }
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        if (sceneName == "Main Menu")
        {
            HideHUD();
        }
        else if (sceneName == "Lobby")
        {
            ShowHUD();
            StartTimer(); // Reinicia si querés que empiece de nuevo al entrar
        }
        else
        {
            ShowHUD();
        }
    }

    public void ShowHUD()
    {
        if (hudPanel != null)
            hudPanel.SetActive(true);
    }

    public void HideHUD()
    {
        if (hudPanel != null)
            hudPanel.SetActive(false);
    }

    public void StartTimer()
    {
        timeRemaining = startingMinutes * 60f;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }
}
