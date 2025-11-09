using UnityEngine;

namespace Michsky.UI.Dark
{
    public class ModalWindowManager : MonoBehaviour
    {
        [Header("BRUSH ANIMATION")]
        public Animator brushAnimator;
        public bool enableSplash = true;

        private Animator mWindowAnimator;

        void Start()
        {
            mWindowAnimator = gameObject.GetComponent<Animator>();
        }

        public void ModalWindowIn()
        {
            mWindowAnimator.Play("Modal Window In");

            if(enableSplash == true)
            {
                brushAnimator.Play("Transition Out");
            }
        }
        public void ModalWindowInTest()
        {
            gameObject.SetActive(true); // Asegurarte de que esté activo
            mWindowAnimator.Play("Modal Window In");

            // Resetear alpha si estaba en 0
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }

            if (enableSplash && brushAnimator != null)
            {
                brushAnimator.Play("Transition Out");
            }
        }


        public void ModalWindowOut()
        {
            mWindowAnimator.Play("Modal Window Out");

            if (enableSplash == true)
            {
                brushAnimator.Play("Transition In");
            }
        }
    }
}