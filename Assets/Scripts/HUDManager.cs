using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Michsky.UI.Dark;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Timer Settings")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] int startingMinutes = 30;
    private float timeRemaining;
    private bool timerRunning = false;
    private bool gameEnded = false;
    public float TimeRemaining => timeRemaining;
    [Header("Panels")]
    [SerializeField] GameObject hudPanel;
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota;
  


    [SerializeField] private MonoBehaviour cameraControlScript;
    [SerializeField] private ModalWindowManager exitModal;

    private bool isPaused = false;
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
        if (gameEnded) return;
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
                GameOver();
                //HideHUD();
                //SceneManager.LoadScene("Lost");
            }
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
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

    public void TogglePause()
    {
        if (gameEnded) return;

        isPaused = !isPaused;

        // Mostrar/ocultar paneles
        panelPausa.SetActive(isPaused);
        hudPanel.SetActive(!isPaused);

        // Pausar o reanudar el tiempo
        Time.timeScale = isPaused ? 0f : 1f;

        cameraControlScript.enabled = !isPaused;

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }
    private void GameOver()
    {
        gameEnded = true;
        isPaused = !isPaused;
        hudPanel.SetActive(!isPaused);
        if (panelDerrota != null)
            panelDerrota.SetActive(true);

        Time.timeScale = 0f; // Pausamos todo

        cameraControlScript.enabled = !isPaused;

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;

      
    }
    public void ShowExitConfirmation()
    {
        if (exitModal != null)
        {
            exitModal.gameObject.SetActive(true); // Asegura que esté visible
            exitModal.ModalWindowInTest();            // Llama la animación
        }
    }
    public void SaveVictoryTime()
    {
        PlayerPrefs.SetFloat("VictoryTime", timeRemaining);
    }

    public void ShowVictoryPanel()
    {
        HideHUD(); // Oculta el HUD normal (timer, etc)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

}
