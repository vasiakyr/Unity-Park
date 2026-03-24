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

    [Header("Traffic Lights")]
    public GameObject carRed;
    public GameObject carGreen;
    public GameObject pedRed;
    public GameObject pedGreen;

    [Header("Player")]
    public bool playerWaiting = false;

    private bool isSequenceRunning = false;
    private bool pedestriansGreenActive = false;

    void Start()
    {
        SetCarsGreen();
    }

    void Update()
    {
        CleanLists();

        // ΜΟΝΟ όταν μπει ο παίκτης στο waiting zone
        if (playerWaiting && !pedestriansGreenActive && !isSequenceRunning)
        {
            StartCoroutine(CrosswalkSequence());
        }

        // Αν είναι ήδη πράσινο και μπουν κι άλλοι waiting AI, ξεκίνα τους
        if (pedestriansGreenActive && waitingPedestrians.Count > 0)
        {
            StartWaitingPedestrians();
        }

        // Μόλις φύγει ο παίκτης από το waiting zone -> αμέσως κόκκινο για πεζούς
        if (!playerWaiting && pedestriansGreenActive)
        {
            ReturnCarsGreen();
        }
    }

    IEnumerator CrosswalkSequence()
    {
        isSequenceRunning = true;
        pedestriansGreenActive = true;

        Debug.Log("PLAYER DETECTED -> CARS RED - PEDESTRIANS GREEN");
        SetPedestriansGreen();

        float dynamicTime = CalculateDynamicGreenTime();
        Debug.Log("Dynamic green time = " + dynamicTime);

        // Ξεκινάνε οι AI που ήδη περιμένουν
        StartWaitingPedestrians();

        yield return new WaitForSeconds(dynamicTime);

        isSequenceRunning = false;
    }

    void StartWaitingPedestrians()
    {
        for (int i = waitingPedestrians.Count - 1; i >= 0; i--)
        {
            PedestrianAI ped = waitingPedestrians[i];

            if (ped == null)
            {
                waitingPedestrians.RemoveAt(i);
                continue;
            }

            ped.StartCrossing();

            if (!crossingPedestrians.Contains(ped))
            {
                crossingPedestrians.Add(ped);
            }

            waitingPedestrians.RemoveAt(i);
        }
    }

    void CleanLists()
    {
        waitingPedestrians.RemoveAll(p => p == null);
        crossingPedestrians.RemoveAll(p => p == null);
    }

    public void ReturnCarsGreen()
    {
        Debug.Log("NO PLAYER IN WAITING ZONE -> CARS GREEN - PEDESTRIANS RED");
        SetCarsGreen();

        pedestriansGreenActive = false;
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

    public void RemoveWaitingPedestrian(PedestrianAI ped)
    {
        if (ped == null) return;

        if (waitingPedestrians.Contains(ped))
        {
            waitingPedestrians.Remove(ped);
            Debug.Log("Removed waiting pedestrian: " + ped.name);
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