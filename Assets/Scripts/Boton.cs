using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Boton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject normalText;
    public GameObject highlightedText;
    // Guardamos referencia al TMP y al material
    private TextMeshProUGUI tmpText;
    private Material instanceMaterial;

    private void Start() // CAMBIAMOS DE Awake a Start
    {
        if (highlightedText != null)
        {
            // Activar temporalmente para inicializar
            bool wasActive = highlightedText.activeSelf;
            highlightedText.SetActive(true);

            tmpText = highlightedText.GetComponent<TextMeshProUGUI>();
            instanceMaterial = Instantiate(tmpText.fontMaterial);
            tmpText.fontMaterial = instanceMaterial;

            // Setear valores base del glow
            instanceMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
            instanceMaterial.SetColor(ShaderUtilities.ID_GlowColor, Color.white * 10f);

            // Volver al estado original
            highlightedText.SetActive(wasActive);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlightedText != null) highlightedText.SetActive(true);
        if (normalText != null) normalText.SetActive(false);

        // Activamos el glow
        if (instanceMaterial != null)
        {
            instanceMaterial.SetColor(ShaderUtilities.ID_GlowColor, Color.white * 10f); // BRILLO real
            instanceMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.9f);
            instanceMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, 0.5f);
            instanceMaterial.SetFloat(ShaderUtilities.ID_GlowInner, 0.3f);
            instanceMaterial.SetFloat(ShaderUtilities.ID_GlowOffset, 0f);

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightedText != null) highlightedText.SetActive(false);
        if (normalText != null) normalText.SetActive(true);

        // Desactivamos el glow
        if (instanceMaterial != null)
        {
            instanceMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
        }
    }
    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (highlightedText != null) highlightedText.SetActive(true);
    //    if (normalText != null) normalText.SetActive(false);
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (highlightedText != null) highlightedText.SetActive(false);
    //    if (normalText != null) normalText.SetActive(true);
    //}
}