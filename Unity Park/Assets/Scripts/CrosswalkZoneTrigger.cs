using UnityEngine;

public class CrosswalkZoneTrigger : MonoBehaviour
{
    public CrosswalkController crosswalkController;

    private void OnTriggerExit(Collider other)
    {
        PedestrianAI ped = other.GetComponent<PedestrianAI>();

        if (ped != null)
        {
            crosswalkController.RemoveCrossingPedestrian(ped);
        }
    }
}