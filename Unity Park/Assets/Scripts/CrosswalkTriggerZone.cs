using UnityEngine;

public class CrosswalkTriggerZone : MonoBehaviour
{
    public CrosswalkGlowController glowController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            glowController.CarEntered();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            glowController.CarExited();
        }
    }
}