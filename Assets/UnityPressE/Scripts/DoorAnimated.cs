using System.Collections;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class DoorAnimated : MonoBehaviour
{
    [Header("Interaction")]
    private bool showUI;
    private bool canInteract = true;

    [Header("Audio")]
    [Tooltip("0 = open UI, 1 = close UI")]
    public AudioClip[] clips;
    public AudioSource source;

    [Header("Mesh Highlight")]
    public Renderer pageRend;
    public Color targetColor = Color.yellow;
    private Color originColor;

    private bool over = false;

    [Header("Prompt Message (Interact HUD)")]
    public string[] prompts = { "Open Door", " Close Door" };

    [Header("Animation")]
    public Animator animator;
    public string animatorBool = "isOpen";


    private Interact interactScript;
    private PlayerMovement playerScript;
    private MouseLook[] lookScripts;

    private Camera cam;

    [SerializeField] private MouseLook mouseLookPlayer;
    [SerializeField] private MouseLook mouseLookCamera;

    void Start()
    {
        StartCoroutine(FindPlayerReferences());
        // detectar cámara correctamente (compatible con DontDestroyOnLoad)
        cam = Camera.main;
        if (cam == null)
        {
            cam = FindFirstObjectByType<Camera>();
        }

        interactScript = cam.GetComponent<Interact>();

        playerScript = FindFirstObjectByType<PlayerMovement>();
        lookScripts = FindObjectsOfType<MouseLook>();

        if (pageRend != null)
            originColor = pageRend.material.color;

        
    }

    // ----------------------------------------------------------------------

    public void Hovering()
    {
        over = true;
        StartCoroutine(Fadeout());

        if (!showUI)
            interactScript.message = prompts[0];
        else
            interactScript.message = prompts[1];
    }

    // ----------------------------------------------------------------------

    //public void Interacting()
    //{
    //    if (!canInteract) return;

    //    if (source != null)
    //    {
    //        source.pitch = Random.Range(.9f, 1.3f);
    //        source.clip = clips.Length > 0 ? clips[showUI ? 1 : 0] : null;
    //        source.Play();
    //    }

    //    StartCoroutine(ToggleUI());
    //    showUI = !showUI;
    //    canInteract = false;
    //}
    public void Interacting()
    {
        if (!canInteract) return;

        // Estado actual antes de cambiar
        bool doorIsOpen = showUI;

        // Elegir clip según ESTADO ACTUAL
        if (source != null && clips.Length > 0)
        {
            source.pitch = Random.Range(.9f, 1.3f);

            // 0 = open, 1 = close
            source.clip = clips[doorIsOpen ? 1 : 0];
            source.Play();
        }

        // Cambiamos el estado de la puerta
        showUI = !showUI;

        // Ahora sí, ejecutamos la corrutina con el NUEVO estado
        StartCoroutine(ToggleUI());

        canInteract = false;
    }

    // ----------------------------------------------------------------------

    void FixedUpdate()
    {
        if (pageRend == null) return;

        if (over)
        {
            pageRend.material.color = Color.Lerp(pageRend.material.color, targetColor, Time.deltaTime * 4);
        }
        else
        {
            pageRend.material.color = Color.Lerp(pageRend.material.color, originColor, Time.deltaTime * 2);
        }
    }

    IEnumerator Fadeout()
    {
        yield return new WaitForSeconds(1);
        over = false;
    }

    // ----------------------------------------------------------------------

    //IEnumerator ToggleUI()
    //{
    //    if (showUI)
    //    {
    //        Debug.Log("Abrir puerta");
    //        if (animator != null)
    //            animator.SetBool(animatorBool, true);
    //    }
    //    else
    //    {
    //        Debug.Log("Cierra puerta");
    //        if (animator != null)
    //            animator.SetBool(animatorBool, false);

    //    }

    //    // tiempo de audio
    //    if (source != null && source.clip != null)
    //        yield return new WaitForSeconds(source.clip.length);
    //    else
    //        yield return new WaitForSeconds(.3f);

    //    canInteract = true;
    //}

    IEnumerator ToggleUI()
    {
        if (showUI)
        {
            Debug.Log("Abrir puerta");
            if (animator != null)
                animator.SetBool(animatorBool, true);
        }
        else
        {
            Debug.Log("Cierra puerta");
            if (animator != null)
                animator.SetBool(animatorBool, false);
        }

        // tiempo de audio
        if (source != null && source.clip != null)
            yield return new WaitForSeconds(source.clip.length);
        else
            yield return new WaitForSeconds(.3f);

        canInteract = true;
    }


    private IEnumerator FindPlayerReferences()
    {
        // Espera hasta que exista el PlayerMovement
        while ((playerScript = FindFirstObjectByType<PlayerMovement>()) == null)
            yield return null;

        // Espera hasta que exista la main camera correcta
        while ((cam = Camera.main) == null)
            yield return null;

        // Busca MouseLook del jugador y cámara
        MouseLook[] allLooks = FindObjectsOfType<MouseLook>(true);

        foreach (var ml in allLooks)
        {
            if (ml.gameObject.CompareTag("Player"))
                mouseLookPlayer = ml;

            if (ml.gameObject.CompareTag("MainCamera"))
                mouseLookCamera = ml;
        }

        // Busca el script Interact
        interactScript = cam.GetComponent<Interact>();

        // Guarda todos los looks
        lookScripts = allLooks;

        Debug.Log("✅ Referencias del Player encontradas correctamente por InspectPanel.");
    }

}
