using DialogueEditor;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;



public class Npc: MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    [SerializeField] private MonoBehaviour playerMovement;

    [SerializeField] private MouseLook mouseLookPlayer;
    [SerializeField] private MouseLook mouseLookCamera;

    private bool isPaused = false;
    public string playerTag = "Player";

    [Header("Unlock Events")]
    [SerializeField] private UnityEvent unlock = null;
    [SerializeField] private GameObject linterna = null;

    private void Start()
    {
        StartCoroutine(FindPlayerReferences());
    }
    private System.Collections.IEnumerator FindPlayerReferences()
    {
        if(playerMovement != null)
            yield break;
        // Esperar hasta que PlayerMovement esté disponible
        while (playerMovement == null)
        {
            playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
            yield return null;
        }

        // Buscar MouseLook en hijos de Player
        MouseLook[] looks = Object.FindObjectsOfType<MouseLook>(true);
        foreach (var ml in looks)
        {
            if (ml.CompareTag("Player"))
                mouseLookPlayer = ml;
            else if (ml.CompareTag("MainCamera"))
                mouseLookCamera = ml;
        }

        if (mouseLookPlayer == null || mouseLookCamera == null)
        {
            Debug.LogWarning("MouseLook references no fueron encontradas correctamente.");
        }
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {            
            SetPlayerControl(true);
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            SetPlayerControl(false);
            ConversationManager.Instance.EndConversation();
        }
    }

    public void SetPlayerControl(bool isUIActive)
    {
        if(mouseLookPlayer == null) StartCoroutine(FindPlayerReferences());

        if (mouseLookPlayer != null)
        {
            mouseLookPlayer.overrideCursorLock = isUIActive;
            Debug.Log("Override cursor lock seteado a: " + mouseLookPlayer.overrideCursorLock);
        }
        if (mouseLookCamera != null)
        {
            mouseLookCamera.overrideCursorLock = isUIActive;
            Debug.Log("Override cursor lock seteado a: " + mouseLookCamera.overrideCursorLock);
        }

        Cursor.visible = isUIActive;
        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void UnlockLinterna()
    {
        if (linterna != null)
        {
            linterna.SetActive(true);
            Debug.Log("Linterna desbloqueada");
        }
        else
        {
            Debug.LogWarning("Linterna no asignada en el inspector.");
        }
       
    }
}
