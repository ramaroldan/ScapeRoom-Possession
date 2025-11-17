using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering.PostProcessing;

/* 

    First Person Interaction Toolkit by Steven Harmon stevenharmongames.com
    Licensed under the MPL 2.0. https://www.mozilla.org/en-US/MPL/2.0/FAQ/
    Please use in your walking sims/horror/adventure/puzzle games! Drop me a line and share what make with it! :)    

 */

[RequireComponent(typeof(Camera))]
public class Squint : MonoBehaviour
{
    //Squint.cs right click to zoom in fov and vignette intensity
    public static Squint Instance;
    [HideInInspector]
    public float changeFov = 45;
    private float defFov = 80;
    private float fov;
    private bool on = false;
    private Camera mainCam;
    [Tooltip("Toggle or hold")]
    public bool toggle = false;
    [Tooltip("Is this script active or not")]
    public bool working = true;
    public PostProcessVolume volume;
    [HideInInspector]
    public Vignette ppv_vig;
    private float timer = 0.2f;
    //AutoFocus
    private GameObject doFFocusTarget;
    private Vector3 lastDoFPoint;
    [HideInInspector]
    public DepthOfField ppv_dop;//default focal length 16, default apeture 5.6
    [Tooltip("Quality of depth of field focus post processing blur")]
    public DoFAFocusQuality focusQuality = Squint.DoFAFocusQuality.NORMAL;
    [Tooltip("Which layers does raycast for depth of field use?")]
    public LayerMask hitLayer = 1;
    [Tooltip("Max raycast distance for depth of field effect")]
    public float maxDistance = 100.0f;
    [Tooltip("Focus lerps in to target or instant")]
    public bool interpolateFocus = true;
    [Tooltip("for easing in of depth of field, .1 is safe")]
    public float interpolationTime = 0.1f;
    [Tooltip("Is ignoring dynamic depth of field calculation")]
    public bool ignoreDOP = false;
    private bool isReady = false;

    public enum DoFAFocusQuality
    {
        NORMAL,
        HIGH
    }
    private void Awake()
    {
        // Singleton para evitar duplicados
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += SceneChanged;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= SceneChanged;
    }

    private void SceneChanged(UnityEngine.SceneManagement.Scene oldScene, UnityEngine.SceneManagement.Scene newScene)
    {
        // Rebind camera every time the scene changes
        RebindCamera();
    }

    // Use this for initialization
    void Start()
    {
        if (gameObject.GetComponent<Camera>() != null)
        {
            mainCam = gameObject.GetComponent<Camera>();
            mainCam.fieldOfView = defFov;
            fov = defFov;
        }
        volume.profile.TryGetSettings(out ppv_vig);
        volume.profile.TryGetSettings(out ppv_dop);
        doFFocusTarget = new GameObject("DoFFocusTarget");
        DontDestroyOnLoad(doFFocusTarget);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady) return;
        // switch between Modes Test Focus every Frame
        if (focusQuality == Squint.DoFAFocusQuality.HIGH)
        {
            if (!ignoreDOP)
            {
                Focus();
            }
            else
            {
                ppv_dop.focusDistance.value = 1.3f;
            }
        }

        //mainCam.fieldOfView = fov;
        if (mainCam != null)
            mainCam.fieldOfView = fov;


        if (working)
        {
            if (toggle)
            {
                if (Input.GetButtonDown("Squint"))
                {
                    on = !on;
                }
            }
            else if (!toggle)
            {
                if (Input.GetButton("Squint"))
                {
                    on = true;
                }
                else
                {
                    on = false;
                }
            }
            if (on)
            {
                fov = Mathf.Lerp(fov, changeFov, Time.deltaTime * 1.5f);
                timer += Time.deltaTime * 0.5f;
                ppv_vig.intensity.value = timer;
                timer = Mathf.Clamp(timer, 0.1f, 0.4f);
            }
            else if (!on)
            {
                fov = Mathf.Lerp(fov, defFov, Time.deltaTime * 1.5f);
                timer -= Time.deltaTime * 0.5f;
                ppv_vig.intensity.value = timer;
                timer = Mathf.Clamp(timer, 0.1f, 0.4f);
            }
        }
        else
        {
            fov = Mathf.Lerp(fov, defFov, Time.deltaTime * 1.5f);
            ppv_vig.intensity.value = .2f;
        }
    }

    void FixedUpdate()
    {
        if (!isReady) return;
        // switch between modes Test Focus like the Physicsupdate
        if (focusQuality == Squint.DoFAFocusQuality.NORMAL)
        {
            if (!ignoreDOP)
            {
                ppv_dop.active = true;
                Focus();
            }
            else
            {
                ppv_dop.active = false;
                ppv_dop.focusDistance.value = 2.2f;
            }
        }
    }

    IEnumerator InterpolateFocus(Vector3 targetPosition)
    {
        // Debug.Log("Entre a InterpolateFocus");

        Vector3 start = this.doFFocusTarget.transform.position;
        Vector3 end = targetPosition;
        float dTime = 0;

       // if(start==null) Debug.LogError("❌ start es nulo");
        //if (end == null) Debug.LogError("❌ end es nulo");

        // Debug.DrawLine(start, end, Color.green);
        while (dTime < 1)
        {
           // Debug.Log("Entre al while");
            yield return new WaitForEndOfFrame();
            dTime += Time.deltaTime / this.interpolationTime;
            this.doFFocusTarget.transform.position = Vector3.Lerp(start, end, dTime);
            //if (ppv_dop == null) Debug.LogError("❌ ppv_dop es nulo");
            ppv_dop.focusDistance.value = Vector3.Distance(doFFocusTarget.transform.position, transform.position);
        }
        this.doFFocusTarget.transform.position = end;
       // Debug.Log("finalizo InterpolateFocus");
    }
    public void RebindCamera()
    {
        var cam = Camera.main;
        if (cam == null)
        {
            isReady = false;
            //Debug.LogError("❌ No hay MainCamera en la escena para re-asignar a Squint.");
            return;
        }
        mainCam = cam;

        // BUSCAR EL VOLUMEN EN HIJOS (Correcto para tu proyecto)
        var vol = cam.GetComponentInChildren<PostProcessVolume>(true);

        if (vol != null)
        {
            volume = vol;
            volume.profile.TryGetSettings(out ppv_vig);
            volume.profile.TryGetSettings(out ppv_dop);
            isReady = true;
            //Debug.Log("✅ Squint re-asignó cámara + POSTPROCESS VOLUME correctamente.");
        }
        else
        {
            //Debug.LogError("❌ La MainCamera NO TIENE PostProcessVolume como hijo.");
            isReady = false;
        }
        if (doFFocusTarget == null)
        {
            doFFocusTarget = new GameObject("DoFFocusTarget");
            DontDestroyOnLoad(doFFocusTarget);
          //  Debug.Log("♻️ Se recreó doFFocusTarget");
        }

    }



    void Focus()
    {
        // our ray
        Vector3 rayOrigin = GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, this.maxDistance, this.hitLayer))
        {
            Debug.DrawLine(rayOrigin, hit.point);

            // do we have a new point?					
            if (this.lastDoFPoint == hit.point)
            {
                return;
                // No, do nothing
            }
            else if (this.interpolateFocus)
            { // Do we interpolate from last point to the new Focus Point ?
              // stop the Coroutine
                StopCoroutine("InterpolateFocus");
                // start new Coroutine
                //if(hit.point == null) Debug.LogError("❌ hit.point es nullo");

               // Debug.Log("por arrancar StartCoroutine InterpolateFocus");
                StartCoroutine(InterpolateFocus(hit.point));
               // Debug.Log("finalizo StartCoroutine");
            }
            else
            {
                this.doFFocusTarget.transform.position = hit.point;
                ppv_dop.focusDistance.value = Vector3.Distance(doFFocusTarget.transform.position, transform.position);
                // print(depthOfField.focusDistance);
            }
            // asign the last hit
            this.lastDoFPoint = hit.point;
        }
    }
}
