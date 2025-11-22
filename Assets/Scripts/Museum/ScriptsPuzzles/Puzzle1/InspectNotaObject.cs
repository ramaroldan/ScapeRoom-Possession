using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InspectNotaObject : InteractableBase
{
    [Header("UI Nota (Canvas 2D)")]
    [Tooltip("Panel de UI que contiene la imagen de la nota (GameObject hijo del Canvas).")]
    public GameObject notaPanel;

    [Tooltip("Componente Image donde se mostrará la nota.")]
    public Image notaImage;

    [Tooltip("Sprite de la nota (la imagen con el texto ya creado).")]
    public Sprite notaSprite;

    [Header("Input para cerrar la nota")]
    [Tooltip("Tecla para cerrar la nota además del botón en pantalla.")]
    public KeyCode cerrarKey = KeyCode.Escape;

    protected override void Start()
    {
        base.Start();

        // Asegurarnos de que al inicio la nota esté oculta
        if (notaPanel != null)
            notaPanel.SetActive(false);
    }

    private void Update()
    {
        // Si la nota está abierta, permitir cerrarla con la tecla definida
        if (notaPanel != null && notaPanel.activeSelf)
        {
            if (Input.GetKeyDown(cerrarKey))
            {
                CloseNota();
            }
        }
    }

    // -------------------------------------------------------------------
    // 🔥 Acción principal al interactuar con el objeto de la nota
    // -------------------------------------------------------------------
    protected override IEnumerator DoInteraction()
    {
        if (state)   // abrir nota
        {
            // Asignar sprite de la nota
            if (notaImage != null && notaSprite != null)
                notaImage.sprite = notaSprite;

            if (notaPanel != null)
                notaPanel.SetActive(true);

            // Bloquear control de la cámara/jugador
            foreach (var look in lookScripts)
                look.working = false;

            if (playerScript != null)
                playerScript.SetWorking(false);

            // Deshabilitar el script de interacción mientras la UI está abierta
            if (interactScript != null)
                interactScript.enabled = false;

            // Activar mouse para mover el cursor sobre la UI
            SetPlayerControl(true);
        }
        else // cerrar nota
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

    // -------------------------------------------------------------------
    // 🔥 Control del mouse/cursor (misma lógica que tu script base)
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
    // 🔥 Acción llamada desde botón “Cerrar” del panel de la nota
    // -------------------------------------------------------------------
    public void CloseNota()
    {
        if (state) // si internamente está en estado "abierto"
        {
            state = false;
            StartCoroutine(DoInteraction());
        }
    }
}
