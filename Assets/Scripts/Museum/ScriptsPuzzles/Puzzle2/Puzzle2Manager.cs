using UnityEngine;
using TMPro; // 👈 importante si usas TextMeshPro

public class Puzzle2Manager : MonoBehaviour
{
    [Header("Configuración")]
    public int totalPieces = 8;

    [Header("Referencias")]
    [Tooltip("El InspectPanel del tablero del puzzle 2 (el que abre el canvas del puzzle)")]
    public InspectPanel puzzleBoardInspect;

    [Tooltip("Collider que usa el tablero para ser clickeado / interactuado")]
    public Collider boardCollider;

    [Tooltip("Root visual del tablero (solo por si quieres ocultarlo totalmente al inicio)")]
    public GameObject boardRoot;

    [Tooltip("Objeto que contiene el mensaje de 'faltan piezas' (puede ser un panel o un texto)")]
    public GameObject boardLockedHint;

    [Tooltip("Texto donde se muestra cuántas piezas faltan")]
    public TextMeshProUGUI boardHintText;

    private int collectedPieces = 0;

    private void Start()
    {
        LockBoard();
        UpdateHintText(); // mostrar "Faltan X piezas" al inicio
    }

    private void LockBoard()
    {
        Debug.Log("Puzzle2: tablero bloqueado al inicio");

        if (puzzleBoardInspect != null)
            puzzleBoardInspect.enabled = false;

        if (boardCollider != null)
            boardCollider.enabled = false;

        if (boardLockedHint != null)
            boardLockedHint.SetActive(true);
    }

    public void RegisterPieceCollected()
    {
        collectedPieces++;
        Debug.Log($"Puzzle2: pieza recogida ({collectedPieces}/{totalPieces})");

        if (collectedPieces >= totalPieces)
        {
            UnlockBoard();
        }
        else
        {
            UpdateHintText();
        }
    }

    private void UnlockBoard()
    {
        Debug.Log("Puzzle2: todas las piezas recogidas, se habilita el tablero");

        if (puzzleBoardInspect != null)
            puzzleBoardInspect.enabled = true;

        if (boardCollider != null)
            boardCollider.enabled = true;

        if (boardLockedHint != null)
            boardLockedHint.SetActive(false);
    }

    private void UpdateHintText()
    {
        if (boardHintText == null)
            return;

        int remaining = Mathf.Clamp(totalPieces - collectedPieces, 0, totalPieces);

        if (remaining <= 0)
        {
            boardHintText.text = "Ya tienes todas las piezas.";
        }
        else if (remaining == 1)
        {
            boardHintText.text = "Falta 1 pieza para utilizar el tablero.";
        }
        else
        {
            boardHintText.text = $"Faltan {remaining} piezas para utilizar el tablero.";
        }
    }
}
