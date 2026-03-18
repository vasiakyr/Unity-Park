using System.Collections;
using UnityEngine;

public class CrosswalkController_A : MonoBehaviour
{
    [Header("Pedestrians")]
    public PedestrianAI[] pedestrians;

    [Header("Timing")]
    public float waitBeforeCrossing = 4f;

    private bool hasStarted = false;

    void Start()
    {
        StartCoroutine(StartCrosswalkSequence());
    }

    IEnumerator StartCrosswalkSequence()
    {
        yield return new WaitForSeconds(waitBeforeCrossing);

        StartPedestrians();
    }

    void StartPedestrians()
    {
        if (hasStarted) return;

        hasStarted = true;

        foreach (PedestrianAI ped in pedestrians)
        {
            if (ped != null)
            {
                ped.StartCrossing();
            }
        }
    }
}