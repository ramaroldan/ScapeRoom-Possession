using System.Collections;
using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("Highlight")]
    public Renderer highlightRenderer;
    public Color highlightColor = Color.yellow;
    protected Color originalColor;
    protected bool hovering = false;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip[] clips;

    [Header("Prompts")]
    public string[] prompts = { "Interact", "" };

    protected bool canInteract = true;
    protected bool state = false; // true=open/active, false=closed/inactive

    protected Interact interactScript;
    protected PlayerMovement playerScript;
    protected MouseLook[] lookScripts;
    protected MouseLook mouseLookPlayer;
    protected MouseLook mouseLookCamera;
    protected Camera cam;

    // ---------------------------------------------------
    protected virtual void Start()
    {
        StartCoroutine(FindPlayerReferences());

        cam = Camera.main;
        if (cam == null)
            cam = FindFirstObjectByType<Camera>();

        interactScript = cam.GetComponent<Interact>();

        playerScript = FindFirstObjectByType<PlayerMovement>();
        lookScripts = FindObjectsOfType<MouseLook>(true);

        if (highlightRenderer != null)
            originalColor = highlightRenderer.material.color;
    }

    // ---------------------------------------------------
    //void Update()
    //{
    //    if (!hovering && interactScript != null && interactScript.message != "")
    //    {
    //        interactScript.message = "";
    //    }
    //}

    public virtual void Hovering()
    {
        hovering = true;
        StartCoroutine(HoverFade());

        string rawText = state ? LocalizationManager.Instance.GetText(prompts[1]) : LocalizationManager.Instance.GetText(prompts[0]);

        // Si el texto es nulo, vacío o tiene el formato de key no encontrada
        if (string.IsNullOrWhiteSpace(rawText) || rawText.StartsWith("["))
            interactScript.message = ""; // o null si preferís
        else
            interactScript.message = rawText;

        // interactScript.message = state ? LocalizationManager.Instance.GetText(prompts[1]): LocalizationManager.Instance.GetText(prompts[0]);
    }

    IEnumerator HoverFade()
    {
        yield return new WaitForSeconds(1);
        hovering = false;
    }

    // ---------------------------------------------------
    public void Interacting()
    {
        if (!canInteract) return;

        PlayAudio();

        state = !state;
        StartCoroutine(DoInteraction());
        canInteract = false;
    }

    // ---------------------------------------------------
    void FixedUpdate()
    {
        if (highlightRenderer == null) return;

        if (hovering)
            highlightRenderer.material.color =
                Color.Lerp(highlightRenderer.material.color, highlightColor, Time.deltaTime * 4);
        else
            highlightRenderer.material.color =
                Color.Lerp(highlightRenderer.material.color, originalColor, Time.deltaTime * 2);
    }

    // ---------------------------------------------------
    void PlayAudio()
    {
        if (source != null && clips.Length > 0)
        {
            source.pitch = Random.Range(.9f, 1.3f);
            source.clip = state ? clips[1] : clips[0];
            source.Play();
        }
    }

    // ---------------------------------------------------
    // 🔥 CADA HIJO IMPLEMENTA SU ACCIÓN PARTICULAR
    protected abstract IEnumerator DoInteraction();

    // ---------------------------------------------------
    protected IEnumerator FindPlayerReferences()
    {
        while ((playerScript = FindFirstObjectByType<PlayerMovement>()) == null)
            yield return null;

        while ((cam = Camera.main) == null)
            yield return null;

        MouseLook[] allLooks = FindObjectsOfType<MouseLook>(true);

        foreach (var ml in allLooks)
        {
            if (ml.CompareTag("Player"))
                mouseLookPlayer = ml;
            if (ml.CompareTag("MainCamera"))
                mouseLookCamera = ml;
        }

        interactScript = cam.GetComponent<Interact>();
        lookScripts = allLooks;
    }
}
