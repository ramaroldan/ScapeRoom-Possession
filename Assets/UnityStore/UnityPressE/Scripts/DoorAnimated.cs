using System.Collections;
using UnityEngine;

public class DoorAnimated : InteractableBase
{
    [Header("Animation")]
    public Animator animator;
    public string animatorBool = "isOpen";

    protected override IEnumerator DoInteraction()
    {
        Debug.Log(state ? "Abriendo puerta" : "Cerrando puerta");

        if (animator != null)
            animator.SetBool(animatorBool, state);

        // esperar audio
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(.3f);

        canInteract = true;
    }
}
