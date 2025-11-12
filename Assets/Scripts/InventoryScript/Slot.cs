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
            if (image != null && icon != null)
            {
                image.sprite = icon;
            }
        }
    }

    public void UseItem()
    {
        item.GetComponent<Item>().ItemUsage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UseItem();
    }
}
