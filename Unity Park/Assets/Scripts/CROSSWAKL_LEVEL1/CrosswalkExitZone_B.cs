using UnityEngine;

public class CrosswalkExitZone_B : MonoBehaviour
{
    public CrosswalkController_B crosswalkController;

    private void OnTriggerEnter(Collider other)
    {
        PedestrianAI ped = other.GetComponent<PedestrianAI>();

        if (ped != null)
        {
            crosswalkController.RemoveCrossingPedestrian(ped);
        }
    }
}