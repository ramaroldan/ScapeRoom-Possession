using UnityEngine;

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

    [Tooltip("Texto u objeto que diga 'Faltan piezas' (opcional)")]
    public GameObject boardLockedHint;

    private int collectedPieces = 0;

    private void Start()
    {
        LockBoard();
    }

    private void LockBoard()
    {
        Debug.Log("Puzzle2: tablero bloqueado al inicio");

        // 1) Deshabilitar el InspectPanel (para que no abra el panel)
        if (puzzleBoardInspect != null)
            puzzleBoardInspect.enabled = false;

        // 2) Deshabilitar el collider para que el raycast no lo detecte
        if (boardCollider != null)
            boardCollider.enabled = false;

        // 3) Mostrar mensaje de "bloqueado", si hay
        if (boardLockedHint != null)
            boardLockedHint.SetActive(true);

        // 4) (Opcional) ocultar completamente el tablero al inicio
        //    Si quieres que se vea, deja esto comentado
        /*
        if (boardRoot != null)
            boardRoot.SetActive(false);
        */
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

        if (boardCollider != null)
            boardCollider.enabled = true;

        if (boardLockedHint != null)
            boardLockedHint.SetActive(false);

        // Si decidiste ocultar el tablero al inicio, aquí lo muestras
        /*
        if (boardRoot != null)
            boardRoot.SetActive(true);
        */
    }
}
