using UnityEngine;

namespace Game.Mono.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        public float walkSpeed = 5f;
        public float sprintSpeed = 10f;
        public float verticalSpeed = 5f;
        public float gravity = -9.81f;

        CharacterController controller;
        Vector3 velocity;

        void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            // --- Speed (Shift to sprint)
            float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

            // --- Horizontal movement
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            // --- Vertical movement (Space / Ctrl)
            float yInput = 0f;
            if (Input.GetKey(KeyCode.Space))
                yInput = 1f;
            else if (Input.GetKey(KeyCode.LeftControl))
                yInput = -1f;

            Vector3 verticalMove = Vector3.up * yInput * verticalSpeed;

            // --- Apply movement
            controller.Move((move * currentSpeed + verticalMove) * Time.deltaTime);

            // --- Optional gravity (only if NOT flying)
            if (yInput == 0)
            {
                if (controller.isGrounded && velocity.y < 0)
                    velocity.y = -2f;

                velocity.y += gravity * Time.deltaTime;
                controller.Move(velocity * Time.deltaTime);
            }
            else
            {
                velocity.y = 0; // cancel gravity when flying
            }
        }
    }
}