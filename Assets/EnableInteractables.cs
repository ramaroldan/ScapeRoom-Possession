using UnityEngine;

public class EnableInteractables : MonoBehaviour
{
    [SerializeField] private GameObject[] interactablesToEnable;
    public bool _enabled = false;

    public void EnableItemsInteractables()
    {
        foreach (var obj in interactablesToEnable)
        {
            obj.SetActive(_enabled); // ✅ Habilita el objeto entero
        }

        Debug.Log("✅ Interactuables activados");
    }

}
