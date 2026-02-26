using UnityEngine;

public class Car3PointsStop : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;

    public float speed = 6f;
    public float turnSpeed = 6f;
    public float arriveDistance = 0.5f;

    private int currentTargetIndex = 0;
    private Transform currentTarget;
    private bool hasStopped = false;

    void Start()
    {
        currentTarget = point1;
    }

    void Update()
    {
        if (hasStopped) return; // Αν έφτασε στο 3, σταματάει

        if (currentTarget == null) return;

        // Κρατάμε την κίνηση σε επίπεδο (ίδιο Y)
        Vector3 targetPos = currentTarget.position;
        targetPos.y = transform.position.y;

        // Ομαλή στροφή προς τον στόχο
        Vector3 dir = (targetPos - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, turnSpeed * Time.deltaTime);
        }

        // Κίνηση
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // Έλεγχος αν έφτασε
        if (Vector3.Distance(transform.position, targetPos) <= arriveDistance)
        {
            currentTargetIndex++;

            if (currentTargetIndex == 1)
                currentTarget = point2;

            else if (currentTargetIndex == 2)
                currentTarget = point3;

            else if (currentTargetIndex >= 3)
            {
                hasStopped = true;
                Debug.Log("Car stopped at Point 3");
            }
        }
    }
}