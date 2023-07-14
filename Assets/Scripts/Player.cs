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

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            // If the player is moving
            if (move.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

                move *= speed * Time.deltaTime;
                controller.Move(move);
            }
            // If the player is stationary but is pressing a direction key
            else if (x != 0 || z != 0)
            {
                // Just rotate towards the direction
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

            if (x != 0 || z != 0) // if there's any movement input
            {
                anim.SetInteger("AnimationPar", 1);
            }
            else
            {
                anim.SetInteger("AnimationPar", 0);
            }


            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                anim.SetTrigger("Jump");
            }
            anim.SetBool("IsGrounded", isGrounded);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

    }
}
