using UnityEngine;

public class DoorLockByPadlock : MonoBehaviour
{
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
