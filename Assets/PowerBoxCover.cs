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

       
    }
    public void DropCover()
    {
        if (hasFallen) return;

        hasFallen = true;

        hintProvider.AdvancePuzzleHint(nameof(PowerBoxCover));

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
