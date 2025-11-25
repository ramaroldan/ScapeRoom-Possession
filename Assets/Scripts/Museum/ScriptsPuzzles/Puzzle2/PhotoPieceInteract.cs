using System.Collections;
using UnityEngine;

public class PhotoPieceInteract : InteractableBase
{
    [Header("Referencia a la pieza de foto")]
    public PhotoPiece photoPiece;

    [Header("Opcional")]
    public Renderer[] renderersToHide;   // por si querés ocultar solo el mesh
    public Collider[] collidersToDisable;

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator DoInteraction()
    {
        // Evitar doble interacción
        if (!canInteract)
            yield break;

        canInteract = false;

        // 1) Avisar al manager y desactivar la pieza
        if (photoPiece != null)
            photoPiece.Collect();

        // 2) Opcional: ocultar renderers y colliders específicos
        foreach (var r in renderersToHide)
            if (r != null) r.enabled = false;

        foreach (var c in collidersToDisable)
            if (c != null) c.enabled = false;

        // 3) Reproducir audio si hay (usa el AudioSource de InteractableBase)
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(0.1f);

        // 4) Destruir el interactable para que no se vuelva a usar
        Destroy(gameObject);
    }
}
