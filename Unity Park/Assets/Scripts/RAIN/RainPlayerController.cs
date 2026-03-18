using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RainPlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public WeatherManager weatherManager; // <- σύρε εδώ το WeatherManager από Inspector

    [Header("Movement Speeds")]
    public float normalSpeed = 3.5f;
    public float slowSpeed = 2.0f;     // Y = πιο αργά (προσεκτικά)
    public float sprintSpeed = 5.5f;   // X = πιο γρήγορα (προαιρετικό)

    [Header("Rain / Slip")]
    [Range(0.1f, 1f)] public float rainSpeedMultiplier = 0.7f; // όταν βρέχει, κόβει speed
    [Tooltip("Μικρότερο = πιο πολύ πάγος. Δοκίμασε 3-10")]
    public float slipResponsiveness = 6f; // πόσο γρήγορα “ακολουθεί” το input
    [Tooltip("Πόση αστάθεια στο περπάτημα όταν βρέχει")]
    public float rainWobble = 0.12f;

    [Header("Gravity")]
    public float gravity = -9.81f;

    [Header("Look")]
    public float lookSpeed = 2.0f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private CharacterController _cc;
    private float _pitch;
    private Vector3 _velocity;

    // γλιστρημα: κρατάμε move που “αργεί” να αλλάξει
    private Vector3 _slipMove;

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

        transform.Rotate(0f, mx, 0f);

        _pitch -= my;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

        if (cameraTransform != null)
            cameraTransform.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }

    private void HandleMove()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        // 1) Κατεύθυνση input (χωρίς speed ακόμα)
        Vector3 desired = (transform.right * x + transform.forward * z);
        desired = Vector3.ClampMagnitude(desired, 1f);

        // 2) Speed επιλογή με κουμπιά
        float speed = normalSpeed;

        if (Input.GetKey(KeyCode.Y))
            speed = slowSpeed;       // προσεκτικό περπάτημα
        else if (Input.GetKey(KeyCode.X))
            speed = sprintSpeed;     // προαιρετικό sprint

        // 3) Βροχή;
        bool raining = (weatherManager != null && weatherManager.IsRaining);

        if (raining)
        {
            // κόβει ταχύτητα στην βροχή
            speed *= rainSpeedMultiplier;

            // αστάθεια (προαιρετικό) για να “νιώθει” γλιστερό
            desired += new Vector3(
                Random.Range(-rainWobble, rainWobble),
                0f,
                Random.Range(-rainWobble, rainWobble)
            );
            desired = Vector3.ClampMagnitude(desired, 1f);

            // γλίστρημα: αργή αλλαγή κατεύθυνσης
            _slipMove = Vector3.Lerp(_slipMove, desired, Time.deltaTime * slipResponsiveness);
        }
        else
        {
            // στεγνό: άμεση απόκριση
            _slipMove = desired;
        }

        Vector3 move = _slipMove * speed;

        // 4) gravity
        if (_cc.isGrounded && _velocity.y < 0f)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;

        _cc.Move((move + _velocity) * Time.deltaTime);
    }
}