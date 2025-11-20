using System.Collections;
using UnityEngine;

public class PanelLight : InteractableBase
{
    [Header("Animation")]
    public Animator animator;
    public string animatorBool = "isOn";

    protected override IEnumerator DoInteraction()
    {
        Debug.Log(state ? "Prender Luz" : "Apagar Luz");

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
