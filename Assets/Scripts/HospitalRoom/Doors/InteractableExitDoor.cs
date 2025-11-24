using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static Unity.VisualScripting.Member;


public class InteractableExitDoor : InteractableBase
{
    [Header("Game Object to interact")]
    public GameObject _object1;
    
    //public string _objectBool = "isOpen";

    [Header("Eventos")]
    [Tooltip("Se dispara cuando el item/objeto hace lo que debe (puedes enganchar tu Inventory aquí).")]
    public UnityEvent onItemUsed;

    protected override IEnumerator DoInteraction()
    {
        Debug.Log(state ? "Abriendo puerta" : "Cerrando puerta");

        if (_object1 != null && _object1.activeSelf)
            onItemUsed?.Invoke();
            //Activate();



        //if (_object1.activeSelf)


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
        
    }
}
