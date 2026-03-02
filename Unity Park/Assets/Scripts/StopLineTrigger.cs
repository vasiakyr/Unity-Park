using UnityEngine;

public class StopLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        CarPatrol car = other.GetComponentInParent<CarPatrol>();
        if (car != null) car.SetInStopLine(true);
    }

    private void OnTriggerExit(Collider other)
    {
        CarPatrol car = other.GetComponentInParent<CarPatrol>();
        if (car != null) car.SetInStopLine(false);
    }
}