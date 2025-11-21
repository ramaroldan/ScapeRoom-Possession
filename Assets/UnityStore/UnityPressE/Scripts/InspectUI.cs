using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InspectPanel : InteractableBase
{
    [Header("UI Panel To Toggle")]
    public GameObject panelUI;
    public Image targetImage;
    public Sprite displayedSprite;

    protected override void Start()
    {
        base.Start();

        if (panelUI != null)
            panelUI.SetActive(false);
    }

    // -------------------------------------------------------------------
    // 🔥 Acción principal al interactuar (abre/cierra la UI)
    // -------------------------------------------------------------------
    protected override IEnumerator DoInteraction()
    {
        if (state)   // abrir UI
        {
            // setear imagen si corresponde
            if (targetImage != null && displayedSprite != null)
                targetImage.sprite = displayedSprite;

            if (panelUI != null)
                panelUI.SetActive(true);

            // bloquear control del player
            foreach (var look in lookScripts)
                look.working = false;

            if (playerScript != null)
                playerScript.SetWorking(false);

            // bloquear el Interact para que no detecte raycasts mientras UI está abierta
            if (interactScript != null)
                interactScript.enabled = false;

            // activar mouse
            SetPlayerControl(true);
        }
        else // cerrar UI
        {
            if (panelUI != null)
                panelUI.SetActive(false);

            foreach (var look in lookScripts)
                look.working = true;

            if (playerScript != null)
                playerScript.SetWorking(true);

            // restaurar interacción
            if (interactScript != null)
                interactScript.enabled = true;

            SetPlayerControl(false);
        }

        // esperar fin del audio
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(.3f);

        canInteract = true;
    }

    // -------------------------------------------------------------------
    // 🔥 Control del mouse/cursor (misma lógica que tu script original)
    // -------------------------------------------------------------------
    private void SetPlayerControl(bool isUIActive)
    {
        if (mouseLookPlayer != null)
            mouseLookPlayer.overrideCursorLock = isUIActive;

        if (mouseLookCamera != null)
            mouseLookCamera.overrideCursorLock = isUIActive;

        Cursor.visible = isUIActive;
        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // -------------------------------------------------------------------
    // 🔥 Acción llamada desde botón “Cerrar” del panel
    // -------------------------------------------------------------------
    public void CloseUI()
    {
        if (state) // si está abierta
        {
            state = false;
            StartCoroutine(DoInteraction());
        }
    }
}

