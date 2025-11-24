using System.Collections.Generic;
using UnityEngine;

public class DoorLockByPadlock : MonoBehaviour
{
    private RoomHintProvider hintProvider;

    [Header("Interacción de la puerta")]
    [Tooltip("Script que maneja la interacción de la puerta (por ejemplo InspectPanel o InteractableBase específico).")]
    public MonoBehaviour doorInteractScript;

    [Tooltip("Collider que se usa para hacer raycast / clic sobre la puerta.")]
    public Collider doorCollider;

    [Header("Visual opcional")]
    [Tooltip("Puerta cerrada (si usas dos modelos: cerrada/abierta).")]
    public GameObject closedDoorVisual;

    [Tooltip("Puerta abierta (opcional, se activa al desbloquear).")]
    public GameObject openDoorVisual;

    [Header("Estado")]
    public bool lockedAtStart = true;

    private bool unlocked = false;

    private void Start()
    {
        if (lockedAtStart)
            LockDoor();
        else
            UnlockDoor();

        hintProvider = FindObjectOfType<RoomHintProvider>();

        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(2, nameof(PcController), new List<string>
        {
                "Esos objetos que se repiten en la habitación... ¿no te suenan de los posters?",
                "Contá cuántas veces aparece cada objeto del poster en la sala.",
                "Poné los números en el mismo orden en el que están los posters."
        });
    }

    // 🔒 Bloquear interacción (por si querés relockear en algún momento)
    public void LockDoor()
    {
        unlocked = false;

        if (doorInteractScript != null)
            doorInteractScript.enabled = false;

        if (doorCollider != null)
            doorCollider.enabled = false;

        if (closedDoorVisual != null)
            closedDoorVisual.SetActive(true);

        if (openDoorVisual != null)
            openDoorVisual.SetActive(false);
    }

    // 🔓 Llamar a esto DESDE el evento del candado cuando se resuelve
    public void UnlockDoor()
    {
        hintProvider.AdvancePuzzleHint(nameof(PcController));

        if (unlocked) return;
        unlocked = true;

        if (doorInteractScript != null)
            doorInteractScript.enabled = true;

        if (doorCollider != null)
            doorCollider.enabled = true;

        if (closedDoorVisual != null)
            closedDoorVisual.SetActive(false);

        if (openDoorVisual != null)
            openDoorVisual.SetActive(true);

        Debug.Log("DoorLockByPadlock: puerta desbloqueada por el candado.");
    }
}
