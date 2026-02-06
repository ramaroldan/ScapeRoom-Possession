using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;

    public void Play()
    {
        audioSource.Play();
    }
}
