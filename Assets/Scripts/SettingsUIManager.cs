using UnityEngine;

public class SettingsUIManager : MonoBehaviour
{
    [Header("Main Menu (Botones verticales)")]
    [SerializeField] private GameObject categoryButtonList;

    [Header("Sub Panels")]
    [SerializeField] private GameObject panelGameplay;
    [SerializeField] private GameObject panelControls;
    [SerializeField] private GameObject panelAudio;
    [SerializeField] private GameObject panelVisuals;

    [Header("Home Panels")]
    [SerializeField] private GameObject panelHome;
    [SerializeField] private GameObject panelBackground;

    [Header("Botón Back")]
    public GameObject backButton;

    private GameObject currentActivePanel;

    public void OpenPanel(string panelName)
    {
        // Desactiva todos
       // panelBackground.SetActive(false);
        panelHome.SetActive(false);
        panelGameplay.SetActive(false);
        panelControls.SetActive(false);
        panelAudio.SetActive(false);
        panelVisuals.SetActive(false);

        // Oculta las categorías
        categoryButtonList.SetActive(false);

        // Activa el panel correspondiente
        switch (panelName)
        {
            case "PanelGameplay":
                panelGameplay.SetActive(true);
                currentActivePanel = panelGameplay;
                break;
            case "PanelControls":
                panelControls.SetActive(true);
                currentActivePanel = panelControls;
                break;
            case "PanelAudio":
                panelAudio.SetActive(true);
                currentActivePanel = panelAudio;
                break;
            case "PanelVisuals":
                panelVisuals.SetActive(true);
                currentActivePanel = panelVisuals;
                break;
        }

        // Activa el botón de volver (si tenés uno)
        if (backButton != null)
            backButton.SetActive(true);
    }

    public void GoBack()
    {
        panelGameplay.SetActive(false);
        panelControls.SetActive(false);
        panelAudio.SetActive(false);
        panelVisuals.SetActive(false);

        categoryButtonList.SetActive(true);

       
    }

    public void backHome()
    {
        if (currentActivePanel != null)
            currentActivePanel.SetActive(false);

        //panelBackground.SetActive(true);    
        panelHome.SetActive(true);
    }
}