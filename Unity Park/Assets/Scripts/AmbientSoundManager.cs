using UnityEngine;

public class AmbientSoundManager : MonoBehaviour
{
    public static AmbientSoundManager Instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Ambient Clips")]
    public AudioClip normalAmbience;
    public AudioClip rainAmbience;

    private bool isRaining = false;

    private void Awake()
    {
        // Αν υπάρχει ήδη άλλο instance, σβήσε αυτό
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayNormal();
    }

    public void PlayNormal()
    {
        if (audioSource == null || normalAmbience == null) return;

        isRaining = false;

        if (audioSource.clip != normalAmbience)
            audioSource.Stop();

        audioSource.clip = normalAmbience;
        audioSource.loop = true;

        if (!audioSource.isPlaying)
            audioSource.Play();
        else
            audioSource.Play();
    }

    public void PlayRain()
    {
        if (audioSource == null || rainAmbience == null) return;

        isRaining = true;

        if (audioSource.clip != rainAmbience)
            audioSource.Stop();

        audioSource.clip = rainAmbience;
        audioSource.loop = true;

        if (!audioSource.isPlaying)
            audioSource.Play();
        else
            audioSource.Play();
    }

    public void SetRain(bool rain)
    {
        if (rain == isRaining) return;

        if (rain)
            PlayRain();
        else
            PlayNormal();
    }
}