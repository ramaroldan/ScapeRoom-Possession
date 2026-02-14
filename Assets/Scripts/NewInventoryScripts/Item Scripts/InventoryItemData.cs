using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Scriptable Objects/InventoryItem")]
public class InventoryItemData : ScriptableObject
{
    public GameObject worldRepresentation;
    public int ID;
    public string Type;
    public string Description;
    public Sprite Icon;
    public int MaxStackSize;

    [HideInInspector]
    public bool pickedUp;

    [HideInInspector]
    public bool equipped;

    [HideInInspector]
    public GameObject toolManager;

    [HideInInspector]
    public GameObject tool;

    public bool playerTool;
}
