using UnityEngine;

public class CrosswalkDetector : MonoBehaviour
{
    public TrafficLightController trafficLight;

    [Header("Extra time by pedestrian type")]
    public float extraDisabled = 3f;
    public float extraPhone = 4f;

    void OnTriggerEnter(Collider other)
    {
        if (trafficLight == null) return;

        if (other.CompareTag("Ped_normal"))
        {
            trafficLight.RequestCrossing(0f);
            Debug.Log("ENTER crosswalk: Ped_normal");
        }
        else if (other.CompareTag("Ped_disabled"))
        {
            trafficLight.RequestCrossing(extraDisabled);
            Debug.Log("ENTER crosswalk: Ped_disabled");
        }
        else if (other.CompareTag("Ped_phone"))
        {
            trafficLight.RequestCrossing(extraPhone);
            Debug.Log("ENTER crosswalk: Ped_phone");
        }
    }
}