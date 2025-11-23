using UnityEngine;
using UnityEngine.Events;

public class SlidingPuzzleManager : MonoBehaviour
{
    [Header("Tamaño del puzzle")]
    public int rows = 3;
    public int cols = 3;

    [Header("Slots (posiciones en pantalla)")]
    [Tooltip("9 slots (RectTransform) donde pueden estar las fichas. Ordenados 0..8")]
    public RectTransform[] slots;   // tamaño = 9

    [Header("Fichas (tiles)")]
    [Tooltip("Fichas del puzzle. En los índices donde no haya ficha, se deja null (hueco).")]
    public SlidingTile[] tiles;     // tamaño = 9, con 1 elemento null para el hueco

    [Header("Resolución del puzzle")]
    [Tooltip("Índice del slot que empieza vacío (0-8)")]
    public int emptySlotIndex = 8;  // por ejemplo, el último

    public UnityEvent onPuzzleSolved;   // jumpscare, activar objetos, etc.

    private bool solved = false;

    private void Start()
    {
        // Inicializar referencias cruzadas y posiciones
        for (int i = 0; i < tiles.Length; i++)
        {
            SlidingTile tile = tiles[i];
            if (tile != null)
            {
                tile.Init(this, i);
                tile.MoveToSlot(slots[i]); // colocarla visualmente en su slot inicial
            }
        }
    }

    public void TryMoveTile(SlidingTile tile)
    {
        if (solved) return;

        int tileIndex = tile.currentSlotIndex;

        if (!IsAdjacent(tileIndex, emptySlotIndex))
            return;

        // Mover visualmente la ficha al slot vacío
        tile.MoveToSlot(slots[emptySlotIndex]);

        // Actualizar mapa lógico
        tiles[emptySlotIndex] = tile;
        tiles[tileIndex] = null;

        int previousEmpty = emptySlotIndex;
        emptySlotIndex = tileIndex;
        tile.currentSlotIndex = previousEmpty;

        CheckSolved();
    }

    private bool IsAdjacent(int indexA, int indexB)
    {
        int rowA = indexA / cols;
        int colA = indexA % cols;

        int rowB = indexB / cols;
        int colB = indexB % cols;

        int rowDiff = Mathf.Abs(rowA - rowB);
        int colDiff = Mathf.Abs(colA - colB);

        // Adyacente si está justo arriba/abajo o izquierda/derecha
        return (rowDiff + colDiff) == 1;
    }

    private void CheckSolved()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            SlidingTile tile = tiles[i];
            if (tile == null)
                continue;

            if (tile.currentSlotIndex != tile.correctSlotIndex)
                return; // aún no está resuelto
        }

        // Si llegamos acá, todas las fichas están en su posición correcta
        solved = true;
        Debug.Log("Puzzle2: ¡rompecabezas resuelto!");

        onPuzzleSolved?.Invoke();
    }
}
