using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Michsky.UI.Dark
{
    public class MainPanelManager : MonoBehaviour
    {
        [Header("PANEL LIST")]
        public List<GameObject> panels = new List<GameObject>();

        [Header("RESOURCES")]
        public BlurManager homeBlurManager;

        [Header("SETTINGS")]
        public int currentPanelIndex = 0;
        public bool enableBrushAnimation = true;
        public bool enableHomeBlur = true;
         
        private GameObject currentPanel;
        private GameObject nextPanel;
        private Animator currentPanelAnimator;
        private Animator nextPanelAnimator;

        string panelFadeIn = "Panel In";
        string panelFadeOut = "Panel Out";

        PanelBrushManager currentBrush;
        PanelBrushManager nextBrush;

        [SerializeField] private GameObject backgrounds;

        void Start()
        {
            currentPanel = panels[currentPanelIndex];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);

            if (enableHomeBlur == true)
                homeBlurManager.BlurInAnim();
        }

        public void OpenFirstTab()
        {
            currentPanel = panels[currentPanelIndex];
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);

            if (enableHomeBlur == true)
                homeBlurManager.BlurInAnim();
        }

        public void PanelAnim(int newPanel)
        {
            if (newPanel != currentPanelIndex)
            {
                currentPanel = panels[currentPanelIndex];

                currentPanelIndex = newPanel;
                nextPanel = panels[currentPanelIndex];
                nextPanel.SetActive(true);
                currentPanelAnimator = currentPanel.GetComponent<Animator>();                
                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBrushAnimation == true)
                {
                    currentBrush = currentPanel.GetComponent<PanelBrushManager>();
                    if (currentBrush.brushAnimator != null)
                        currentBrush.BrushSplashOut();
                    nextBrush = nextPanel.GetComponent<PanelBrushManager>();
                    if (nextBrush.brushAnimator != null)
                        nextBrush.BrushSplashIn();
                }

                if (currentPanelIndex == 0 && enableHomeBlur == true)
                    homeBlurManager.BlurInAnim();
                else if (currentPanelIndex != 0 && enableHomeBlur == true)
                    homeBlurManager.BlurOutAnim();
            }
        }

        public void NextPage()
        {
            if (currentPanelIndex <= panels.Count - 2)
            {
                currentPanel = panels[currentPanelIndex];
                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex += 1;
                nextPanel = panels[currentPanelIndex];

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBrushAnimation == true)
                {
                    currentBrush = currentPanel.GetComponent<PanelBrushManager>();
                    if (currentBrush.brushAnimator != null)
                        currentBrush.BrushSplashOut();
                    nextBrush = nextPanel.GetComponent<PanelBrushManager>();
                    if (nextBrush.brushAnimator != null)
                        nextBrush.BrushSplashIn();
                }

                if (currentPanelIndex == 0 && enableHomeBlur == true)
                    homeBlurManager.BlurInAnim();
                else if (currentPanelIndex != 0 && enableHomeBlur == true)
                    homeBlurManager.BlurOutAnim();
            }
        }

        public void PrevPage()
        {
            if (currentPanelIndex >= 1)
            {
                currentPanel = panels[currentPanelIndex];
                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex -= 1;
                nextPanel = panels[currentPanelIndex];

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);

                if (enableBrushAnimation == true)
                {
                    currentBrush = currentPanel.GetComponent<PanelBrushManager>();
                    if (currentBrush.brushAnimator != null)
                        currentBrush.BrushSplashOut();
                    nextBrush = nextPanel.GetComponent<PanelBrushManager>();
                    if (nextBrush.brushAnimator != null)
                        nextBrush.BrushSplashIn();
                }

                if (currentPanelIndex == 0 && enableHomeBlur == true)
                    homeBlurManager.BlurInAnim();
                else if (currentPanelIndex != 0 && enableHomeBlur == true)
                    homeBlurManager.BlurOutAnim();
            }
        }

        public void EscenaAnim(string escena)
        {
            StartCoroutine(EsceneAnimCoroutine(escena));
        }
        public IEnumerator EsceneAnimCoroutine(string escenaName)
        {
            // Animator del panel actual
            Animator panelAnimator = currentPanel.GetComponent<Animator>();
            if (panelAnimator != null)
                panelAnimator.Play("FadeOut");

            // Brush out si está habilitado
            if (enableBrushAnimation && currentBrush != null && currentBrush.brushAnimator != null)
                currentBrush.BrushSplashOut();

            if (backgrounds != null)
                backgrounds.SetActive(false); // o .GetComponent<Animator>().Play("FadeOutBackground");

            // Esperar que termine la animación
            yield return new WaitForSeconds(1.2f); // Ajustá este tiempo según la duración real del fade

            SceneManager.LoadScene(escenaName);
            //currentPanel = panels[currentPanelIndex];
            //currentPanelAnimator = currentPanel.GetComponent<Animator>();

            //// FadeOut animación
            //currentPanelAnimator.Play(panelFadeOut);

            //// Brush out
            //if (enableBrushAnimation)
            //{
            //    currentBrush = currentPanel.GetComponent<PanelBrushManager>();
            //    if (currentBrush != null && currentBrush.brushAnimator != null)
            //        currentBrush.BrushSplashOut();
            //}

            //// Esperamos un poco y luego cargamos la escena
            //StartCoroutine(LoadSceneAfterDelay(escenaName, 1.2f)); // ajustá el tiempo al largo del fade
        }

        private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(sceneName);
        }

    }
}