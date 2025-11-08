using Michsky.UI.Dark;
using UnityEngine;

public class SceneStartManager : MonoBehaviour
{
    public GameObject entradaPanel;
    private Animator entradaAnimator;
    private PanelBrushManager entradaBrush;

    public string fadeInAnim = "Panel Out";

    void Start()
    {
        entradaAnimator = entradaPanel.GetComponent<Animator>();
        entradaAnimator.Play(fadeInAnim);

        entradaBrush = entradaPanel.GetComponent<PanelBrushManager>();
        if (entradaBrush != null && entradaBrush.brushAnimator != null)
        {
            entradaBrush.BrushSplashIn();
        }
    }
}
