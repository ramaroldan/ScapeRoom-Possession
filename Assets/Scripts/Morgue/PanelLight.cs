using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLight : InteractableBase
{
    [Header("Animation")]
    public Animator animator;
    public string animatorBool = "isOn";

    [Header("Lights to Toggle")]
    public Light[] lightsToControl;     // 🚀 Lista de luces reales
    public GameObject[] emissiveObjects; // 🚀 (Opcional) Objetos con material emisivo

   

    protected override IEnumerator DoInteraction()
    {
        Debug.Log(state ? "🔆 Prender Luces" : "🌑 Apagar Luces");

        // ---------------------
        // 1) ANIMACIÓN
        // ---------------------
        if (animator != null)
            animator.SetBool(animatorBool, state);

        // ---------------------
        // 2) LUCES REALES (Light)
        // ---------------------
        if (lightsToControl != null && lightsToControl.Length > 0)
        {
            foreach (Light l in lightsToControl)
            {
                if (l != null)
                    l.enabled = state;
            }
        }       

        // ---------------------
        // 3) OBJETOS EMISIVOS (si querés usarlos)
        // ---------------------
        if (emissiveObjects != null && emissiveObjects.Length > 0)
        {
            foreach (GameObject obj in emissiveObjects)
            {
                if (obj != null)
                {
                    var rend = obj.GetComponent<Renderer>();
                    if (rend != null && rend.material.HasProperty("_EmissionColor"))
                    {
                        if (state)
                            rend.material.SetColor("_EmissionColor", Color.white * 1.5f);
                        else
                            rend.material.SetColor("_EmissionColor", Color.black);
                    }
                }
            }
        }

        // ---------------------
        // 4) ESPERAR SONIDO
        // ---------------------
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(.3f);

        canInteract = true;
    }
}