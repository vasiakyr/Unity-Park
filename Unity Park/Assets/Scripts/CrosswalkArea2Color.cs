using UnityEngine;

public class CrosswalkArea2Color : MonoBehaviour
{
    public TrafficLight2Color trafficLight;

    [Header("Extra time by pedestrian type")]
    public float extraDisabled = 3f;
    public float extraPhone = 4f;

    private void OnTriggerEnter(Collider other)
    {
        if (!trafficLight) return;

        float extra = 0f;

        if (other.CompareTag("Ped_disabled"))
            extra = extraDisabled;
        else if (other.CompareTag("Ped_phone"))
            extra = extraPhone;

        trafficLight.RequestCrossing(extra);
    }
}