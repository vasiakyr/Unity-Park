using UnityEngine;

public class PedestrianWaitingZone_B : MonoBehaviour
{
    public CrosswalkController_B crosswalkController;

    private void OnTriggerEnter(Collider other)
    {
        TryAddPedestrian(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryAddPedestrian(other);
    }

    void TryAddPedestrian(Collider other)
    {
        PedestrianAI ped = other.GetComponent<PedestrianAI>();

        if (ped != null)
        {
            crosswalkController.AddWaitingPedestrian(ped);
        }
    }
}