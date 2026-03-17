using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswalkController : MonoBehaviour
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

    private bool isSequenceRunning = false;

    void Update()
    {
        if (!isSequenceRunning && waitingPedestrians.Count > 0)
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

        Debug.Log("CARS GREEN - PEDESTRIANS RED");

        yield return new WaitForSeconds(cooldownTime);

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
        if (!waitingPedestrians.Contains(ped))
        {
            waitingPedestrians.Add(ped);
            Debug.Log("Added waiting pedestrian: " + ped.name);
        }
    }

    public void RemoveCrossingPedestrian(PedestrianAI ped)
    {
        if (crossingPedestrians.Contains(ped))
        {
            crossingPedestrians.Remove(ped);
            Debug.Log("Pedestrian left crosswalk: " + ped.name);
        }
    }
}