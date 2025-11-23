using System.Collections.Generic;
using UnityEngine;

public class PowerBoxCover : MonoBehaviour
{
    private bool hasFallen = false;
    private RoomHintProvider hintProvider;
    public GameObject stichesluz = null;

    private void Start()
    {
        hintProvider = FindObjectOfType<RoomHintProvider>();

        // Registramos las pistas para esta puerta
        hintProvider.RegisterPuzzleHints(1, nameof(PcController), new List<string>
            {
                "Parece que hay una secuencia numérica en esa grabación...",
                "Ese audio no está ahí por casualidad. Presta atención a los detalles.",

            });
    }
    public void DropCover()
    {
        if (hasFallen) return;

        hasFallen = true;

        hintProvider.AdvancePuzzleHint(nameof(PcController));

        stichesluz.GetComponent<EnableInteractables>().EnableItemsInteractables();


        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        rb.AddForce(Vector3.forward * 2f + Vector3.down * 5f, ForceMode.Impulse);

        Collider staticCollider = GetComponent<Collider>();
        if (staticCollider != null)
            staticCollider.enabled = false;
    }
}
