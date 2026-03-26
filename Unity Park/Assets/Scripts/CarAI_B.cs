using System.Collections.Generic;
using UnityEngine;

public class CarAI_B : MonoBehaviour
{
    [Header("Path")]
    public List<Transform> waypoints = new List<Transform>();
    public int currentWaypointIndex = 0;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float waypointReachDistance = 0.3f;

    [Header("Traffic Light")]
    public CrosswalkController_B crosswalkController;
    public Transform stopPoint;
    public float stopDistance = 1.5f;

    private bool isWaitingAtRed = false;

    void Update()
    {
        if (waypoints.Count == 0) return;
        if (currentWaypointIndex >= waypoints.Count) return;

        HandleTrafficLight();
        MoveAlongPath();
    }

    void HandleTrafficLight()
    {
        if (crosswalkController == null || stopPoint == null) return;

        float distanceToStopPoint = Vector3.Distance(transform.position, stopPoint.position);

        bool pedestriansGreen =
            crosswalkController.pedGreen != null &&
            crosswalkController.pedGreen.activeSelf;

        // Αν πλησιάζει στο stop point και το πεζών είναι πράσινο,
        // το αμάξι πρέπει να σταματήσει
        if (distanceToStopPoint <= stopDistance && pedestriansGreen)
        {
            isWaitingAtRed = true;
        }

        // Αν το πεζών γίνει κόκκινο, το αμάξι ξαναξεκινά
        if (!pedestriansGreen)
        {
            isWaitingAtRed = false;
        }
    }

    void MoveAlongPath()
    {
        if (isWaitingAtRed) return;

        Transform target = waypoints[currentWaypointIndex];
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= waypointReachDistance)
        {
            currentWaypointIndex++;

            // αν θες να κάνει loop
            if (currentWaypointIndex >= waypoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }
}