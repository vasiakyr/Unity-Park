using UnityEngine;

public class CarPatrol : MonoBehaviour
{
    [Header("Waypoints (βάλε εδώ τα 4 σημεία σου με σειρά)")]
    public Transform[] waypoints;

    [Header("Movement")]
    public float speed = 5f;
    public float turnSpeed = 8f;
    public float arriveDistance = 0.8f;

    [Header("Traffic")]
    public TrafficLight trafficLight;

    private int currentIndex = 0;
    private bool isInStopLine = false;

    private void Start()
    {
        // Ασφάλεια: αν δεν έχεις βάλει waypoints
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("CarPatrol: Χρειάζονται τουλάχιστον 2 waypoints!");
        }
    }

    private void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        // STOP μόνο αν: είμαι στη γραμμή στάσης ΚΑΙ το φανάρι είναι κόκκινο
        if (isInStopLine && trafficLight != null && trafficLight.IsRed)
            return;

        Transform target = waypoints[currentIndex];
        if (target == null) return;

        Vector3 dir = (target.position - transform.position);
        dir.y = 0f;

        // Αν έφτασα στο waypoint -> πάω στο επόμενο
        if (dir.magnitude <= arriveDistance)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
            return;
        }

        // στρίβει προς τον στόχο
        if (dir.sqrMagnitude > 0.0001f)
        {
            Quaternion rot = Quaternion.LookRotation(dir.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, turnSpeed * Time.deltaTime);
        }

        // προχωράει μπροστά
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void SetInStopLine(bool value)
    {
        isInStopLine = value;
    }
}