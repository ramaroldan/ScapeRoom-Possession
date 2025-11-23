using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PanelLight : InteractableBase
{
    [Header("Animation")]
    public Animator animator;
    public string animatorBool = "isOn";

    [Header("Lights to Toggle")]
    public Light[] lightsToControl;     // 🚀 Lista de luces reales
    public GameObject[] emissiveObjects; // 🚀 (Opcional) Objetos con material emisivo

    public bool apagartodo = true; // true = encender, false = apagar
    public GameObject itemsUV = null;



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
                    l.enabled = apagartodo ? false : state;
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
                        if (apagartodo ? false : state)
                            rend.material.SetColor("_EmissionColor", Color.white * 1.5f);
                        else
                            rend.material.SetColor("_EmissionColor", Color.black);
                    }
                }
            }
        }
        if(apagartodo)
        {
            // Activar linterna violeta al encender
            Debug.Log("🔦 Activando linterna violeta.");
            ActivarLinternaVioleta();
            itemsUV.GetComponent<EnableInteractables>().EnableItemsInteractables();
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

    void ActivarLinternaVioleta()
    {
        // Buscar el objeto Player (DontDestroyOnLoad)
        GameObject player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogWarning("❌ No se encontró el objeto 'Player'.");
            return;
        }

        // Buscar la linterna dentro de la jerarquía
        Transform torchLightTransform = player.transform.Find("ToolManager/Linterna/Torch Light");

        if (torchLightTransform == null)
        {
            Debug.LogWarning("❌ No se encontró 'Torch Light' en la jerarquía del jugador.");
            return;
        }

        GameObject torchLight = torchLightTransform.gameObject;

        // Activar el objeto si está desactivado
        if (!torchLight.activeSelf)
        {
            torchLight.SetActive(true);
            Debug.Log("🔦 Linterna activada.");
        }

        // Cambiar el color de la luz a violeta (si tiene componente Light)
        Light lightComponent = torchLight.GetComponent<Light>();
        if (lightComponent != null)
        {
            lightComponent.color = new Color(0.6f, 0.2f, 0.8f); // Violeta
            Debug.Log("🎨 Luz cambiada a violeta.");
        }
        else
        {
            Debug.LogWarning("⚠ No se encontró un componente Light en 'Torch Light'.");
        }
    }
}