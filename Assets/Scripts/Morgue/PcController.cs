using UnityEngine;
using UnityEngine.UI;
using TMPro; // si usás TMP. Si no, usá UnityEngine.UI con InputField

public class PcController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject panelLogin;     // PanelPc
    public GameObject panelDesk;      // PanelDesk

    [Header("Login UI")]
    public TMP_InputField passwordInput;
    public Button loginButton;

    [Header("Desk UI")]
    public Button exitButton;
    public Button playButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    [Header("Password Settings")]
    public string correctPassword = "1234";

    private void Start()
    {
        // Asegurar estados iniciales
        panelLogin.SetActive(true);
        panelDesk.SetActive(false);

        // Asignar listeners
        loginButton.onClick.AddListener(ValidatePassword);
        exitButton.onClick.AddListener(ExitDesk);
        playButton.onClick.AddListener(PlayAudio);
    }

    // --------------------------------------------------------------------
    void ValidatePassword()
    {
        if (passwordInput.text == correctPassword)
        {
            Debug.Log("✔ Password correcta");
            panelLogin.SetActive(false);
            panelDesk.SetActive(true);
        }
        else
        {
            Debug.Log("❌ Password incorrecta");
        }
    }

    // --------------------------------------------------------------------
    public void ExitDesk()
    {
        // Volver al login pero sin cerrar el canvas completo
        panelDesk.SetActive(false);
        panelLogin.SetActive(true);
    }

    // --------------------------------------------------------------------
    void PlayAudio()
    {
        if (audioSource != null && audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
