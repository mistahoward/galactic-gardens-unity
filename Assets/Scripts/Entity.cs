using UnityEngine;

public class Entity : MonoBehaviour
{
    public float speed = 600.0f;
    public float turnSpeed = 400.0f;
    public float rotationSpeed = 10.0f;
    public float gravity = -20.0f;
    public float jumpHeight = 2.0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity = Vector3.zero;
    private bool isGrounded;
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }
}
