using System.Collections.Generic;
using UnityEngine;

public class DripperMainHint : MonoBehaviour
{
    private RoomHintProvider hintProvider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hintProvider = FindObjectOfType<RoomHintProvider>();

        // Registramos las pistas para esta puerta
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceHint()
    {
        hintProvider.AdvancePuzzleHint(nameof(DripperMainHint));
    }
}
