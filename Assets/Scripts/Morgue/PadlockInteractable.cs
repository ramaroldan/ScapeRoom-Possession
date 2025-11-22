using System.Collections;
using UnityEngine;
using PadlockSystem;

public class PadlockInteractable : InteractableBase
{

    [Header("Padlock Setup")]
    [SerializeField] private PadlockController padlockController;
    [SerializeField] private GameObject padlockCamera;

    private Camera playerCamera;
    private bool padlockActive = false;


    protected override void Start()
    {
        base.Start();

        // Lanzar la corutina para esperar a la cámara
        StartCoroutine(SetupPadlockObject());
    }

    private IEnumerator SetupPadlockObject()
    {
        while (Camera.main == null)
            yield return null;

        playerCamera = Camera.main;

        Transform padlockTransform = null;

        // Buscar aunque esté desactivado
        foreach (Transform t in playerCamera.GetComponentsInChildren<Transform>(true))
        {
            if (t.name == "Padlock_Camera_Mechanism_A1")
            {
                padlockTransform = t;
                break;
            }
        }

        if (padlockTransform != null)
        {
            padlockCamera = padlockTransform.gameObject;
            padlockCamera.SetActive(false); // asegurarse que arranca apagado
            Debug.Log("<color=green>✔ Se asignó correctamente el objeto del padlock.</color>");
        }
        else
        {
            Debug.LogWarning("❌ No se encontró el objeto 'Padlock_Camera_Mechanism_A1'.");
        }
        padlockController.OnPadlockClosed.AddListener(OnPadlockClosedFromController);

    }
    private void OnPadlockClosedFromController()
    {
        StartCoroutine(ExitPadlockMode());
    }
    private IEnumerator ExitPadlockMode()
    {
        //padlockCamera.SetActive(false);
        playerCamera.enabled = true;
        SetPlayerControl(false);
      //  padlockActive = false;
        canInteract = true;

        yield return null;
    }

    protected override IEnumerator DoInteraction()
    {
        if (padlockActive)
        {
            canInteract = true;
            yield break;
        }

        padlockActive = true;

        Debug.Log("🔢 KEYAPD MODE ON");
               
       
        // Apagar highlight / collider de este interactable
        //if (highlightRenderer != null) highlightRenderer.enabled = false;
        //var coll = GetComponent<Collider>();
        //if (coll != null) coll.enabled = false;

        // 🔥 DESACTIVAR SISTEMAS DEL PLAYER 🔥
       // DisablePlayerSystems(false);

        // Activar control del mouse
        SetPlayerControl(true);

        // Activar cámara del keypad
        padlockCamera.SetActive(true);


        // 🔺 Mostrar el padlock
        padlockController.ShowPadlock();
        // Esperar salida
        yield return StartCoroutine(WaitForExit());

        // ---------------------------------------------------
        // 🔥 RESTAURAR TODO 🔥
        padlockCamera.SetActive(false);
       // padlockCamera.GetComponent<AudioListener>().enabled = false;
        playerCamera.enabled = true;

        //if (highlightRenderer != null) highlightRenderer.enabled = true;
        //if (coll != null) coll.enabled = true;

      //  DisablePlayerSystems(true);
        SetPlayerControl(false);

        Debug.Log("🔙 KEYAPD MODE OFF");

        padlockActive = false;
        canInteract = true;
       
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
}
