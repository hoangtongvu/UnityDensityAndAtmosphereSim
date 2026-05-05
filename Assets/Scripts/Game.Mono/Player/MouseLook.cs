using Game.Domain.Player;
using Game.Domain.PubSub.Messengers;
using UnityEngine;
using ZBase.Foundation.PubSub;

namespace Game.Mono.Player
{
    public class MouseLook : MonoBehaviour
    {
        public Transform playerBody;
        public float sensitivity = 200f;

        float xRotation = 0f;

        private bool isEnabled = true;
        private ISubscription setEnabledMouseLookMessageSub;
        private ISubscription setLockCursorMessageSub;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            setEnabledMouseLookMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<SetEnabledMouseLookMessage>(message => this.isEnabled = message.Value);

            setLockCursorMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<SetLockCursorMessage>(message => Cursor.lockState = message.LockMode);
        }

        private void OnDisable()
        {
            setEnabledMouseLookMessageSub.Dispose();
            setLockCursorMessageSub.Dispose();
        }

        void Update()
        {
            if (!isEnabled) return;

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