using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswalkController_B : MonoBehaviour
{
    [Header("Pedestrian Lists")]
    public List<PedestrianAI> waitingPedestrians = new List<PedestrianAI>();
    public List<PedestrianAI> crossingPedestrians = new List<PedestrianAI>();

    [Header("Timing")]
    public float defaultGreenTime = 5f;
    public float extraTimeForDisabled = 3f;
    public float extraTimeForPhone = 4f;
    public float extraTimeForCrowd = 2f;
    public float cooldownTime = 3f;

    [Header("Traffic Lights")]
    public GameObject carRed;
    public GameObject carGreen;
    public GameObject pedRed;
    public GameObject pedGreen;

    [Header("Player")]
    public bool playerWaiting = false;

    private bool isSequenceRunning = false;

    void Start()
    {
        SetCarsGreen();
    }

    void Update()
    {
        // Ξεκινάει ΜΟΝΟ όταν ο παίκτης είναι στο waiting zone
        if (!isSequenceRunning && playerWaiting)
        {
            StartCoroutine(CrosswalkSequence());
        }
    }

    IEnumerator CrosswalkSequence()
    {
        isSequenceRunning = true;

        Debug.Log("CARS RED - PEDESTRIANS GREEN");

        float dynamicTime = CalculateDynamicGreenTime();
        Debug.Log("Dynamic green time = " + dynamicTime);

        SetPedestriansGreen();

        foreach (PedestrianAI ped in waitingPedestrians)
        {
            if (ped != null)
            {
                ped.StartCrossing();

                if (!crossingPedestrians.Contains(ped))
                {
                    crossingPedestrians.Add(ped);
                }
            }
        }

        waitingPedestrians.Clear();

        yield return new WaitForSeconds(dynamicTime);

        Debug.Log("WAITING FOR PEDESTRIANS TO FINISH");

        while (crossingPedestrians.Count > 0)
        {
            crossingPedestrians.RemoveAll(p => p == null);
            yield return null;
        }

        if (playerWaiting)
        {
            Debug.Log("Player still waiting - keep pedestrians green");
            isSequenceRunning = false;
            yield break;
        }

        ReturnCarsGreen();
    }

    public void ReturnCarsGreen()
    {
        if (crossingPedestrians.Count == 0)
        {
            Debug.Log("CARS GREEN - PEDESTRIANS RED");
            SetCarsGreen();
        }

        isSequenceRunning = false;
    }

    float CalculateDynamicGreenTime()
    {
        float totalTime = defaultGreenTime;

        if (waitingPedestrians.Count >= 5)
        {
            totalTime += extraTimeForCrowd;
        }

        foreach (PedestrianAI ped in waitingPedestrians)
        {
            if (ped == null) continue;

            if (ped.CompareTag("Ped_disabled"))
            {
                totalTime += extraTimeForDisabled;
            }

            if (ped.CompareTag("Ped_phone"))
            {
                totalTime += extraTimeForPhone;
            }
        }

        return totalTime;
    }

    public void AddWaitingPedestrian(PedestrianAI ped)
    {
        if (ped == null) return;

        if (!waitingPedestrians.Contains(ped) && !crossingPedestrians.Contains(ped))
        {
            waitingPedestrians.Add(ped);
            Debug.Log("Added waiting pedestrian: " + ped.name);
        }
    }

    public void RemoveCrossingPedestrian(PedestrianAI ped)
    {
        if (ped == null) return;

        if (crossingPedestrians.Contains(ped))
        {
            crossingPedestrians.Remove(ped);
            Debug.Log("Pedestrian left crosswalk: " + ped.name);
        }

        if (!playerWaiting && crossingPedestrians.Count == 0)
        {
            ReturnCarsGreen();
        }
    }

    void SetCarsGreen()
    {
        if (carGreen != null) carGreen.SetActive(true);
        if (carRed != null) carRed.SetActive(false);

        if (pedGreen != null) pedGreen.SetActive(false);
        if (pedRed != null) pedRed.SetActive(true);
    }

    void SetPedestriansGreen()
    {
        if (carGreen != null) carGreen.SetActive(false);
        if (carRed != null) carRed.SetActive(true);

        if (pedGreen != null) pedGreen.SetActive(true);
        if (pedRed != null) pedRed.SetActive(false);
    }
}