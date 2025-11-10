using UnityEditor.EditorTools;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string type;
    public string description;
    public Sprite icon;

    [HideInInspector]
    public bool pickedUp;

    [HideInInspector]
    public bool equipped;

    [HideInInspector]
    public GameObject toolManager;

    [HideInInspector]
    public GameObject tool;

    public bool playerTool;

    private void Start()
    {
        toolManager = GameObject.FindWithTag("ToolManager");

        if(!playerTool)
        {
            int allTools = toolManager.transform.childCount;
            for (int i = 0; i < allTools; i++)
            {
                if (toolManager.transform.GetChild(i).gameObject.GetComponent<Item>().ID == ID)
                {
                    tool = toolManager.transform.GetChild(i).gameObject;
                }
            }
        }
    }

    private void Update()
    {
        if (equipped)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                equipped = false;
            }
            if(equipped == false)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void ItemUsage()
    {
        if (type == "Tool")
        {
            tool.SetActive(true);
            tool.GetComponent<Item>().equipped = true;
        }
    }
}
