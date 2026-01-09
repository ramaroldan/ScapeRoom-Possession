using TMPro;
using UnityEngine;

public class DificultySelectorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI legendText;

    public void ShowLegend(string message)
    {
        if (legendText != null)
            legendText.text = message;
    }

    public void ClearLegend()
    {
        if (legendText != null)
            legendText.text = "";
    }
}
