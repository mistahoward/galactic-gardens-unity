using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 1.5f, -2f);
    public float damping = 5f;
    public float rotationSpeed = 100f;
    public float decayFactor = 0.9f;
    public LayerMask obstacleLayers;  // Set this in inspector to include layers which should block the camera
    public float characterFollowRotationSpeed = 2f; // Speed at which camera follows the player's rotation
    public float offsetFactor = 1.0f; // The strength of the offset. You can adjust this as needed

    private float x = 0f;
    private float y = 0f;
    private float currentXSpeed = 0f;
    private float currentYSpeed = 0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    // LateUpdate is called once per frame, but after all Update functions have been called
    void LateUpdate()
    {
        if (target)
        {
            // Calculate input with decay factor
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float horizontalInput = Input.GetAxis("Horizontal"); // Assuming this is the axis that controls character turning

            if (Mathf.Abs(mouseX) > 0.01f)
            {
                currentXSpeed = mouseX * rotationSpeed * Time.deltaTime;
            }
            else
            {
                currentXSpeed *= decayFactor;
            }

            if (Mathf.Abs(mouseY) > 0.01f)
            {
                currentYSpeed = mouseY * rotationSpeed * Time.deltaTime;
            }
            else
            {
                currentYSpeed *= decayFactor;
            }

            // Gradually align camera rotation with target rotation
            x = Mathf.LerpAngle(x, target.rotation.eulerAngles.y, Time.deltaTime * characterFollowRotationSpeed);
            // Apply an offset based on the character's turning direction
            x += horizontalInput * offsetFactor;

            x += currentXSpeed;
            y -= currentYSpeed;

            // Clamp the vertical angle within 0 to 50 degree range
            y = Mathf.Clamp(y, 0f, 50f);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 direction = rotation * offset;
            Vector3 desiredPosition = target.position + direction;

            // Check if view is obstructed
            if (Physics.Raycast(target.position, direction, out RaycastHit hit, direction.magnitude, obstacleLayers))
            {
                desiredPosition = hit.point;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        }
    }
}
