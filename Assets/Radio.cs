using System.Collections;
using UnityEngine;

public class Radio : InteractableBase
{
    [Header("Animation")]
    public AudioSource audio;    

    protected override IEnumerator DoInteraction()
    {
        Debug.Log(state ? "Abriendo puerta" : "Cerrando puerta");

        if (audio != null)
        {
            if (!state) {
                audio.Play(); // Play sound when turning off
                yield return new WaitForSeconds(.3f);
            }
            if (state)
            {
                audio.Stop();
                yield return new WaitForSeconds(.3f);
            }
            


           
                
        }           

        canInteract = true;
    }
}
