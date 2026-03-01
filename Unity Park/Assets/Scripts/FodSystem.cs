using UnityEngine;

public class FogSystem : MonoBehaviour
{
    [Header("Fog Settings")]
    public float fogInsideDensity = 0.05f; // Πόσο πυκνή θα γίνει μέσα στη ζώνη

    private float defaultDensity; // Αρχική τιμή ομίχλης

    private void Start()
    {
        // Κρατάμε την αρχική τιμή της ομίχλης
        defaultDensity = RenderSettings.fogDensity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fogDensity = fogInsideDensity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RenderSettings.fogDensity = defaultDensity;
        }
    }
}