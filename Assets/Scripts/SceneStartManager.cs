
using UnityEngine;

public class SceneStartManager : MonoBehaviour
{
    public GameObject entradaPanel;
    private Animator entradaAnimator;
   

    public string fadeInAnim = "Panel Out";

    void Start()
    {
        entradaAnimator = entradaPanel.GetComponent<Animator>();
        entradaAnimator.Play(fadeInAnim);

       
    }
}
