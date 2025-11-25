using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PadlockSystem
{
    public class PadlockController : MonoBehaviour
    {
        

        [Header("Your Inputs")]
        [SerializeField] private string yourCombination = "1234";
        [SerializeField] private string sceneName = "1234";
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

        [SerializeField] private PowerBoxCover powerBoxCover;

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
            // 🔥 Buscar y desactivar cualquier padlock residual de escenas anteriores
            foreach (Transform t in playerCamera.GetComponentsInChildren<Transform>(true))
            {
                if (t.name.StartsWith("Padlock_Camera_Mechanism_"))
                {
                    t.gameObject.SetActive(false);
                }
            }

            // Buscar aunque esté desactivado
            foreach (Transform t in playerCamera.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "Padlock_Camera_Mechanism_A1" && sceneName=="HospitalRoom")
                {
                    padlockTransform = t;
                    break;
                }
                if (t.name == "Padlock_Camera_Mechanism_A2" && sceneName == "Morgue")
                {
                    padlockTransform = t;
                    break;
                }
                if (t.name == "Padlock_Camera_Mechanism_A3" && sceneName == "Museum")
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

            if(powerBoxCover!=null)
                powerBoxCover.DropCover();

            cameraPadlock.SetActive(false);
            OnPadlockClosed?.Invoke();
            interactableLock.SetActive(false);
            unlock.Invoke();
          
            gameObject.SetActive(false);

            
        }

        public void ResetearCandado()
        {
            combinationRow1 = 1;
            combinationRow2 = 1;
            combinationRow3 = 1;
            combinationRow4 = 1;
            hasUnlocked = false;
            if (lockAnim != null)
                lockAnim.Play("Default");
            interactableLock.SetActive(true);
            //gameObject.SetActive(true);
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

