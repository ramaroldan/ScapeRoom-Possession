using UnityEngine;

public class EnableInteractables : MonoBehaviour
{
    [SerializeField] private GameObject[] interactablesToEnable;

    public void EnableItemsInteractables()
    {
        foreach (var obj in interactablesToEnable)
        {
            obj.SetActive(true); // ✅ Habilita el objeto entero
        }

        Debug.Log("✅ Interactuables activados");
    }

}
