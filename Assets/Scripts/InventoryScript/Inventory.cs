
using Unity.VisualScripting;
using UnityEngine;  

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private bool inventoryEnabled;
    public GameObject inventory;
    public GameObject panel_scene;
    public GameObject panel_pistas;

    private int allSlots;
    private int enabledSlots;

    private GameObject[] slot;
    public GameObject slotHolder;

    public GameObject Btn_Useitem;
    public GameObject titulo_description;
    public GameObject icon_description;
    public GameObject text_description;

    [SerializeField] private MouseLook mouseLookPlayer;
    [SerializeField] private MouseLook mouseLookCamera;

    [SerializeField] private PlayerMovement playerScript;
    [SerializeField] private Interact interactScript;

    // 🔹 NUEVO: selección de slots + dropPoint
    [Header("Selección y uso de items")]
    public Slot selectedSlot;          // Último slot clickeado
    public Transform dropPoint;        // Punto donde se coloca el objeto al usarlo

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        allSlots = slotHolder.transform.childCount;
        slot = new GameObject[allSlots];

        for (int i = 0; i < allSlots; i++)
        {
            slot[i] = slotHolder.transform.GetChild(i).gameObject;

            if (slot[i].GetComponent<Slot>().item == null)
                slot[i].GetComponent<Slot>().empty = true;
        }

        // Inicializar inventario apagado
        inventory.SetActive(true);
        inventoryEnabled = false;
        inventory.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            OpenInventory();
    }

    public void OpenInventory()
    {
        inventoryEnabled = !inventoryEnabled;
        
        inventory.SetActive(inventoryEnabled);
        panel_pistas.SetActive(false);
        panel_scene.SetActive(false);
        // bloquear control del player
        mouseLookPlayer.working = !inventoryEnabled;
        mouseLookCamera.working = !inventoryEnabled;

        if (playerScript != null)
            playerScript.SetWorking(!inventoryEnabled);

        if (interactScript != null)
            interactScript.enabled = !inventoryEnabled;

        SetPlayerControl(inventoryEnabled);
    }
    public void OpenScene()
    {
        
        inventory.SetActive(false);
        panel_pistas.SetActive(false);
        panel_scene.SetActive(true);

    }
    public void OpenItems()
    {
        panel_scene.SetActive(false);
        panel_pistas.SetActive(false);
        inventory.SetActive(true);
        

    }
    public void OpenPistas()
    {
        panel_pistas.SetActive(true);
        panel_scene.SetActive(false);
        inventory.SetActive(false);


    }

    public void SetPlayerControl(bool isUIActive)
    {
        if (mouseLookPlayer != null)
            mouseLookPlayer.overrideCursorLock = isUIActive;

        if (mouseLookCamera != null)
            mouseLookCamera.overrideCursorLock = isUIActive;

        Cursor.visible = isUIActive;
        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // 🔹 llamado desde Slot.OnPointerClick
    public void SetSelectedSlot(Slot slotToSelect)
    {
        selectedSlot = slotToSelect;
        Debug.Log("Slot seleccionado: " + selectedSlot.name);

        if (selectedSlot.description != "")
        {
            titulo_description.SetActive(true);
            icon_description.SetActive(true);
            text_description.SetActive(true);
            //titulo_description.GetComponent<UnityEngine.UI.Text>().text = selectedSlot.description;
            icon_description.GetComponent<UnityEngine.UI.Image>().sprite = selectedSlot.icon;
            text_description.GetComponent<TMPro.TextMeshProUGUI>().text = selectedSlot.description;

        }
        else
        {
            titulo_description.SetActive(false);
            icon_description.SetActive(false);
            text_description.SetActive(false);
        }
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            GameObject itemPickedUp = other.gameObject;
            Item item = itemPickedUp.GetComponent<Item>();

            AddItem(itemPickedUp, item.ID, item.type, item.description, item.icon);
        }
    }

    public void AddItem(GameObject itemObject, int itemID, string itemType, string itemDescription, Sprite itemIcon)
    {
        for (int i = 0; i < allSlots; i++)
        {
            var s = slot[i].GetComponent<Slot>();

            if (s.empty)
            {
                itemObject.GetComponent<Item>().pickedUp = true;

                s.item = itemObject;
                s.ID = itemID;
                s.type = itemType;
                s.description = itemDescription;
                s.icon = itemIcon;

                itemObject.transform.SetParent(slot[i].transform);
                itemObject.SetActive(false);

                if (itemDescription == "Calavera")
                {
                    Btn_Useitem.SetActive(true);
                }
                s.UpdateSlot();
                s.empty = false;
                return;
            }
        }
    }

    // 🔹 llamado por el botón "Usar Item"
    public void UseSelectedItem()
    {

        if (dropPoint.childCount>0)
        {
            return;
        }

        if (selectedSlot == null || selectedSlot.empty)
        {
            Debug.Log("No hay item seleccionado.");
            return;
        }

        GameObject itemObject = selectedSlot.item;
        if (itemObject == null)
        {
            Debug.Log("Error: el slot seleccionado no tiene item.");
            return;
        }

        Item itemData = itemObject.GetComponent<Item>();

        // 🔹 Si es un tool (linterna, etc.) usar la lógica original
        if (itemData != null && itemData.type == "Tool")
        {
            selectedSlot.UseItem();
            return;
        }

        // 🔹 OBJETOS DE PUZZLE (reloj, rosario, calavera)
        // limpiar slot de inventario
        selectedSlot.item = null;
        selectedSlot.empty = true;
        selectedSlot.icon = null;
        selectedSlot.UpdateSlot();

        // desparentar del slot
        itemObject.transform.SetParent(null);

        // poner delante del jugador y hacerlo hijo del dropPoint
        if (dropPoint != null)
        {
            itemObject.transform.SetParent(dropPoint);
            itemObject.transform.localPosition = Vector3.zero;
            itemObject.transform.localRotation = Quaternion.identity;
        }

        itemObject.SetActive(true);

        // 🔹 Desactivar PickupItemObject (para que no vuelva al inventario)
        var pickupScript = itemObject.GetComponent<PickupItemObject>();
        if (pickupScript != null)
            pickupScript.enabled = false;

        // habilitar collider y física
        Collider col = itemObject.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        Rigidbody rb = itemObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        if (itemData != null)
            itemData.pickedUp = false;

        Debug.Log("Item equipado: " + (itemData != null ? itemData.description : itemObject.name));
    }

   
}
