using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public bool isRaining;
    public bool isSlippery; // μπορεί να σημαίνει “πάγος/βροχή/υγρασία”

    [Header("References")]
    public LEDStripController[] ledStrips;

    // quick test με πλήκτρα
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRaining = !isRaining;
            // αν βρέχει, το κάνουμε “ολισθηρό”
            isSlippery = isRaining;
            Apply();
        }
    }

    public void Apply()
    {
        bool danger = isSlippery;

        foreach (var s in ledStrips)
            if (s != null) s.SetDanger(danger);

        Debug.Log($"Weather Apply: Raining={isRaining}, Slippery={isSlippery}, LEDs danger={danger}");
    }

    private void Start()
    {
        Apply();
    }
}