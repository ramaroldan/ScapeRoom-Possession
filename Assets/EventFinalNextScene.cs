using UnityEngine;

public class EventFinalNextScene : MonoBehaviour
{
    [SerializeField] private GameObject[] interactablesToEnable;
    [Header("Animation")]
    public Animator animator;
    public string animatorBool = "isOpen";

    public AudioSource audioSource;
   
    public void EnableItemsInteractables()
    {
        foreach (var obj in interactablesToEnable)
        {
            obj.SetActive(true); // ✅ Habilita el objeto entero
        }

        if (animator != null)
            animator.SetBool(animatorBool, true);

        if (audioSource != null )   
            audioSource.Play();
           

        Debug.Log("✅ Interactuables activados");
    }
}
