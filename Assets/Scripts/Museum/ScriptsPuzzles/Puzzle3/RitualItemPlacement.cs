using UnityEngine;

public class RitualItemPlacement : MonoBehaviour
{
    public string itemID; // debe coincidir con el ID en el manager
    private bool placed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (placed) return;

        RitualBoxManager manager = other.GetComponentInParent<RitualBoxManager>();
        if (manager != null)
        {
            manager.TryPlaceItem(itemID, gameObject);
            placed = true;
        }
    }
}
