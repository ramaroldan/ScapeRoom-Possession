using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InteractableDripperMain : InteractableBase
{
    [Header("Game Object to interact")]
    public GameObject _object1;
    public GameObject _object2;
    public GameObject _object3;
    //public string _objectBool = "isOpen";

    [Header("Eventos")]
    [Tooltip("Se dispara cuando el item/objeto hace lo que debe (puedes enganchar tu Inventory aquí).")]
    public UnityEvent onItemPlaced;

    protected override IEnumerator DoInteraction()
    {
        Debug.Log(state ? "Abriendo puerta" : "Cerrando puerta");

        if (_object1 != null && _object2!= null && _object3!= null)
            Activate();

        

        if (_object1.activeSelf)
            onItemPlaced?.Invoke();

        // esperar audio
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(.3f);

        canInteract = true;
    }

    public void Activate()
    {
        _object1.SetActive(true);
        _object2.SetActive(true);
        _object3.SetActive(true);
    }
}
