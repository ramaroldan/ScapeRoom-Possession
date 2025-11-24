using System.Collections;
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
        int savedDiff = PlayerPrefs.GetInt("Difficulty", 0);
        Difficulty diff = (Difficulty)savedDiff;

        // Lo aplica a tu sistema
        SetDifficulty(diff);
    }

    public void SetDifficulty(Difficulty diff= Difficulty.Easy)
    {
        CurrentDifficulty = diff;

        remainingHints = diff switch
        {
            Difficulty.Easy => 20,
            Difficulty.Normal => 4,
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
        }
        else
        {
            DisplayHint("No hay más pistas para este puzzle.");
        }
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
