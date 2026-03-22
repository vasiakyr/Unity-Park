using UnityEngine;

public class PlayerWaitingZone_B : MonoBehaviour
{
    public CrosswalkController_B crosswalkController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crosswalkController.playerWaiting = true;
            Debug.Log("Player entered waiting zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crosswalkController.playerWaiting = false;
            Debug.Log("Player left waiting zone");

            if (crosswalkController.crossingPedestrians.Count == 0)
            {
                crosswalkController.ReturnCarsGreen();
            }
        }
    }
}