using UnityEngine;

public class PedestrianWaitingZone : MonoBehaviour
{
    public CrosswalkController crosswalkController;

    private void OnTriggerEnter(Collider other)
    {
        PedestrianAI ped = other.GetComponent<PedestrianAI>();

        if (ped != null)
        {
            crosswalkController.AddWaitingPedestrian(ped);
        }
    }
}