using UnityEngine;

public class UseItemFromInventory : MonoBehaviour
{
    public Transform dropPoint; // frente al jugador

    public void UseItem(Item item)
    {
        if (item == null) return;

        GameObject worldObj = item.worldRepresentation;

        if (worldObj == null)
        {
            Debug.LogError("El Item no tiene asignado worldRepresentation.");
            return;
        }

        // volver a activar el objeto físico
        worldObj.SetActive(true);

        // ponerlo frente al jugador
        worldObj.transform.position = dropPoint.position;
        worldObj.transform.rotation = dropPoint.rotation;

        // permitir interacción y colisiones
        var col = worldObj.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        var rb = worldObj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        Debug.Log("Item usado: " + item.description);
    }
}
