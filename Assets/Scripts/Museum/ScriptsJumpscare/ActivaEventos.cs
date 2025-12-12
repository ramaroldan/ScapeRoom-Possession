using UnityEngine;

public class ActivaEventos : MonoBehaviour
{
   public Animator animatorTerror;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animatorTerror.enabled = true;
            Destroy(gameObject);
        }
    }
}
