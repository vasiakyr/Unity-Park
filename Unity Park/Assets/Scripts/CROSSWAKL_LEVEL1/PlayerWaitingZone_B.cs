using UnityEngine;

public class PlayerWaitingZone_B : MonoBehaviour
{
    public CrosswalkController_B crosswalkController;

    private int aiInsideCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crosswalkController.playerWaiting = true;
            Debug.Log("Player entered waiting zone B");
            return;
        }

        PedestrianAI ped = other.GetComponent<PedestrianAI>();
        if (ped != null)
        {
            aiInsideCount++;
            crosswalkController.AddWaitingPedestrian(ped);
            Debug.Log("AI entered waiting zone B: " + ped.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            crosswalkController.playerWaiting = false;
            Debug.Log("Player exited waiting zone B");
        }

        PedestrianAI ped = other.GetComponent<PedestrianAI>();
        if (ped != null)
        {
            aiInsideCount--;
            if (aiInsideCount < 0) aiInsideCount = 0;

            crosswalkController.RemoveWaitingPedestrian(ped);
            Debug.Log("AI exited waiting zone B: " + ped.name);
        }

        // Αν δεν είναι κανείς μέσα στο waiting zone -> κόκκινο για πεζούς
        if (!crosswalkController.playerWaiting && aiInsideCount == 0)
        {
            crosswalkController.ReturnCarsGreen();
            Debug.Log("Waiting zone empty -> pedestrians red");
        }
    }
}