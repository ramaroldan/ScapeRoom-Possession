using TMPro;
using UnityEngine;

public class DificultySelectorUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI legendText;

    public void ShowLegend(string localizationKey)
    {
        string localizedText = LocalizationManager.Instance.GetText(localizationKey);
        legendText.text = localizedText;
    }

    public void ClearLegend()
    {
        if (legendText != null)
            legendText.text = "";
    }
}
