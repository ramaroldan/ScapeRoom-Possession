using DialogueEditor;
using Michsky.UI.Dark;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Npc: MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private MonoBehaviour cameraControlScript;


    private bool isPaused = false;
    public string playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPaused = !isPaused;
            cameraControlScript.enabled = !isPaused;

            Cursor.visible = isPaused;
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }
}
