using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [Header("Rain Settings")]
    public float rainDuration = 10f;
    public float clearDuration = 10f;

    public ParticleSystem rainVFX;
    public LEDStripController[] ledStrips;

    [SerializeField] private bool isRaining;
    public bool IsRaining => isRaining;

    private void Start()
    {
        StartCoroutine(WeatherLoop());
    }

    IEnumerator WeatherLoop()
    {
        while (true)
        {
            isRaining = true;
            ApplyWeather();
            yield return new WaitForSeconds(rainDuration);

            isRaining = false;
            ApplyWeather();
            yield return new WaitForSeconds(clearDuration);
        }
    }

    void ApplyWeather()
    {
        if (rainVFX != null)
        {
            if (isRaining) rainVFX.Play();
            else rainVFX.Stop();
        }

        if (ledStrips != null)
        {
            foreach (var s in ledStrips)
            {
                if (s != null) s.SetDanger(isRaining);
            }
        }
    }
}