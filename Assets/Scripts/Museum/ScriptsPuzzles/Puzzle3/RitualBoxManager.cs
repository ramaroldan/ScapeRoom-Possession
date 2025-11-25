using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RitualBoxManager : MonoBehaviour
{
    private RoomHintProvider hintProvider;

    [Header("Eventos")]
    public UnityEvent onPuzzleCompleted;   // se dispara cuando aparece la llave


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

        hintProvider = FindObjectOfType<RoomHintProvider>();

    }

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
        // 🔹 Dejar de ser hijo del dropPoint (o de lo que sea)
        obj.transform.SetParent(null);

        // Congelar física para que quede fijo en la caja
        var rb = obj.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Colocarlo exactamente en el slot
        obj.transform.position = slot.position;
        obj.transform.rotation = slot.rotation;
        obj.transform.SetParent(slot);   // ahora es hijo del slot, ya no del dropPoint

        // Evitar volver a recogerlo
        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        var pickup = obj.GetComponent<PickupItemObject>();
        if (pickup != null)
            pickup.enabled = false; // ya no entra al inventario de nuevo
    }


    private void CheckCompletion()
    {
        if (relojColocado && rosarioColocado && calaveraColocado)
        {
            hintProvider.AdvancePuzzleHint(nameof(RitualBoxManager));
            Debug.Log("Puzzle final completado. La llave aparece.");

            if (finalKey != null)
                finalKey.SetActive(true);

            // 🔥 disparar sustos / sonidos / efectos
            onPuzzleCompleted?.Invoke();
        }
    }

}
