using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CloseCanvas : InteractableBase
{
    [Header("UI Nota (Canvas 2D)")]
    [Tooltip("Panel de UI que contiene la imagen de la nota (GameObject hijo del Canvas).")]
    public GameObject notaPanel;

    [Tooltip("Componente Image donde se mostrará la nota.")]
    public Image notaImage;

    [Tooltip("Sprite de la nota (la imagen con el texto ya creado).")]
    public Sprite notaSprite;

    void Start()
    {
        base.Start();
    }
    protected override IEnumerator DoInteraction()
    {
        if (!state)   // abrir nota
        { 
            if (notaPanel != null)
                notaPanel.SetActive(false);

            // Restaurar control de cámara/jugador
            foreach (var look in lookScripts)
                look.working = true;

            if (playerScript != null)
                playerScript.SetWorking(true);

            // Volver a habilitar interacción
            if (interactScript != null)
                interactScript.enabled = true;

            SetPlayerControl(false);
        }

        // Esperar fin del audio (si hay) o un pequeño delay
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(.3f);

        canInteract = true;
    }
    public void CloseCanvasBoton()
    {
        if (notaPanel != null)
            notaPanel.SetActive(false);

        // Restaurar control de cámara/jugador
        foreach (var look in lookScripts)
            look.working = true;

        if (playerScript != null)
            playerScript.SetWorking(true);

        // Volver a habilitar interacción
        if (interactScript != null)
            interactScript.enabled = true;

        SetPlayerControl(false);
    }
    private void SetPlayerControl(bool isUIActive)
    {
        if (mouseLookPlayer != null)
            mouseLookPlayer.overrideCursorLock = isUIActive;

        if (mouseLookCamera != null)
            mouseLookCamera.overrideCursorLock = isUIActive;

        Cursor.visible = isUIActive;
        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
