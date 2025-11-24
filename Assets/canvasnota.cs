using UnityEngine;

public class canvasnota : MonoBehaviour
{
    [SerializeField] private GameObject panelPausa;

    public void CerrarPanel()
    {
        panelPausa.SetActive(false);
    }
}
