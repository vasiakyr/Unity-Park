using UnityEngine;

public class LEDStripController : MonoBehaviour
{
    [Header("Renderer(s) που θα αλλάζουν emission")]
    public Renderer[] renderers;

    [Header("Colors")]
    public Color safeColor = Color.green;  // προσβάσιμο
    public Color dangerColor = Color.blue; // ολισθηρό/κίνδυνος

    [Header("Emission Intensity")]
    [Range(0f, 20f)] public float safeIntensity = 2f;
    [Range(0f, 20f)] public float dangerIntensity = 6f;

    private bool _isDanger;

    public void SetDanger(bool danger)
    {
        _isDanger = danger;
        Apply();
    }

    private void Awake()
    {
        Apply();
    }

    private void Apply()
    {
        Color c = _isDanger ? dangerColor : safeColor;
        float intensity = _isDanger ? dangerIntensity : safeIntensity;

        // HDR emission: χρώμα * ένταση
        Color emissive = c * intensity;

        foreach (var r in renderers)
        {
            if (r == null) continue;

            // Καλύτερα να έχεις material instance ανά strip:
            var mat = r.material;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", emissive);
        }
    }
}