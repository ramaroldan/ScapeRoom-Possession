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
    private void Museum()
    {
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(0, nameof(CriptexCodePanel), new List<string>
        {
               "En la nota arriba de la caja encontraras la respuesta",
                "Cuenta la cantidad de cuadros en la pared, alfombras en el suelo y arañas de techo",
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(1, nameof(SlidingPuzzleManager), new List<string>
        {
                "Recoje las fotos para activar el rompecabezas",
                "Las piezas estan en las vitrinas y biblioteca",
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(2, nameof(DoorLockByPadlock), new List<string>
        {
                "Ve al mueble donde esta el televisor, debajo encontraras una carta.",
                "el reloj que esta al lado de la puerta es la clave para abrir el candado.",
        });
        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(3, nameof(RitualBoxManager), new List<string>
        {
                "Necesitas el rosario, la calavera y el reloj.",
                "usa la llave para abrir la puerta de salida.",
        });
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
