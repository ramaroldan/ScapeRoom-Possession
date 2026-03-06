using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RandomAmbientSounds : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> clips = new List<AudioClip>();

    [Header("Timing (seconds)")]
    [SerializeField, Min(0f)] private float minDelay = 60f;   // 1 minuto
    [SerializeField, Min(0f)] private float maxDelay = 300f;  // 5 minutos

    [Header("Behavior")]
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool waitIfAudioIsPlaying = true; // si el clip dura más, espera a que termine
    [SerializeField] private bool avoidImmediateRepeat = true;

    private Coroutine routine;
    private int lastIndex = -1;

    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 2D por defecto (cambiá a 1 si querés 3D)
    }

    private void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    private void OnEnable()
    {
        if (playOnStart)
            StartRandomLoop();
    }

    private void OnDisable()
    {
        StopRandomLoop();
    }

    public void StartRandomLoop()
    {
        if (routine != null) return;
        routine = StartCoroutine(RandomLoop());
    }

    public void StopRandomLoop()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    private IEnumerator RandomLoop()
    {
        while (true)
        {
            if (clips == null || clips.Count == 0)
            {
                yield return null;
                continue;
            }

            // Delay aleatorio entre min y max (en segundos)
            float delay = Random.Range(Mathf.Min(minDelay, maxDelay), Mathf.Max(minDelay, maxDelay));
            yield return new WaitForSeconds(delay);

            int idx = GetRandomIndex();
            AudioClip clip = clips[idx];
            if (clip == null) continue;

            audioSource.PlayOneShot(clip);

            if (waitIfAudioIsPlaying)
            {
                // Si por alguna razón el AudioSource ya estaba reproduciendo algo, esperamos que termine.
                while (audioSource.isPlaying)
                    yield return null;
            }
        }
    }

    private int GetRandomIndex()
    {
        if (clips.Count == 1) return 0;

        int idx = Random.Range(0, clips.Count);
        if (!avoidImmediateRepeat) return idx;

        // Evitar repetir el mismo dos veces seguidas
        if (idx == lastIndex)
            idx = (idx + 1 + Random.Range(0, clips.Count - 1)) % clips.Count;

        lastIndex = idx;
        return idx;
    }
}