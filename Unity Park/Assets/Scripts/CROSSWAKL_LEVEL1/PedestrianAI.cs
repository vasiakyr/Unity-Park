using UnityEngine;

public enum PedestrianType
{
    Normal,
    Disabled,
    Phone
}

public class PedestrianAI : MonoBehaviour
{
    public PedestrianType pedestrianType;

    public Transform targetPoint;
    public float moveSpeed = 2f;

    [Header("Phone Pedestrian Settings")]
    public float zigzagAmount = 0.5f;
    public float zigzagSpeed = 2f;

    [Header("Avoidance Settings")]
    public float avoidRadius = 1.2f;
    public float avoidStrength = 1.5f;
    public LayerMask pedestrianLayer;

    private bool canMove = false;

    void Start()
    {
        SetupPedestrian();
    }

    void Update()
    {
        if (!canMove || targetPoint == null) return;

        MovePedestrian();
    }

    void SetupPedestrian()
    {
        switch (pedestrianType)
        {
            case PedestrianType.Normal:
                moveSpeed = 2f;
                gameObject.tag = "Ped_normal";
                break;

            case PedestrianType.Disabled:
                moveSpeed = 1.1f;
                gameObject.tag = "Ped_disabled";
                break;

            case PedestrianType.Phone:
                moveSpeed = 1.0f;
                gameObject.tag = "Ped_phone";
                break;
        }
    }

    void MovePedestrian()
    {
        Vector3 targetPos = targetPoint.position;

        Vector3 dirToTarget = (targetPos - transform.position).normalized;

        if (pedestrianType == PedestrianType.Phone)
        {
            float zigzag = Mathf.Sin(Time.time * zigzagSpeed) * zigzagAmount;
            Vector3 side = Vector3.Cross(dirToTarget, Vector3.up) * zigzag;
            targetPos += side;
            dirToTarget = (targetPos - transform.position).normalized;
        }

        Vector3 avoidDir = GetAvoidanceDirection();

        Vector3 finalDir = dirToTarget + avoidDir * avoidStrength;
        finalDir.y = 0f;
        finalDir.Normalize();

        transform.position += finalDir * moveSpeed * Time.deltaTime;

        if (finalDir != Vector3.zero)
        {
            transform.forward = finalDir;
        }
    }

    Vector3 GetAvoidanceDirection()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, avoidRadius, pedestrianLayer);

        Vector3 avoidance = Vector3.zero;

        foreach (Collider other in nearby)
        {
            if (other.gameObject == gameObject) continue;

            Vector3 diff = transform.position - other.transform.position;
            diff.y = 0f;

            float distance = diff.magnitude;

            if (distance > 0.01f)
            {
                avoidance += diff.normalized / distance;
            }
        }

        return avoidance;
    }

    public void StartCrossing()
    {
        canMove = true;
    }

    public void StopCrossing()
    {
        canMove = false;
    }
}