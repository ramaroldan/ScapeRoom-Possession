using System.Collections.Generic;
using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class RoomHintProvider : MonoBehaviour
{
    [System.Serializable]
    public class HintBlock
    {
        public string id;
        public List<string> hints;
        public int progress = 0;

        public bool IsComplete => progress >= hints.Count;
        public string GetCurrentHint()
        {
            if (IsComplete) return null;
            return hints[progress];
        }

        public void Advance()
        {
            if (!IsComplete) progress++;
        }
    }

    private List<HintBlock> orderedHints = new();
    private int currentPuzzleIndex = 0;

    private void Start()
    {
        HintGameManager.Instance.RegisterRoomHints(this);
    }

    // El puzzle se registra con sus pistas en orden
    public void RegisterPuzzleHints(int orderIndex, string id, List<string> hints)
    {
        HintBlock block = new HintBlock
        {
            id = id,
            hints = hints
        };

        // Insertamos en el lugar correcto o al final si no existe aún
        if (orderIndex < orderedHints.Count)
            orderedHints.Insert(orderIndex, block);
        else
            orderedHints.Add(block);
    }


    public void AdvancePuzzleHint(string id)
    {
        if (currentPuzzleIndex >= orderedHints.Count)
            return;

        var current = orderedHints[currentPuzzleIndex];

        if (current.id != id)
        {
            Debug.LogWarning($"⛔ El puzzle '{id}' no es el actual. Se espera: '{current.id}'");
            return;
        }

        Debug.Log($"✅ Puzzle '{id}' completado. Avanzando al siguiente.");
        currentPuzzleIndex++;
       
    }


    public string GetCurrentHint()
    {
        var current = orderedHints[currentPuzzleIndex];

        // Si el puzzle ya fue completado, pasar automáticamente al siguiente
        if (current.IsComplete)
        {
            currentPuzzleIndex++;
           

            current = orderedHints[currentPuzzleIndex];
        }

        int randomIndex = Random.Range(0, current.hints.Count);
        return current.hints[randomIndex];
    }



}
