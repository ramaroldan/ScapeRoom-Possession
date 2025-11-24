using Michsky.UI.Dark;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private bool inventoryEnabled;
    public GameObject inventory;

    private int allSlots;
    private int enabledSlots;

    private GameObject[] slot;
    public GameObject slotHolder;

    [SerializeField] private MouseLook mouseLookPlayer;
    [SerializeField] private MouseLook mouseLookCamera;
    

    [SerializeField] private PlayerMovement playerScript;
    [SerializeField] private Interact interactScript;


    private void Awake()
    {
        // Singleton para evitar duplicados
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
            {
                slot[i].GetComponent<Slot>().empty = true;
            }
        }
        inventoryEnabled = true;

        // Mostrar/ocultar el inventario
        inventory.SetActive(true);
        inventoryEnabled =false;

        // Mostrar/ocultar el inventario
        inventory.SetActive(false);

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        inventoryEnabled = !inventoryEnabled;

        // Mostrar/ocultar el inventario
        inventory.SetActive(inventoryEnabled);

        // bloquear control del player
        mouseLookPlayer.working = !inventoryEnabled;
        mouseLookCamera.working = !inventoryEnabled;

        if (playerScript != null)
            playerScript.SetWorking(!inventoryEnabled);

        // bloquear el Interact para que no detecte raycasts mientras UI está abierta
        if (interactScript != null)
            interactScript.enabled = !inventoryEnabled;

        SetPlayerControl(inventoryEnabled);


    }
    public void SetPlayerControl(bool isUIActive)
    {
        if (mouseLookPlayer != null)
        {
            mouseLookPlayer.overrideCursorLock = isUIActive;
            // Debug.Log("Override cursor lock seteado a: " + mouseLookPlayer.overrideCursorLock);
        }
        if (mouseLookCamera != null)
        {
            mouseLookCamera.overrideCursorLock = isUIActive;
            // Debug.Log("Override cursor lock seteado a: " + mouseLookCamera.overrideCursorLock);
        }

        Cursor.visible = isUIActive;
        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
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
            if (slot[i].GetComponent<Slot>().empty)
            {
                itemObject.GetComponent<Item>().pickedUp = true;

                slot[i].GetComponent<Slot>().item = itemObject;
                slot[i].GetComponent<Slot>().ID = itemID;

                slot[i].GetComponent<Slot>().type = itemType;
                slot[i].GetComponent<Slot>().description = itemDescription;
                slot[i].GetComponent<Slot>().icon = itemIcon;
                
                itemObject.transform.parent = slot[i].transform;
                itemObject.SetActive(false);

                slot[i].GetComponent<Slot>().UpdateSlot();

                slot[i].GetComponent<Slot>().empty = false;
                return;
            }
            //return;
        }
    }

}
