using UnityEngine;
using TMPro;

public class VictoryPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recordText;

    private void Start()
    {
        float victoryTime = PlayerPrefs.GetFloat("VictoryTime", 0f);
        int minutes = Mathf.FloorToInt(victoryTime / 60f);
        int seconds = Mathf.FloorToInt(victoryTime % 60f);

        recordText.text = $"{minutes:00}:{seconds:00}";
    }
}
