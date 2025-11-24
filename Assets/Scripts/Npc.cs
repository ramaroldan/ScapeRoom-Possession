using DialogueEditor;
using Michsky.UI.Dark;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Forzando cursor visible");
        }
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
