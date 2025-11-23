using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InteractablePatient : InteractableBase
{
    [Header("Game Object to interact")]
    public GameObject _object;
    //public string _objectBool = "isOpen";

    [Header("Eventos")]
    [Tooltip("Se dispara cuando el item/objeto hace lo que debe (puedes enganchar tu Inventory aquí).")]
    public UnityEvent onItemPlaced;

    protected override IEnumerator DoInteraction()
    {
        Debug.Log(state ? "Abriendo puerta" : "Cerrando puerta");

        if (_object != null)
            //Activate();

            if (_object.activeSelf)
                onItemPlaced?.Invoke();

        // esperar audio
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(.3f);

        canInteract = true;
    }
}
