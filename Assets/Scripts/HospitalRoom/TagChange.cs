using UnityEngine;

public class TagChange : MonoBehaviour
{
    public void SetInteractable()
    {
        this.gameObject.tag = "Interactable";
    }

    public void SetUsedObject()
    {
        this.gameObject.tag = "UsedObject";
    }

}
