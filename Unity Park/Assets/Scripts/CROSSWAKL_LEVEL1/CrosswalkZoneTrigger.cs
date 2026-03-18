using UnityEngine;

public class CrosswalkZoneTrigger : MonoBehaviour
{
    public CrosswalkController_B crosswalkController;

    private void OnTriggerExit(Collider other)
    {
        PedestrianAI ped = other.GetComponent<PedestrianAI>();

        if (ped != null)
        {
            crosswalkController.RemoveCrossingPedestrian(ped);
        }
    }
}