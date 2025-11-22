using UnityEngine;

public class PhotoPiece : MonoBehaviour
{
    [Header("Referencias")]
    public Puzzle2Manager puzzle2Manager;

    [Tooltip("Desactivar el objeto al recogerlo")]
    public bool disableOnCollect = true;

    // Este método lo vas a llamar desde tu sistema de interacción (click/E/etc)
    public void Collect()
    {
        if (puzzle2Manager != null)
            puzzle2Manager.RegisterPieceCollected();

        if (disableOnCollect)
            gameObject.SetActive(false);
    }
}
