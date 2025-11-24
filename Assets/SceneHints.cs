using NavKeypad;
using System.Collections.Generic;
using UnityEngine;

public class SceneHints : MonoBehaviour
{
    private RoomHintProvider hintProvider;

    [SerializeField] private string sceneName = "Scene";
    void Start()
    {
        hintProvider = FindObjectOfType<RoomHintProvider>();
        switch (sceneName)
        {
            case "HospitalRoom":
                HospitalRoom();
                break;
            case "Morgue":
                Morgue();
                break;
            case "Museum":
                Museum();
                break;
                // Agregar más escenas y sus pistas aquí si es necesario
        }
    }

    private void HospitalRoom()
    {
    }
    private void Museum()
    {
        
    }
    private void Morgue()
    {   // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(0, nameof(PcController), new List<string>
        {
                "Esos objetos que se repiten en la habitación... ¿no te suenan de los posters?",
                "Contá cuántas veces aparece cada objeto del poster en la sala.",
                "Poné los números en el mismo orden en el que están los posters."
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(1, nameof(PowerBoxCover), new List<string>
            {
                "Parece que hay una secuencia numérica en esa grabación...",
                "Ese audio no está ahí por casualidad. Presta atención a los detalles.",

            });

        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(2, nameof(Keypad), new List<string>
            {
                "Con la linterna mira las paredes",
                "Conta las cosas en la pared",
                "De menor a mayor los numeros que te muestra"
            });
    }

}
