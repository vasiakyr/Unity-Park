using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [Header("Weather State")]
    public bool isRaining = false;

    [Header("References")]
    public ParticleSystem rainVFX;
    public AmbientSoundManager ambientSoundManager;

    private void Start()
    {
        // Αν δεν έχει μπει από Inspector, πάρε το global instance
        if (ambientSoundManager == null)
            ambientSoundManager = AmbientSoundManager.Instance;

        ApplyWeather();
    }

    public void SetRain(bool rain)
    {
        isRaining = rain;
        ApplyWeather();
    }

    public void ToggleRain()
    {
        isRaining = !isRaining;
        ApplyWeather();
    }

    private void ApplyWeather()
    {
        // Rain VFX
        if (rainVFX != null)
        {
            if (isRaining)
            {
                if (!rainVFX.isPlaying)
                    rainVFX.Play();
            }
            else
            {
                if (rainVFX.isPlaying)
                    rainVFX.Stop();
            }
        }

        // Rain Sound
        if (ambientSoundManager != null)
        {
            ambientSoundManager.SetRain(isRaining);
        }
    }
}