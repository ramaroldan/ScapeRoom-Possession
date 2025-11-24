using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject item;
    public int ID;
    public string type;
    public string description;

    public bool empty;
    public Sprite icon;

    public Transform slotIconGameObject;

    private void Start()
    {
        slotIconGameObject = transform.GetChild(0);
    }

    public void UpdateSlot()
    {
        if (slotIconGameObject != null)
        {
            Image image = slotIconGameObject.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = icon;
                image.enabled = icon != null;
            }
        }
    }

    public void UseItem()
    {
        if (item != null)
            item.GetComponent<Item>().ItemUsage();
    }

    // 🔹 Click en el slot: avisar al Inventory qué slot fue clickeado
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.Instance != null)
        {
            Inventory.Instance.SetSelectedSlot(this);
            Debug.Log("Slot clickeado: " + name);
        }
    }
}
