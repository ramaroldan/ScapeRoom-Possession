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

        // Cambiar el color de la luz a violeta (si tiene componente Light)
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogWarning("❌ No se encontró el objeto 'Player'.");
            return;
        }
        Transform torchLightTransform = player.transform.Find("ToolManager/Linterna/Torch Light");
        GameObject torchLight = torchLightTransform.gameObject;
        Light lightComponent = torchLight.GetComponent<Light>();
        if (lightComponent != null)
        {
            lightComponent.color = new Color(1f, 1f, 1f); // 
            Debug.Log("🎨 Luz cambiada a violeta.");
        }
        else
        {
            Debug.LogWarning("⚠ No se encontró un componente Light en 'Torch Light'.");
        }

        Debug.Log("✅ Interactuables activados");
    }
}
