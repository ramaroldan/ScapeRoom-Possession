using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SlidingPuzzleManager : MonoBehaviour
{
    private RoomHintProvider hintProvider;

    [Header("Tamaño del puzzle")]
    public int rows = 3;
    public int cols = 3;

    [Header("Slots (posiciones UI)")]
    [Tooltip("9 slots RectTransform ordenados en el orden 0..8")]
    public RectTransform[] slots;   // tamaño = 9

    [Header("Fichas")]
    [Tooltip("Las 8 fichas, en orden del 0 al 7 (su posición correcta final)")]
    public SlidingTile[] allTiles;  // tamaño = 8

    // mapa lógico actual del puzzle (slots = 9 posiciones; 1 estará vacío)
    [HideInInspector] public SlidingTile[] tiles;

    [HideInInspector] public int emptySlotIndex = 8;

    [Header("Eventos al resolver")]
    public UnityEvent onPuzzleSolved;

    private bool solved = false;

    [Header("Debug")]
    [Tooltip("Permite usar la tecla secreta para resolver el puzzle")]
    public bool enableDebugHotkey = true;

    [Tooltip("La tecla secreta para resolver instantáneamente")]
    public KeyCode debugSolveKey = KeyCode.F10;

    private void Start()
    {
        // preparar estructura lógica
        tiles = new SlidingTile[rows * cols];

        // generar layout aleatorio resolvible
        int[] layout = GenerateSolvableLayout();

        // asignar fichas a slots según el layout
        for (int slotIndex = 0; slotIndex < layout.Length; slotIndex++)
        {
            int tileIndex = layout[slotIndex];

            if (tileIndex == -1)
            {
                tiles[slotIndex] = null;
                emptySlotIndex = slotIndex;
            }
            else
            {
                SlidingTile tile = allTiles[tileIndex];
                tiles[slotIndex] = tile;

                tile.Init(this, slotIndex);
                tile.MoveToSlot(slots[slotIndex]);
            }
        }

        hintProvider = FindObjectOfType<RoomHintProvider>();

        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(1, nameof(PcController), new List<string>
        {
                "Recoje las fotos para activar el rompecabezas",
                "Las piezas estan en las vitrinas y biblioteca",
        });
    }

    private void Update()
    {
        if (enableDebugHotkey && !solved)
        {
            if (Input.GetKeyDown(debugSolveKey))
            {
                SolveInstantly();
            }
        }
    }

    // -----------------------------------------------
    // 🔹 Intentar mover una ficha al hueco
    // -----------------------------------------------
    public void TryMoveTile(SlidingTile tile)
    {
        if (solved) return;

        int tileIndex = tile.currentSlotIndex;

        if (!IsAdjacent(tileIndex, emptySlotIndex))
            return;

        // mover visualmente
        tile.MoveToSlot(slots[emptySlotIndex]);

        // actualizar estado lógico
        tiles[emptySlotIndex] = tile;
        tiles[tileIndex] = null;

        // intercambiar índices
        int previousEmpty = emptySlotIndex;
        emptySlotIndex = tileIndex;
        tile.currentSlotIndex = previousEmpty;

        CheckSolved();
    }

    // -----------------------------------------------
    // 🔹 Revisar si dos slots son adyacentes (arriba/abajo/izq/der)
    // -----------------------------------------------
    private bool IsAdjacent(int indexA, int indexB)
    {
        int rowA = indexA / cols;
        int colA = indexA % cols;

        int rowB = indexB / cols;
        int colB = indexB % cols;

        int rowDiff = Mathf.Abs(rowA - rowB);
        int colDiff = Mathf.Abs(colA - colB);

        return (rowDiff + colDiff) == 1;
    }

    // -----------------------------------------------
    // 🔹 Revisar si el puzzle está resuelto
    // -----------------------------------------------
    private void CheckSolved()
    {
        for (int slotIndex = 0; slotIndex < tiles.Length; slotIndex++)
        {
            SlidingTile tile = tiles[slotIndex];
            if (tile == null) continue;

            if (tile.currentSlotIndex != tile.correctSlotIndex)
                return;
        }
        hintProvider.AdvancePuzzleHint(nameof(PcController));
        solved = true;
        Debug.Log("Puzzle2: ¡RESUELTO!");
        onPuzzleSolved?.Invoke();
    }

    // -----------------------------------------------
    // 🟪 Generar un layout aleatorio pero resolvible
    // -----------------------------------------------
    private int[] GenerateSolvableLayout()
    {
        int[] arr = new int[rows * cols];

        // cargar fichas 0..7 y hueco -1
        for (int i = 0; i < arr.Length; i++)
        {
            if (i < allTiles.Length)
                arr[i] = i;
            else
                arr[i] = -1; // hueco
        }

        // mezclar hasta que sea solvible y no esté resuelto
        do
        {
            Shuffle(arr);
        }
        while (!IsSolvable(arr) || IsTriviallySolved(arr));

        return arr;
    }

    // Fisher–Yates shuffle
    private void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    // solvible si el número de inversiones es PAR (para puzzle 3x3)
    private bool IsSolvable(int[] array)
    {
        int[] flat = new int[array.Length];
        int idx = 0;

        // copiar solo fichas, ignorar hueco
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != -1)
            {
                flat[idx++] = array[i];
            }
        }

        int inversions = 0;
        for (int i = 0; i < idx; i++)
        {
            for (int j = i + 1; j < idx; j++)
            {
                if (flat[i] > flat[j])
                    inversions++;
            }
        }

        return (inversions % 2) == 0;
    }

    // evitar puzzle ya resuelto
    private bool IsTriviallySolved(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int expected = (i < allTiles.Length) ? i : -1;
            if (array[i] != expected)
                return false;
        }
        return true;
    }

    // -----------------------------------------------
    // 🟩 Resolver el puzzle automáticamente (tecla secreta o botón)
    // -----------------------------------------------
    public void SolveInstantly()
    {
        if (solved) return;

        Debug.Log("Puzzle2: RESUELTO con hotkey (DEBUG)");

        for (int i = 0; i < allTiles.Length; i++)
        {
            SlidingTile tile = allTiles[i];
            int correctIndex = tile.correctSlotIndex;

            tile.MoveToSlot(slots[correctIndex]);
            tiles[correctIndex] = tile;
            tile.currentSlotIndex = correctIndex;
        }

        // localizar hueco
        for (int i = 0; i < tiles.Length; i++)
            if (tiles[i] == null)
                emptySlotIndex = i;

        solved = true;
        onPuzzleSolved?.Invoke();
    }
}
