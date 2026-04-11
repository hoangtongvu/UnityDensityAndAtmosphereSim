using UnityEngine;

namespace Game.Mono.Player
{
    public class MouseLook : MonoBehaviour
    {
        public Transform playerBody;
        public float sensitivity = 200f;

        float xRotation = 0f;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        void Update()
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            // Pitch (up/down)
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Yaw (left/right)
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}