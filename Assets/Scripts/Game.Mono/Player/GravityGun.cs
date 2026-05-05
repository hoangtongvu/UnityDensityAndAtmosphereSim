using SaintsField;
using UnityEngine;

namespace Game.Mono.Player
{
    public class GravityGun : MonoBehaviour
    {
        [Header("References")]
        public Camera cam;
        [PositionHandle, DrawLabel] public Vector3 laserStartPosOffset;
        public LineRenderer laser;

        [Header("Settings")]
        public float grabDistance = 30f;
        public float moveForce = 150f;
        public float maxVelocity = 10f;

        public float holdDistance = 5f;
        public float minHoldDistance = 2f;
        public float maxHoldDistance = 15f;

        public float shootForce = 30f;

        private Rigidbody grabbedRb;

        void Awake()
        {
            this.cam = Camera.main;
        }

        void Update()
        {
            HandleHoldDistance();

            if (Input.GetMouseButtonDown(0))
            {
                TryGrab();
            }

            if (Input.GetMouseButtonUp(0))
            {
                Release();
            }

            if (Input.GetMouseButtonDown(1))
            {
                Shoot();
            }
        }

        void FixedUpdate()
        {
            UpdateLaser();

            if (grabbedRb != null)
            {
                MoveObject();
            }
        }

        void HandleHoldDistance()
        {
            if (grabbedRb)
            {
                holdDistance += Input.mouseScrollDelta.y;
                holdDistance = Mathf.Clamp(holdDistance, minHoldDistance, maxHoldDistance);
            }
        }

        void UpdateLaser()
        {
            if (this.grabbedRb)
            {
                laser.SetPosition(0, transform.TransformPoint(laserStartPosOffset));
                laser.SetPosition(1, grabbedRb.transform.position);
            }
            else
            {
                laser.SetPosition(0, Vector3.zero);
                laser.SetPosition(1, Vector3.zero);
            }
        }

        void TryGrab()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, grabDistance))
            {
                Rigidbody rb = hit.rigidbody;

                if (rb != null)
                {
                    grabbedRb = rb;
                    grabbedRb.useGravity = false;
                    grabbedRb.linearDamping = 10f; // smooth
                }
            }
        }

        void MoveObject()
        {
            Vector3 targetPos = cam.transform.position + cam.transform.forward * holdDistance;
            Vector3 dir = targetPos - grabbedRb.position;

            Vector3 force = dir * moveForce;
            grabbedRb.AddForce(force, ForceMode.Acceleration);

            // Clamp velocity
            if (grabbedRb.linearVelocity.magnitude > maxVelocity)
            {
                grabbedRb.linearVelocity = grabbedRb.linearVelocity.normalized * maxVelocity;
            }
        }

        void Release()
        {
            if (grabbedRb != null)
            {
                grabbedRb.useGravity = true;
                grabbedRb.linearDamping = 0f;
                grabbedRb = null;
            }
        }

        void Shoot()
        {
            if (grabbedRb != null)
            {
                grabbedRb.useGravity = true;
                grabbedRb.linearDamping = 0f;
                grabbedRb.AddForce(cam.transform.forward * shootForce, ForceMode.Impulse);
                grabbedRb = null;
            }
        }
    }
}