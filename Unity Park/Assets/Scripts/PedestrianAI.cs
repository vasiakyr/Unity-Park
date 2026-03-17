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

        if (pedestrianType == PedestrianType.Phone)
        {
            float zigzag = Mathf.Sin(Time.time * zigzagSpeed) * zigzagAmount;
            Vector3 dir = (targetPoint.position - transform.position).normalized;
            Vector3 side = Vector3.Cross(dir, Vector3.up) * zigzag;
            targetPos += side;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        Vector3 lookDir = targetPos - transform.position;
        lookDir.y = 0f;

        if (lookDir != Vector3.zero)
        {
            transform.forward = lookDir;
        }
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