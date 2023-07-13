using UnityEngine;

namespace Player
{
    public class Player : Entity
    {
        private Animator anim;
        private CharacterController controller;

        private Vector3 moveDirection = Vector3.zero;

        protected override void Start()
        {
            base.Start();
            controller = GetComponent<CharacterController>();
            anim = gameObject.GetComponentInChildren<Animator>();
        }

        protected override void Update()
        {
            base.Update();

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Get horizontal and vertical input (automatically works with WASD, arrow keys, and joystick)
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // Create a direction vector from the input
            Vector3 move = transform.right * x + transform.forward * z;

            if (move != Vector3.zero)
            {
                // Calculate the target rotation based on the move direction
                Quaternion targetRotation = Quaternion.LookRotation(move);

                // Smoothly rotate the player towards the target rotation at a certain speed
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            // Apply speed to the direction vector
            move *= speed * Time.deltaTime;

            // Move the player
            controller.Move(move);

            // Animation
            if (z != 0) // if there's any forward movement
            {
                anim.SetInteger("AnimationPar", 1);
            }
            else
            {
                anim.SetInteger("AnimationPar", 0);
            }


            // Jumping
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                // Apply upward force
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                // Trigger JumpStart animation
                anim.SetTrigger("Jump");
            }
            // Update IsGrounded parameter
            anim.SetBool("IsGrounded", isGrounded);

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;

            // Move the player vertically
            controller.Move(velocity * Time.deltaTime);
        }
    }
}
