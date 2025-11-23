using UnityEngine;

public class Puzzle2PanelController : MonoBehaviour
{
    [Header("Quién abre/cierra este panel")]
    [Tooltip("InspectPanel del tablero en la mesa")]
    public InspectPanel boardInspectPanel;

    // Llamar desde el botón "Salir" del puzzle
    public void ExitPuzzle()
    {
        if (boardInspectPanel != null)
        {
            boardInspectPanel.CloseUI();
        }
        else
        {
            // Fallback: simplemente ocultar este panel
            gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
