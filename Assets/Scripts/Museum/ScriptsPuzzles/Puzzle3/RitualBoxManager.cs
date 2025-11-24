using UnityEngine;

public class RitualBoxManager : MonoBehaviour
{
    [Header("Slots para colocar cada objeto")]
    public Transform slotReloj;
    public Transform slotRosario;
    public Transform slotCalavera;

    [Header("Prefab / Objeto de la llave final")]
    public GameObject finalKey;  // se activa cuando los 3 están colocados

    [Header("IDs de los objetos correctos")]
    public string relojItemID = "reloj";
    public string rosarioItemID = "rosario";
    public string calaveraItemID = "calavera";

    private bool relojColocado = false;
    private bool rosarioColocado = false;
    private bool calaveraColocado = false;

    private void Start()
    {
        if (finalKey != null)
            finalKey.SetActive(false); // la llave aparece solo al final
    }

    // Llamado por cada objeto cuando se coloca en un slot
    public void TryPlaceItem(string itemID, GameObject worldObject)
    {
        if (itemID == relojItemID && !relojColocado)
        {
            PlaceObject(worldObject, slotReloj);
            relojColocado = true;
        }
        else if (itemID == rosarioItemID && !rosarioColocado)
        {
            PlaceObject(worldObject, slotRosario);
            rosarioColocado = true;
        }
        else if (itemID == calaveraItemID && !calaveraColocado)
        {
            PlaceObject(worldObject, slotCalavera);
            calaveraColocado = true;
        }
        else
        {
            Debug.Log("Objeto incorrecto o ya colocado.");
            return;
        }

        CheckCompletion();
    }

    private void PlaceObject(GameObject obj, Transform slot)
    {
        // Opcional: si el objeto tiene Rigidbody, lo fijamos
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Lo colocamos en el slot
        obj.transform.position = slot.position;
        obj.transform.rotation = slot.rotation;
        obj.transform.SetParent(slot);

        // Evitar que se vuelva a recoger / mover
        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        var pickup = obj.GetComponent<PickupItemObject>();
        if (pickup != null)
            pickup.enabled = false; // que no vuelva a intentar entrar al inventario
    }

    private void CheckCompletion()
    {
        if (relojColocado && rosarioColocado && calaveraColocado)
        {
            Debug.Log("Puzzle final completado. La llave aparece.");

            if (finalKey != null)
                finalKey.SetActive(true);
        }
    }
}
