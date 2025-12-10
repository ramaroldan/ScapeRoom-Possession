using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public GameObject CanvaPanelJumpscare;
    public float TimeToDestroy = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvaPanelJumpscare.SetActive(true);
            Destroy(gameObject, TimeToDestroy);
            Destroy(CanvaPanelJumpscare, TimeToDestroy);
        }
    }
}
