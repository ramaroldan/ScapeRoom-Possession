using NavKeypad;
using System.Collections;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class KeypadInteractable : InteractableBase
{
    [Header("Keypad Camera")]
    public Camera keypadCam;

    private Camera playerCam;
    private bool keypadActive = false;

    private KeypadButton[] keypadButtons;

 
    protected override void Start()
    {
        base.Start();

        if (keypadCam == null)
            Debug.LogError("❌ Asigná la cámara del Keypad en el inspector.");

        keypadCam.enabled = false;

        keypadButtons = GetComponentsInChildren<KeypadButton>(true);

        if (keypadButtons.Length == 0)
            Debug.LogError("❌ No encontré botones KeypadButton dentro del Keypad.");
    }

    // ---------------------------------------------------
    protected override IEnumerator DoInteraction()
    {
        if (keypadActive)
        {
            canInteract = true;
            yield break;
        }

        keypadActive = true;

        Debug.Log("🔢 KEYAPD MODE ON");

        // Guardar cámara del player
        playerCam = cam;
        playerCam.enabled = false;

        // Apagar highlight / collider de este interactable
        if (highlightRenderer != null) highlightRenderer.enabled = false;
        var coll = GetComponent<Collider>();
        if (coll != null) coll.enabled = false;

        // 🔥 DESACTIVAR SISTEMAS DEL PLAYER 🔥
        DisablePlayerSystems(false);

        // Activar control del mouse
        SetPlayerControl(true);

        // Activar cámara del keypad
        keypadCam.enabled = true;
        keypadCam.GetComponent<AudioListener>().enabled = true;
        // Esperar salida
        yield return StartCoroutine(WaitForExit());

        // ---------------------------------------------------
        // 🔥 RESTAURAR TODO 🔥
        keypadCam.enabled = false;
        keypadCam.GetComponent<AudioListener>().enabled = false;
        playerCam.enabled = true;

        if (highlightRenderer != null) highlightRenderer.enabled = true;
        if (coll != null) coll.enabled = true;

        DisablePlayerSystems(true);
        SetPlayerControl(false);

        Debug.Log("🔙 KEYAPD MODE OFF");

        keypadActive = false;
        canInteract = true;
    }

    // ---------------------------------------------------
    void DisablePlayerSystems(bool value)
    {
        // bloquear movimiento / cámara
        foreach (var look in lookScripts) look.working = value;

        if (playerScript != null)
            playerScript.SetWorking(value);

        // desactivar el sistema de interacción
        if (interactScript != null)
        {
            interactScript.enabled = value;

            if (interactScript.InteractionUI != null)
                interactScript.InteractionUI.SetActive(value);
        }

        // por si tuvieras LineRenderer
        foreach (var lr in FindObjectsOfType<LineRenderer>(true))
            lr.enabled = value;
    }

    // ---------------------------------------------------
    private IEnumerator WaitForExit()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(1)) // CLICK DERECHO
                break;

            yield return null;
        }
    }

    // ---------------------------------------------------
    private void Update()
    {
        if (!keypadActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = keypadCam.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f);

            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
            {
                if (hit.collider.TryGetComponent<KeypadButton>(out var bttn))
                {
                    bttn.PressButton();
                }
            }
        }
    }

    // ---------------------------------------------------
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
