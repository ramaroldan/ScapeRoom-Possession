using UnityEngine;

public class DoorPrimeraSala : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // Asignás el objeto con NextScene y el collider en el Inspector

    public void Start()
    {
        // ✅ Desactivar el script NextScene al inicio
        if (targetObject != null)
        {
            var nextSceneScript = targetObject.GetComponent<NextScene>();
            if (nextSceneScript != null)
                nextSceneScript.enabled = false;
            // ✅ Desactivar el BoxCollider al inicio
            var boxCollider = targetObject.GetComponent<BoxCollider>();
            if (boxCollider != null)
                boxCollider.enabled = false;
        }
        else
        {
            Debug.LogWarning("❗ No se asignó el objeto targetObject en DoorPrimeraSala.");
        }
    }
    public void ArrancarSalas()
    {
        if (targetObject != null)
        {
            // ✅ Activar el script NextScene
            var nextSceneScript = targetObject.GetComponent<NextScene>();
            if (nextSceneScript != null)
                nextSceneScript.enabled = true;

            // ✅ Activar el BoxCollider
            var boxCollider = targetObject.GetComponent<BoxCollider>();
            if (boxCollider != null)
                boxCollider.enabled = true;
        }
        else
        {
            Debug.LogWarning("❗ No se asignó el objeto targetObject en DoorPrimeraSala.");
        }
    }
}
