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

    //    protected override void Start()
    //    {
    //        base.Start();

    //        if (keypadCam == null)
    //            Debug.LogError("❌ Asigná la cámara del Keypad en el inspector.");

    //        keypadCam.enabled = false;

    //        keypadButtons = GetComponentsInChildren<KeypadButton>(true);

    //        if (keypadButtons.Length == 0)
    //            Debug.LogError("❌ No encontré botones KeypadButton dentro del Keypad.");
    //    }

    //    protected override IEnumerator DoInteraction()
    //    {
    //        if (keypadActive)
    //        {
    //            canInteract = true;
    //            yield break;
    //        }

    //        keypadActive = true;
    //        // DESACTIVAR highlight y collider del objeto interactuable
    //        if (highlightRenderer != null)
    //            highlightRenderer.enabled = false;

    //        var coll = GetComponent<Collider>();
    //        if (coll != null)
    //            coll.enabled = false;


    //        Debug.Log("🔢 KEYAPD MODE ON");

    //        playerCam = cam;


    //        // bloquear control del player
    //        foreach (var look in lookScripts)
    //            look.working = false;

    //        if (playerScript != null)
    //            playerScript.SetWorking(false);

    //        interactScript.message = "";
    //        // bloquear el Interact para que no detecte raycasts mientras UI está abierta
    //        // Apagar UI del interact sí o sí
    //        //if (interactScript != null)
    //        //{
    //        //    if (interactScript.InteractionUI != null)
    //        //        interactScript.InteractionUI.SetActive(false);

    //        //    interactScript.enabled = false;
    //        //}
    //        //// 2. Desactivar raycast en UI (botón “Interact” invisible)
    //        //if (interactScript != null && interactScript.InteractionUI != null)
    //        //{
    //        //    foreach (var g in interactScript.InteractionUI.GetComponentsInChildren<UnityEngine.UI.Graphic>(true))
    //        //        g.raycastTarget = false;
    //        //}
    //        //// 3. Desactivar LineRenderer o rayos visuales del sistema
    //        //foreach (var lr in FindObjectsOfType<LineRenderer>(true))
    //        //    lr.enabled = false;

    //        //// 4. Desactivar cualquier collider “raro” en hijos de la cámara
    //        //foreach (var col in cam.GetComponentsInChildren<Collider>(true))
    //        //    col.enabled = false;

    //        playerCam.enabled = false;
    //        // activar mouse
    //        SetPlayerControl(true);

    //        DisablePlayerRaycastSystems(false);

    //        // -------- ACTIVAR CAMARA KEYPAD --------
    //        keypadCam.enabled = true;

    //        // -------- ESPERAR SALIDA (CLICK DERECHO) --------
    //        yield return StartCoroutine(WaitForExit());

    //        // REACTIVAR highlight y collider
    //        if (highlightRenderer != null)
    //            highlightRenderer.enabled = true;

    //        coll = GetComponent<Collider>();
    //        if (coll != null)
    //            coll.enabled = true;


    //        // -------- RESTAURAR SISTEMAS --------
    //        keypadCam.enabled = false;
    //        playerCam.enabled = true;

    //        //interactScript.enabled = true;
    //        //if (interactScript.InteractionUI != null)
    //        //    interactScript.InteractionUI.SetActive(true);
    //        DisablePlayerRaycastSystems(true);
    //        SetPlayerControl(true);

    //        Debug.Log("🔙 KEYAPD MODE OFF");

    //        keypadActive = false;
    //        canInteract = true;
    //    }
    //    void DisablePlayerRaycastSystems(bool value)
    //    {
    //        // 1. Desactivar todos los colliders del Interact UI
    //        if (interactScript != null && interactScript.InteractionUI != null)
    //        {
    //            foreach (var col in interactScript.InteractionUI.GetComponentsInChildren<Collider>(true))
    //                col.enabled = value;
    //        }

    //        // 2. Desactivar raycast en UI (botón “Interact” invisible)
    //        if (interactScript != null && interactScript.InteractionUI != null)
    //        {
    //            foreach (var g in interactScript.InteractionUI.GetComponentsInChildren<UnityEngine.UI.Graphic>(true))
    //                g.raycastTarget = value;
    //        }

    //        // 3. Desactivar LineRenderer o rayos visuales del sistema
    //        foreach (var lr in FindObjectsOfType<LineRenderer>(true))
    //            lr.enabled = value;

    //        // 4. Desactivar cualquier collider “raro” en hijos de la cámara
    //        foreach (var col in cam.GetComponentsInChildren<Collider>(true))
    //            col.enabled = value;

    //        if (interactScript != null)
    //        {
    //            if (interactScript.InteractionUI != null)
    //                interactScript.InteractionUI.SetActive(value);

    //            interactScript.enabled = value;
    //        }
    //    }

    //    private IEnumerator WaitForExit()
    //    {
    //        while (true)
    //        {
    //            if (Input.GetMouseButtonDown(1)) // CLICK DERECHO
    //                break;

    //            yield return null;
    //        }
    //    }

    //    private void Update()
    //    {
    //        if (!keypadActive) return;

    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            Ray ray = keypadCam.ScreenPointToRay(Input.mousePosition);
    //            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red, 1f);


    //            if (Physics.Raycast(ray, out RaycastHit hit, 3f))
    //            {
    //                if (hit.collider.TryGetComponent<KeypadButton>(out var bttn))
    //                {
    //                    bttn.PressButton();
    //                }
    //            }
    //        }
    //    }



    //    private void SetPlayerControl(bool isUIActive)
    //    {
    //        if (mouseLookPlayer != null)
    //            mouseLookPlayer.overrideCursorLock = isUIActive;

    //        if (mouseLookCamera != null)
    //            mouseLookCamera.overrideCursorLock = isUIActive;

    //        Cursor.visible = isUIActive;
    //        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
    //    }
    //}
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

        // Esperar salida
        yield return StartCoroutine(WaitForExit());

        // ---------------------------------------------------
        // 🔥 RESTAURAR TODO 🔥
        keypadCam.enabled = false;
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
