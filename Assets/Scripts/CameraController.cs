using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new(0f, 1.5f, -5f);
    public float damping = 2f;
    public float rotationSpeed = 300f;
    public float decayFactor = 0.9f;
    public LayerMask obstacleLayers;
    public float characterFollowRotationSpeed = 10f;
    public float offsetFactor = 100f;

    private float x = 0f;
    private float y = 0f;
    private float currentXSpeed = 0f;
    private float currentYSpeed = 0f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        SetTarget(null); // Set initial target as null
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    void LateUpdate()
    {
        if (target)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            float horizontalInput = Input.GetAxis("Horizontal");

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

            // Gradually rotate the camera based on the character's rotation and horizontal input
            x = Mathf.LerpAngle(x, target.rotation.eulerAngles.y + (horizontalInput * offsetFactor), Time.deltaTime * characterFollowRotationSpeed);

            x += currentXSpeed;
            y -= currentYSpeed;

            y = Mathf.Clamp(y, 0f, 50f);

            Quaternion rotation = Quaternion.Euler(y - 18.5f, x, 0);
            Vector3 direction = rotation * offset;
            Vector3 desiredPosition = target.position + direction;

            if (Physics.Raycast(target.position, direction, out RaycastHit hit, direction.magnitude, obstacleLayers))
            {
                desiredPosition = hit.point;
            }

            y = Mathf.Clamp(y, -50f, 50f); // This prevents the camera from flipping over

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        }
    }
}
