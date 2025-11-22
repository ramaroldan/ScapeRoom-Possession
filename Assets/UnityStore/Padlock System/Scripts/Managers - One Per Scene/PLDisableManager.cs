using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace PadlockSystem
{
    public class PLDisableManager : MonoBehaviour
    {
        public static PLDisableManager instance;

        [SerializeField] private GameObject player = null;
        [SerializeField] private PadlockRaycast mainCameraRaycast = null;
        [SerializeField] private Image crosshair = null; 

        void Awake()
        {
            if (instance != null) { Destroy(gameObject); }
            else { instance = this; DontDestroyOnLoad(gameObject); }
        }
        void Start()
        {
            if (player == null)
            {
                GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
                player = playerGO;
                if (player == null)
                    Debug.LogError("No se encontró FirstPersonController en la escena.");
            }

            if (mainCameraRaycast == null)
            {
                mainCameraRaycast = FindObjectOfType<PadlockRaycast>();
                if (mainCameraRaycast == null)
                    Debug.LogError("No se encontró PadlockRaycast en la escena.");
            }

            if (crosshair == null)
            {
                GameObject crosshairGO = GameObject.Find("CrosshairUI");
                if (crosshairGO != null)
                {
                    crosshair = crosshairGO.GetComponent<Image>();
                }

                if (crosshair == null)
                    Debug.LogWarning("No se encontró el Crosshair o no tiene componente Image.");
            }
        }


        public void DisablePlayer(bool disable)
        {
            if (disable)
            {
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerMovement>().enabled = false;
                player.GetComponent<MouseLook>().enabled = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                mainCameraRaycast.enabled = false;
                crosshair.enabled = false;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerMovement>().enabled = true;
                player.GetComponent<MouseLook>().enabled = true;
                mainCameraRaycast.enabled = true;
                crosshair.enabled = true;
            }
        }
    }
}
