using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [Header("Rain Settings")]
    public float rainDuration = 10f;      // πόσο κρατάει η βροχή
    public float clearDuration = 10f;     // πόσο κρατάει ο ήλιος

    public ParticleSystem rainVFX;
    public LEDStripController[] ledStrips;

    private bool isRaining;

    private void Start()
    {
        StartCoroutine(WeatherLoop());
    }

    IEnumerator WeatherLoop()
    {
        while (true)
        {
            // 🌧 Ξεκινά βροχή
            isRaining = true;
            ApplyWeather();
            yield return new WaitForSeconds(rainDuration);

            // ☀ Σταματά βροχή
            isRaining = false;
            ApplyWeather();
            yield return new WaitForSeconds(clearDuration);
        }
    }

    void ApplyWeather()
    {
        // Rain particles
        if (rainVFX != null)
        {
            if (isRaining)
                rainVFX.Play();
            else
                rainVFX.Stop();
        }

        // LED strips
        foreach (var s in ledStrips)
        {
            if (s != null)
                s.SetDanger(isRaining);
        }
    }
}