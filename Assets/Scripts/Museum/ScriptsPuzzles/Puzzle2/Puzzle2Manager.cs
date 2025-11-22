using UnityEngine;

public class Puzzle2Manager : MonoBehaviour
{
    [Header("Configuración")]
    public int totalPieces = 8;

    [Header("Referencias")]
    [Tooltip("El InspectPanel del tablero del puzzle 2")]
    public InspectPanel puzzleBoardInspect;   // el script que abre el panel del tablero

    [Tooltip("Texto u objeto que diga 'Faltan piezas' (opcional)")]
    public GameObject boardLockedHint;

    private int collectedPieces = 0;

    private void Start()
    {
        // Al inicio el tablero NO se puede inspeccionar
        if (puzzleBoardInspect != null)
            puzzleBoardInspect.enabled = false;

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
    }

    private void UnlockBoard()
    {
        Debug.Log("Puzzle2: todas las piezas recogidas, se habilita el tablero");

        if (puzzleBoardInspect != null)
            puzzleBoardInspect.enabled = true;

        if (boardLockedHint != null)
            boardLockedHint.SetActive(false);
    }
}
