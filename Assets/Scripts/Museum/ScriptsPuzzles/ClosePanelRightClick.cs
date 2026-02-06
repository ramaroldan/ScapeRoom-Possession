using UnityEngine;

public class ClosePanelRightClick : MonoBehaviour
{
    public GameObject panel;

    void Update()
    {
        if (panel.activeSelf && Input.GetMouseButtonDown(1))
        {
            panel.SetActive(false);
        }
    }
}
