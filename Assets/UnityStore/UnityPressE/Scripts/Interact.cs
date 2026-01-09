using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 

    First Person Interaction Toolkit by Steven Harmon stevenharmongames.com
    Licensed under the MPL 2.0. https://www.mozilla.org/en-US/MPL/2.0/FAQ/
    Please use in your walking sims/horror/adventure/puzzle games! Drop me a line and share what make with it! :)    

 */
public class Interact : MonoBehaviour
{
    public static Interact Instance;
    private Vector3 fwd;
    [HideInInspector]
    public bool hover = false;
    private bool alreadyHovered = false;
    private bool alreadyHovered2 = false;
    [Header("General Interaction Variables")]
    public GameObject InteractionUI;
    [Tooltip("UI de retícula por defecto (visible siempre)")]
    public GameObject defaultCrosshair;
    [Tooltip("UI de retícula cuando se apunta a un objeto (más grande / otro color)")]
    public GameObject hoverCrosshair; // si no se asigna, se usa CrosshairUI para compatibilidad
    [Header("Compatibilidad")]
    public GameObject CrosshairUI; // legacy - se usará como hoverCrosshair si éste no está asignado
    private Animation anim;
    private Text dispText;
    private float dist = 1000;
    [System.NonSerialized]
    public string message = "";

    [System.NonSerialized]
    public GameObject currentObj = null;
    private GameObject storedIntObj;

    [Tooltip("Si activas esto se mostrará el cursor del sistema (puede interferir con el control de cámara)")]
    public bool useSystemCursor = false;

    [Header("Tamaños de retícula (Inspector)")]
    [Tooltip("Tamaño en píxeles para UI (RectTransform). Si el objeto no tiene RectTransform se interpretan como escala local X/Y.")]
    public Vector2 defaultCrosshairSize = new Vector2(32, 32);
    [Tooltip("Tamaño en píxeles para UI (RectTransform). Si el objeto no tiene RectTransform se interpretan como escala local X/Y.")]
    public Vector2 hoverCrosshairSize = new Vector2(48, 48);

    private void Awake()
    {
        // Singleton para evitar duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        // compatibilidad: si no se asignó hoverCrosshair, usar CrosshairUI
        if (hoverCrosshair == null && CrosshairUI != null)
            hoverCrosshair = CrosshairUI;

        anim = InteractionUI.GetComponent<Animation>();
        dispText = InteractionUI.GetComponent<Text>();
        dispText.text = "";

        // Configurar retículas iniciales
        if (defaultCrosshair != null)
        {
            defaultCrosshair.SetActive(true);
            ApplyCrosshairSize(defaultCrosshair, defaultCrosshairSize);
        }

        if (hoverCrosshair != null)
        {
            hoverCrosshair.SetActive(false);
            ApplyCrosshairSize(hoverCrosshair, hoverCrosshairSize);
        }

        // Opcional: mostrar cursor del sistema (advertencia: puede desactivar el control tipo FPS)
        if (useSystemCursor)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Aplica tamaño a la retícula. Si hay RectTransform, ajusta sizeDelta (UI).
    // Si no, aplica localScale con los valores X/Y (mantiene Z en 1).
    private void ApplyCrosshairSize(GameObject go, Vector2 size)
    {
        if (go == null) return;

        var rt = go.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.sizeDelta = size;
            return;
        }

        // Si no es UI, interpretamos Vector2 como escala relativa.
        go.transform.localScale = new Vector3(size.x, size.y, go.transform.localScale.z != 0 ? go.transform.localScale.z : 1f);
    }

    // Update is called once per frame
    void Update()
    {
        fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, 100))
        {
            currentObj = hit.collider.gameObject;
            if (currentObj.tag == "Interactable")
            {
                storedIntObj = currentObj;
                dist = Vector3.Distance(hit.transform.position, this.transform.position);
                if (dist < 3)
                {
                    storedIntObj.transform.SendMessage("Hovering", hit.point, SendMessageOptions.DontRequireReceiver);
                    dispText.text = message;
                    if (!alreadyHovered)
                    {
                        anim.Play("An_InteractTextPopup");
                        // Mostrar retícula de hover y ocultar la por defecto si existen
                        if (hoverCrosshair != null)
                        {
                            hoverCrosshair.SetActive(true);
                            ApplyCrosshairSize(hoverCrosshair, hoverCrosshairSize);
                            if (defaultCrosshair != null) defaultCrosshair.SetActive(false);
                        }
                        else if (CrosshairUI != null)
                        {
                            CrosshairUI.SetActive(true);
                            ApplyCrosshairSize(CrosshairUI, hoverCrosshairSize);
                            if (defaultCrosshair != null) defaultCrosshair.SetActive(false);
                        }
                        alreadyHovered2 = false;
                        alreadyHovered = true;
                    }
                    hover = true;
                    if (Input.GetButtonDown("Interact"))
                    {
                        hit.transform.SendMessage("Interacting", SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetButtonDown("Squint"))
                    {
                        hit.transform.SendMessage("Looking", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
            else if (hit.transform.tag != "Interactable")
            {
                // Volver a la retícula por defecto
                if (hoverCrosshair != null)
                {
                    hoverCrosshair.SetActive(false);
                    if (defaultCrosshair != null)
                    {
                        defaultCrosshair.SetActive(true);
                        ApplyCrosshairSize(defaultCrosshair, defaultCrosshairSize);
                    }
                }
                else if (CrosshairUI != null)
                {
                    CrosshairUI.SetActive(false);
                    if (defaultCrosshair != null)
                    {
                        defaultCrosshair.SetActive(true);
                        ApplyCrosshairSize(defaultCrosshair, defaultCrosshairSize);
                    }
                }
                hover = false;
                alreadyHovered = false;
                if (!alreadyHovered2)
                {
                    anim.Play("An_InteractTextPopout");
                    alreadyHovered2 = true;
                }
                if (storedIntObj != null)
                {
                    storedIntObj.transform.SendMessage("UnHover", SendMessageOptions.DontRequireReceiver);
                    storedIntObj = null;
                }
            }
        }
        else
        {
            hover = false;
            dispText.text = "";
            // Asegurar que la retícula de hover esté oculta y la por defecto visible
            if (hoverCrosshair != null)
            {
                hoverCrosshair.SetActive(false);
                if (defaultCrosshair != null)
                {
                    defaultCrosshair.SetActive(true);
                    ApplyCrosshairSize(defaultCrosshair, defaultCrosshairSize);
                }
            }
            else if (CrosshairUI != null)
            {
                CrosshairUI.SetActive(false);
                if (defaultCrosshair != null)
                {
                    defaultCrosshair.SetActive(true);
                    ApplyCrosshairSize(defaultCrosshair, defaultCrosshairSize);
                }
            }
            storedIntObj = null;
        }
    }
}