using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PickupItemObject : InteractableBase
{
    [Header("Item asociado")]
    [Tooltip("Referencia al componente Item de este objeto. Si se deja vacío, se buscará en el mismo GameObject.")]
    public Item itemData;

    [Header("Comportamiento al recoger")]
    [Tooltip("Desactivar el objeto del mundo al recogerlo.")]
    public bool disableWorldObject = true;

    [Tooltip("Usar automáticamente el item al recogerlo (llama a ItemUsage). Útil para Tools.")]
    public bool autoUseOnPickup = false;

    [Header("Eventos")]
    [Tooltip("Se dispara cuando el item se recoge correctamente (puedes enganchar tu Inventory aquí).")]
    public UnityEvent onItemPicked;

    private Inventory inventory;

    protected override void Start()
    {
        base.Start();

        // Si no se asignó manualmente, intentamos obtener el Item del mismo objeto
        if (itemData == null)
            itemData = GetComponent<Item>();

        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();
    }

    // -------------------------------------------------------------------
    // 🔥 Interacción principal (click / tecla E)
    // -------------------------------------------------------------------
    protected override IEnumerator DoInteraction()
    {
        // Si ya fue recogido, no hacemos nada
        if (itemData != null && itemData.pickedUp)
        {
            yield return new WaitForSeconds(0.1f);
            canInteract = true;
            yield break;
        }

        // RECOGER ITEM
        if (itemData != null)
        {
            itemData.pickedUp = true;

            // Si queremos que se use automáticamente (por ej. herramientas)
            if (autoUseOnPickup)
            {
                itemData.ItemUsage();
            }
        }

        // Lanzar eventos (aquí puedes enganchar tu Inventory.AddItem())
        //onItemPicked?.Invoke();
        GameObject itemPickedUp = this.gameObject;

        //Item item = itemPickedUp.GetComponent<Item>();
        inventory.AddItem(itemPickedUp, itemData.ID, itemData.type, itemData.description, itemData.icon);

        // Desactivar objeto del mundo si corresponde
        if (disableWorldObject)
        {
            gameObject.SetActive(false);
        }

        // Esperar fin de audio (si hay) o un pequeño delay, igual que en InspectNotaObject
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(0.2f);

        canInteract = true;
    }
}
