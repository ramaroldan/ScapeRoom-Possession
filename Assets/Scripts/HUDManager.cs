using PixeLadder.EasyTransition;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Timer Settings")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] int startingMinutes = 60;
    private float timeRemaining;
    private bool timerRunning = false;
    private bool gameEnded = false;
    AsyncOperation asyncLoad;
    public float TimeRemaining => timeRemaining;

    [Header("Panels")]
    [SerializeField] GameObject hudPanel;
    [SerializeField] private GameObject panelPausa;
    [SerializeField] private GameObject panelVictoria;
    [SerializeField] private GameObject panelDerrota;
    [SerializeField] private GameObject panelInventory;

    [Header("Panel inventory Scenes")]
    [SerializeField] private GameObject SeceneHospital;
    [SerializeField] private GameObject SceneMorgue;
    [SerializeField] private GameObject SceneMuseum;

    [SerializeField] private MouseLook mouseLookPlayer;
    [SerializeField] private MouseLook mouseLookCamera;

    [Header("Panel loading")]
    public Image image_Progress;
    public GameObject Panel_Loading;
    public Text text_Progress;
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerMovement playerScript;
    [SerializeField] private Interact interactScript;


    private bool isPaused = false;
    private bool isInventoryOpen = false; // Estado del inventario
    float progress = 0f;
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
    private void Start()
    {
        Time.timeScale = 1;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            player.SetActive(true);
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
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            HintGameManager.Instance.ShowHint();
        }

        // Mostrar/ocultar el panel de inventario con la tecla "I"
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
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

        if (sceneName == "MainMenu" || sceneName=="Lobby")
        {
            HideHUD();
            if (sceneName == "MainMenu") panelPausa.SetActive(false);
        }
        else if (sceneName == "HospitalRoom")
        {
            ShowHUD();
            StartTimer();
            SeceneHospital.SetActive(true);
            TMP_Text hospitalInfoText = SeceneHospital.GetComponentInChildren<TMP_Text>();
            hospitalInfoText.text = LocalizationManager.Instance.GetText("hospital_info_text");
            hospitalInfoText.gameObject.SetActive(true);


        }
        if (sceneName == "Morgue")
        {           
            SceneMorgue.SetActive(true);            
        }
        if (sceneName == "Museum")
        {                    
            SceneMuseum.SetActive(true);
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
        //hudPanel.SetActive(!isPaused);

        // Pausar o reanudar el tiempo
        Time.timeScale = isPaused ? 0f : 1f;

        SetPlayerControl(isPaused);

    }
    public void Pause()
    {
        if (gameEnded) return;

        isPaused = !isPaused;

        // Mostrar/ocultar paneles
        panelPausa.SetActive(isPaused);
       // hudPanel.SetActive(!isPaused);

        // Pausar o reanudar el tiempo
        Time.timeScale = isPaused ? 0f : 1f;

        SetPlayerControl(isPaused);

    }
    public void SetPlayerControl(bool isUIActive)
    {      
        if (mouseLookPlayer != null)
        {           
            mouseLookPlayer.overrideCursorLock = isUIActive;
           // Debug.Log("Override cursor lock seteado a: " + mouseLookPlayer.overrideCursorLock);
        }
        if (mouseLookCamera != null)
        {          
            mouseLookCamera.overrideCursorLock = isUIActive;
           // Debug.Log("Override cursor lock seteado a: " + mouseLookCamera.overrideCursorLock);
        }

        Cursor.visible = isUIActive;
        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
    }



    public void ToggleInventory()
    {
        if (panelInventory == null) return;

        isInventoryOpen = !isInventoryOpen;

        // Mostrar/ocultar el panel de inventario
        panelInventory.SetActive(isInventoryOpen);

        // Pausar el juego mientras el inventario está abierto
        // Time.timeScale = isInventoryOpen ? 0f : 1f;
        // bloquear control del player
        mouseLookPlayer.working = !isInventoryOpen;
        mouseLookCamera.working = !isInventoryOpen;
       
        if (playerScript != null)
            playerScript.SetWorking(!isInventoryOpen);

        // bloquear el Interact para que no detecte raycasts mientras UI está abierta
        if (interactScript != null)
            interactScript.enabled = !isInventoryOpen;

        SetPlayerControl(isInventoryOpen);
        // Mostrar/ocultar el cursor
        //Cursor.visible = isInventoryOpen;
        //Cursor.lockState = isInventoryOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void GameOver()
    {
        if (gameEnded) return; // evita doble llamada
        gameEnded = true;
        timerRunning = false;

        Time.timeScale = 1f; // ← importante! que el SceneTransitioner pueda correr

        PlayerPrefs.SetInt("EndGameResult", 0);

        SetPlayerControl(true);
        hudPanel.SetActive(false);

        // NO destruyas el gameObject acá, dejá que la escena lo maneje
        SceneTransitioner.Instance.LoadScene("EndGame", null);
    }

   

    public void SaveVictoryTime()
    {
        PlayerPrefs.SetFloat("VictoryTime", timeRemaining);
    }

    public void ShowVictoryPanel()
    {
        HideHUD(); // Oculta el HUD normal (timer, etc)
        SetPlayerControl(true);

        //if (panelVictoria != null)
        //    panelVictoria.SetActive(true);
        SaveVictoryTime();
        PlayerPrefs.SetInt("EndGameResult", 1); // 1 = victoria
        StopTimer();
        SetPlayerControl(true);
        SceneTransitioner.Instance.LoadScene("EndGame", null);
    }
    public void Click_Exit()
    {
        Application.Quit();
    }

    public void Click_MenuPrincial()
    {
        // hudPanel.SetActive(false);
        // Destroy(Player);
        Player.SetActive(false);
        //
        //Destroy(gameObject);
        Time.timeScale = 1f;
        SceneTransitioner.Instance.LoadScene("MainMenu", null);
      
    }
    IEnumerator StartToLoadTheMenu()
    {
        panelPausa.SetActive(false);
        Panel_Loading.SetActive(true);
       // yield return new WaitForSeconds(1);
        asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        asyncLoad.allowSceneActivation = false;
        while (progress <= 1f)
        {
            image_Progress.fillAmount = progress;
            text_Progress.text = "%" + Mathf.Round(progress * 100f);
            progress += .01f;
            yield return new WaitForSeconds(.01f);
        }
        asyncLoad.allowSceneActivation = true;
        // ButtonStart.SetActive(true);
        //text_Progress.transform.parent.gameObject.SetActive(false);
    }
}