using UnityEngine;

public class FogNode : MonoBehaviour
{
    [Header("References")]
    public TrafficLight trafficLight;

    [Header("Decision")]
    public float requiredWaitTime = 2f;

    private bool pedestrianInZone;
    private float timer;

    private void Update()
    {
        if (pedestrianInZone)
        {
            timer += Time.deltaTime;

            if (timer >= requiredWaitTime)
            {
                trafficLight.SetRed();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    public void SetPedestrianInZone(bool value)
    {
        pedestrianInZone = value;

        // όταν φεύγει ο πεζός, επιστρέφουμε πράσινο
        if (!value)
        {
            trafficLight.SetGreen();
            timer = 0f;
        }
    }
}