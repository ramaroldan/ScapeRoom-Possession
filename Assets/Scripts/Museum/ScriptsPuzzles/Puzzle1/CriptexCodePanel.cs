using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CriptexCodePanel : MonoBehaviour
{
    private RoomHintProvider hintProvider;

    [Header("Configuración del código")]
    [Tooltip("Código correcto que debe ingresar el jugador (3 cifras)")]
    public string correctCode = "533";

    [Header("UI")]
    [Tooltip("InputField donde el jugador escribe el código")]
    public TMP_InputField codeInputField;

    [Header("Flujo")]
    [Tooltip("Root del puzzle de las fotos (debe estar desactivado al inicio)")]
    public GameObject puzzleFotosRoot;

    [Tooltip("Script InspectPanel del Criptex que abre/cierra este panel")]
    public InspectPanel inspectPanel;

    [Header("Piezas de foto que se activan al resolver")]
    [Tooltip("Fotos repartidas por el escenario que deben aparecer tras resolver el Criptex")]
    public GameObject[] fotoPieces;

    [Header("Eventos")]
    [Tooltip("Eventos que se disparan cuando el código es correcto (ej: jumpscare)")]
    public UnityEvent onCorrectCode;

    [Tooltip("Eventos cuando el código es incorrecto (sonido error, etc.)")]
    public UnityEvent onWrongCode;

    private bool solved = false;

    private void OnEnable()
    {
        // Cada vez que se abre el panel, limpiamos el input
        if (codeInputField != null)
        {
            codeInputField.text = "";
            codeInputField.Select();
            codeInputField.ActivateInputField();
        }
    }

    private void Start()
    {
        hintProvider = FindObjectOfType<RoomHintProvider>();

        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(0, nameof(PcController), new List<string>
        {
                "Cuenta la cantidad de cuadros en la pared",
                "Cuenta la cantidad de alfombras en el suelo.",
                "Cuenta la cantidad de arañas de techo."
        });
    }

    /// <summary>
    /// Llamar desde el botón OK del canvas.
    /// </summary>
    public void ConfirmCode()
    {
        if (solved) return; // si ya se resolvió, no hacer nada

        string input = codeInputField != null ? codeInputField.text : "";

        if (input == correctCode)
        {
            hintProvider.AdvancePuzzleHint(nameof(PcController));
            solved = true;
            Debug.Log("Criptex: código correcto");

            // Activar tablero del puzzle 2
            if (puzzleFotosRoot != null)
                puzzleFotosRoot.SetActive(true);

            // Activar las fotos repartidas por el escenario
            if (fotoPieces != null)
            {
                foreach (var go in fotoPieces)
                {
                    if (go != null)
                        go.SetActive(true);
                }
            }

            // Eventos extra (jumpscare, sonidos, luces, etc.)
            onCorrectCode?.Invoke();

            // Cerrar panel usando tu flujo de InspectPanel y devolver control al player
            if (inspectPanel != null)
                inspectPanel.CloseUI();
        }
        else
        {
            Debug.Log("Criptex: código incorrecto");

            // Eventos de fallo (sonido error, vibración leve, etc.)
            onWrongCode?.Invoke();

            // Limpiar el campo para reintentar
            if (codeInputField != null)
            {
                codeInputField.text = "";
                codeInputField.Select();
                codeInputField.ActivateInputField();
            }
        }
    }

    /// <summary>
    /// Llamar desde el botón SALIR del canvas.
    /// No resuelve el puzzle, solo cierra la UI y devuelve control al jugador.
    /// </summary>
    public void Cancel()
    {
        if (inspectPanel != null)
            inspectPanel.CloseUI();
    }
}
