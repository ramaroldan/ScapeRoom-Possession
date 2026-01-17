using UnityEngine;

public class Speaker : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioClipSpanish;
    public AudioClip audioClipEnglish;

    public void PlayAudio()
    {
        //if (audioSource != null && audioClipSpanish != null)
        //{
        //    audioSource.clip = audioClipSpanish;
        //    audioSource.Play();
        //}
        if (audioSource == null) return;

        // Verificamos el idioma actual desde el LocalizationManager
        var lang = LocalizationManager.Instance.GetCurrentLanguage();

        switch (lang)
        {
            case LocalizationManager.Language.Spanish:
                if (audioClipSpanish != null)
                    audioSource.clip = audioClipSpanish;
                break;

            case LocalizationManager.Language.English:
                if (audioClipEnglish != null)
                    audioSource.clip = audioClipEnglish;
                break;
        }

        if (audioSource.clip != null)
            audioSource.Play();
    }
}
