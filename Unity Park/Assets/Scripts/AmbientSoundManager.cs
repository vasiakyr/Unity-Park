using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip normalAmbience;
    public AudioClip rainAmbience;

    private bool isRaining = false;

    private void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null && normalAmbience != null)
        {
            audioSource.clip = normalAmbience;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void SetRain(bool rain)
    {
        if (audioSource == null) return;
        if (rain == isRaining) return;

        isRaining = rain;

        audioSource.Stop();

        if (isRaining)
        {
            audioSource.clip = rainAmbience;
        }
        else
        {
            audioSource.clip = normalAmbience;
        }

        audioSource.loop = true;
        audioSource.Play();
    }
}