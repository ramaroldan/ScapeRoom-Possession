//using System.Collections;
//using UnityEngine;
//using UnityEngine.Events;

//namespace PadlockSystem
//{
//    public class PadlockController : MonoBehaviour
//    {
//        [Header("Your Inputs")]
//        [SerializeField] private string yourCombination = "1234";
//        private string playerCombi;
//        private bool hasUnlocked;
//        private bool isShowing;
//        private Camera mainCamera;

//        //Hiddie from the inspector because these are only integers to hold some information for later.
//        [HideInInspector] public int combinationRow1;
//        [HideInInspector] public int combinationRow2;
//        [HideInInspector] public int combinationRow3;
//        [HideInInspector] public int combinationRow4;

//        [Header("Interactive Padlock")]
//        [SerializeField] private GameObject interactableLock = null;

//        [Header("Camera GameObject References")]
//        [SerializeField] private GameObject cameraPadlock = null;
//        private Animator lockAnim;

//        [Header("Audio Names")]
//        [SerializeField] private string padlockInteractSound = "PadlockInteract";
//        [SerializeField] private string padlockSpinSound = "PadlockSpin";
//        [SerializeField] private string padlockUnlockSound = "PadlockUnlock";

//        [Header("Trigger Type - ONLY if using a trigger event")]
//        [SerializeField] private PadlockTrigger triggerObject = null;
//        [SerializeField] private bool isPadlockTrigger = false;

//        [Header("Unlock Events")]
//        [SerializeField] private UnityEvent unlock = null;

//        void Awake()
//        {
//            mainCamera = Camera.main;
//            // lockAnim = cameraPadlock.GetComponentInChildren<Animator>();
//            if (mainCamera != null)
//            {
//                // Buscar el Padlock dentro de la cámara principal
//                cameraPadlock = mainCamera.transform.Find("Padlock_Camera_Mechanism_A1")?.gameObject;

//                if (cameraPadlock != null)
//                {
//                    lockAnim = cameraPadlock.GetComponentInChildren<Animator>();
//                }
//                else
//                {
//                    Debug.LogWarning("No se encontró el Padlock_Camera_Mechanism_A1 en la cámara principal.");
//                }
//            }
//            else
//            {
//                Debug.LogError("No se encontró la cámara principal.");
//            }
//            combinationRow1 = 1;
//            combinationRow2 = 1;
//            combinationRow3 = 1;
//            combinationRow4 = 1;
//        }

//        void UnlockPadlock()
//        {
//            unlock.Invoke();
//        }
//        public void ClosePadlock()
//        {
//            DisablePadlock();
//        }

//        public void ShowPadlock()
//        {
//           // PLDisableManager.instance.DisablePlayer(true);
//          //  cameraPadlock.SetActive(true);
//            isShowing = true;    
//            mainCamera.transform.localEulerAngles = new Vector3(0, 0, 0);
//            InteractSound();

//            if (isPadlockTrigger)
//            {
//                triggerObject.interactPrompt.SetActive(false);
//                triggerObject.enabled = false;
//            }
//        }

//        void DisablePadlock()
//        {
//           // PLDisableManager.instance.DisablePlayer(false);
//            cameraPadlock.SetActive(false);
//            isShowing = false;

//            if (isPadlockTrigger)
//            {
//                triggerObject.interactPrompt.SetActive(true);
//                triggerObject.enabled = true;
//            }
//        }

//        public void CheckCombination()
//        {
//            playerCombi = combinationRow1.ToString("0") + combinationRow2.ToString("0") + combinationRow3.ToString("0") + combinationRow4.ToString("0");

//            if (playerCombi == yourCombination)
//            {
//                if (!hasUnlocked)
//                {
//                    StartCoroutine(CorrectCombination());
//                    hasUnlocked = true;
//                }
//            }
//        }

//        IEnumerator CorrectCombination()
//        {
//            lockAnim.Play("LockOpen");
//            UnlockSound();

//            const float waitDuration = 1.2f;
//            yield return new WaitForSeconds(waitDuration);

//            cameraPadlock.SetActive(false);
//            interactableLock.SetActive(false);
//            UnlockPadlock();

//            PLDisableManager.instance.DisablePlayer(false);
//            gameObject.SetActive(false);
//        }

//        void Update()
//        {
//            if (isShowing)
//            {
//                if (Input.GetKeyDown(PLInputManager.instance.closeKey))
//                {
//                    DisablePadlock();
//                }
//            }
//        }

//        void InteractSound()
//        {
//            PLAudioManager.instance.Play(padlockInteractSound);
//        }

//        public void SpinSound()
//        {
//            PLAudioManager.instance.Play(padlockSpinSound);
//        }

//        public void UnlockSound()
//        {
//            PLAudioManager.instance.Play(padlockUnlockSound);
//        }
//    }
//}
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PadlockSystem
{
    public class PadlockController : MonoBehaviour
    {
        [Header("Your Inputs")]
        [SerializeField] private string yourCombination = "1234";
        private string playerCombi;
        private bool hasUnlocked;
        private bool isShowing;

        [HideInInspector] public int combinationRow1;
        [HideInInspector] public int combinationRow2;
        [HideInInspector] public int combinationRow3;
        [HideInInspector] public int combinationRow4;

        [Header("Interactive Padlock")]
        [SerializeField] private GameObject interactableLock = null;

        [Header("Camera GameObject References")]
        [SerializeField] private GameObject cameraPadlock = null;
        private Animator lockAnim;

        [Header("Audio Names")]
        [SerializeField] private string padlockInteractSound = "PadlockInteract";
        [SerializeField] private string padlockSpinSound = "PadlockSpin";
        [SerializeField] private string padlockUnlockSound = "PadlockUnlock";

        [Header("Trigger Type - ONLY if using a trigger event")]
        [SerializeField] private PadlockTrigger triggerObject = null;
        [SerializeField] private bool isPadlockTrigger = false;

        [Header("Unlock Events")]
        [SerializeField] private UnityEvent unlock = null;
        private Camera playerCamera;
        private void Start()
        {
            // Obtener el animator directamente del GameObject referenciado
            if (cameraPadlock != null)
            {
                lockAnim = cameraPadlock.GetComponentInChildren<Animator>();
                cameraPadlock.SetActive(false); // por si quedó activa en el prefab
            }
            else
            {
                Debug.LogWarning("❌ No se asignó el GameObject de la cámara del padlock.");
            }

            combinationRow1 = 1;
            combinationRow2 = 1;
            combinationRow3 = 1;
            combinationRow4 = 1;
        }
        void Awake()
        {
            // Lanzar la corutina para esperar a la cámara
            StartCoroutine(SetupPadlockObject());
           
        }
        private IEnumerator SetupPadlockObject()
        {
            while (Camera.main == null)
                yield return null;

            playerCamera = Camera.main;

            Transform padlockTransform = null;

            // Buscar aunque esté desactivado
            foreach (Transform t in playerCamera.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "Padlock_Camera_Mechanism_A1")
                {
                    padlockTransform = t;
                    break;
                }
            }

            if (padlockTransform != null)
            {
                cameraPadlock = padlockTransform.gameObject;
                cameraPadlock.SetActive(false); // asegurarse que arranca apagado
                Debug.Log("<color=green>✔ Se asignó correctamente el objeto del padlock.</color>");
            }
            else
            {
                Debug.LogWarning("❌ No se encontró el objeto 'Padlock_Camera_Mechanism_A1'.");
            }
        }
        public void ShowPadlock()
        {
            cameraPadlock.SetActive(true);
            isShowing = true;
            InteractSound();

            if (isPadlockTrigger)
            {
                triggerObject.interactPrompt.SetActive(false);
                triggerObject.enabled = false;
            }
        }

        public void ClosePadlock()
        {
            DisablePadlock();
        }

        private void DisablePadlock()
        {
            cameraPadlock.SetActive(false);
            isShowing = false;

            if (isPadlockTrigger)
            {
                triggerObject.interactPrompt.SetActive(true);
                triggerObject.enabled = true;
            }
        }

        public void CheckCombination()
        {
            playerCombi = combinationRow1.ToString("0") +
                          combinationRow2.ToString("0") +
                          combinationRow3.ToString("0") +
                          combinationRow4.ToString("0");

            if (playerCombi == yourCombination && !hasUnlocked)
            {
                StartCoroutine(CorrectCombination());
                hasUnlocked = true;
            }
        }

        IEnumerator CorrectCombination()
        {
            if (lockAnim != null)
                lockAnim.Play("LockOpen");

            UnlockSound();

            yield return new WaitForSeconds(1.2f);

            cameraPadlock.SetActive(false);
            OnPadlockClosed?.Invoke();
            interactableLock.SetActive(false);
            unlock.Invoke();
          
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (isShowing && Input.GetKeyDown(PLInputManager.instance.closeKey))
            {
                DisablePadlock();
            }
        }

        void InteractSound()
        {
            PLAudioManager.instance.Play(padlockInteractSound);
        }

        public void SpinSound()
        {
            PLAudioManager.instance.Play(padlockSpinSound);
        }

        public void UnlockSound()
        {
            PLAudioManager.instance.Play(padlockUnlockSound);
        }

        public UnityEvent OnPadlockClosed;
    }
}

