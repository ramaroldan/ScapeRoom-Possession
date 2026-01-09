using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Difficulty
{
    Easy = 0,
    Normal = 1,
    Hard = 2,
    Nightmare = 3
}

public class HintGameManager : MonoBehaviour
{
    public static HintGameManager Instance { get; private set; }

    [Header("UI de la pista")]
    [SerializeField] private GameObject hintUIObject; // Pista (objeto completo, se activa/desactiva)
    [SerializeField] private TMP_Text hintText;       // Text (TMP) interno

    [Header("UI Inventory-Pistas")]
    [SerializeField] private Transform hintListContainer; 
    [SerializeField] private GameObject hintTextPrefab; 

    private List<string> unlockedHints = new List<string>();

    public Difficulty CurrentDifficulty { get; private set; }
    private int remainingHints;

    private RoomHintProvider currentRoomHintProvider;
    private Coroutine hintCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

       
        // Obtiene lo guardado (default = 0 si no existe aún)
        string savedDiff = PlayerPrefs.GetString("Difficulty", "0");
        Difficulty diff = (Difficulty)int.Parse(savedDiff);

        // Lo aplica a tu sistema
        SetDifficulty(diff);
    }

    public void SetDifficulty(Difficulty diff= Difficulty.Easy)
    {
        CurrentDifficulty = diff;

        remainingHints = diff switch
        {
            Difficulty.Easy => 10,
            Difficulty.Normal => 5,
            Difficulty.Hard => 2,
            Difficulty.Nightmare => 0,
            _ => 0
        };
    }

    public void RegisterRoomHints(RoomHintProvider provider)
    {
        currentRoomHintProvider = provider;
    }

    public void ShowHint()
    {
        if (remainingHints <= 0)
        {
            DisplayHint("No te quedan pistas disponibles.");
            return;
        }

        if (currentRoomHintProvider == null)
        {
            DisplayHint("No hay pistas disponibles en este momento.");
            return;
        }

        string hint = currentRoomHintProvider.GetCurrentHint();

        if (!string.IsNullOrEmpty(hint))
        {
            remainingHints--;
            DisplayHint(hint);
            AddHint(hint);
        }
        else
        {
          
            DisplayHint("No hay más pistas para este puzzle.");
        }
    }

    public void AddHint(string hintText)
    {
        if (unlockedHints.Contains(hintText)) return; // evitar duplicados

        unlockedHints.Add(hintText);

        GameObject hintGO = Instantiate(hintTextPrefab, hintListContainer);
        var tmp = hintGO.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.text = hintText;
        }
    }
    public void ClearHints()
    {
        foreach (Transform child in hintListContainer)
        {
            if (child.gameObject == hintTextPrefab)
                continue; // NO destruir el HUD de la pista activa
            Destroy(child.gameObject);
        }
        unlockedHints.Clear();
    }
    private void DisplayHint(string message)
    {
        if (hintCoroutine != null)
            StopCoroutine(hintCoroutine);

        hintCoroutine = StartCoroutine(HintDisplayRoutine(message));
    }

    private IEnumerator HintDisplayRoutine(string message)
    {
        hintText.text = message;
        hintUIObject.SetActive(true);

        yield return new WaitForSeconds(6f); // Mostrar 6 segundos

        hintUIObject.SetActive(false);
    }

    public int GetRemainingHints() => remainingHints;
}
