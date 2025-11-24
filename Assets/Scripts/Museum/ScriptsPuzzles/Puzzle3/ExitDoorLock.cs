using UnityEngine;

public class ExitDoorLock : MonoBehaviour
{
    [Header("Referencia al item 'llave'")]
    public Item keyItem;      // El Item de la llave final

    [Header("Trigger de la puerta")]
    public Collider doorTrigger;   // El collider de la puerta (IsTrigger)

    [Header("Susto opcional al desbloquear")]
    public JumpscareController unlockJumpscare;

    private bool unlocked = false;

    private void Start()
    {
        // Puerta bloqueada al inicio
        if (doorTrigger != null)
            doorTrigger.enabled = false;
    }

    private void Update()
    {
        if (unlocked) return;

        // Cuando la llave entra al inventario
        if (keyItem != null && keyItem.pickedUp)
        {
            UnlockDoor();
        }
    }

    private void UnlockDoor()
    {
        unlocked = true;

        if (doorTrigger != null)
            doorTrigger.enabled = true;

        if (unlockJumpscare != null)
            unlockJumpscare.PlayJumpscare();

        Debug.Log("Puerta de salida desbloqueada.");
    }
}
