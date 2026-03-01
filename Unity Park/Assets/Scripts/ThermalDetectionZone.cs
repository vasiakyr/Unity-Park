using UnityEngine;

public class ThermalDetectionZone : MonoBehaviour
{
    public FogNode fogNode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fogNode.SetPedestrianInZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fogNode.SetPedestrianInZone(false);
        }
    }
}