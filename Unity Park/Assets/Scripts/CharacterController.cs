using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float gravity = -9.81f;

    [Header("Look")]
    public float lookSpeed = 2.0f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private CharacterController _cc;
    private float _pitch;
    private Vector3 _velocity;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();

        if (cameraTransform == null)
        {
            var cam = GetComponentInChildren<Camera>();
            if (cam != null) cameraTransform = cam.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMove();
    }

    private void HandleLook()
    {
        float mx = Input.GetAxis("Mouse X") * lookSpeed;
        float my = Input.GetAxis("Mouse Y") * lookSpeed;

        // γύρνα σώμα δεξιά/αριστερά
        transform.Rotate(0f, mx, 0f);

        // γύρνα κάμερα πάνω/κάτω
        _pitch -= my;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void HandleMove()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        Vector3 move = (transform.right * x + transform.forward * z) * moveSpeed;

        // gravity
        if (_cc.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;

        _cc.Move((move + _velocity) * Time.deltaTime);
    }
}