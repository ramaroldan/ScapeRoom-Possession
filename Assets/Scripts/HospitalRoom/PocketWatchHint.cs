using System.Collections.Generic;
using UnityEngine;

public class PocketWatchHint : MonoBehaviour
{
    private RoomHintProvider hintProvider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hintProvider = FindObjectOfType<RoomHintProvider>();

        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(0, nameof(PocketWatchHint), new List<string>
        {
                "La salud del paciente se deteriora a cada hora. Ya no puede ni levantarse para ir al baño.",
                "Mejor que el paciente no se haya olvidado nada, o el enfermero se lo quedará.",
                "¿Se habrán olvidado algo en el baño?"
        });

        hintProvider.RegisterPuzzleHints(1, nameof(TvScreenHint), new List<string>
        {
                "Ese maldito ruido de estática no te deja concentrar.",
                "Si hubiera una forma de silenciarlo..."
        });

        hintProvider.RegisterPuzzleHints(2, nameof(DrawerLockHint), new List<string>
        {
                "Tiene que haber alguna información sobre el paciente... Pero ¿Donde está?",
                "Debes revisarlo todo!"
        });

        hintProvider.RegisterPuzzleHints(3, nameof(DripperMainHint), new List<string>
        {
                "¿Dónde te podría servir el suero?",
                "Algún lugar donde colocar ese suero..."

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceHint()
    {
        hintProvider.AdvancePuzzleHint(nameof(PocketWatchHint));
    }
}
