using UnityEngine;
using UnityEngine.InputSystem;

public class StickTurnBody : MonoBehaviour
{
    public InputActionReference moveAction;
    public Transform xrOrigin;
    public float turnSpeed = 90f;
    public float deadZone = 0.2f;

    void OnEnable()
    {
        if (moveAction != null)
            moveAction.action.Enable();
    }

    void Update()
    {
        if (moveAction == null || xrOrigin == null) return;

        Vector2 input = moveAction.action.ReadValue<Vector2>();

        // Δεξιά - Αριστερά του αριστερού stick
        float horizontal = input.x;

        if (Mathf.Abs(horizontal) > deadZone)
        {
            xrOrigin.Rotate(0f, horizontal * turnSpeed * Time.deltaTime, 0f);
        }
    }
}