using UnityEngine;
using UnityEngine.UI;

public class SlidingTile : MonoBehaviour
{
    [Header("Índices")]
    [Tooltip("Índice de slot donde debería estar esta ficha cuando el puzzle está resuelto (0-8)")]
    public int correctSlotIndex;

    [HideInInspector] public int currentSlotIndex;
    [HideInInspector] public SlidingPuzzleManager manager;

    private RectTransform rectTransform;
    private Button button;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnClick);
    }

    public void Init(SlidingPuzzleManager m, int startSlotIndex)
    {
        manager = m;
        currentSlotIndex = startSlotIndex;
    }

    private void OnClick()
    {
        if (manager != null)
            manager.TryMoveTile(this);
    }

    public void MoveToSlot(RectTransform slotTransform)
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        rectTransform.position = slotTransform.position;
    }
}
