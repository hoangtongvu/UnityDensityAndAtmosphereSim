using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Game.Mono
{
    [SourceGeneratorInjectable]
    public partial class Floater : MonoBehaviour
    {
        // Rigidbody component of the floating object
        public Rigidbody rb;
        // Depth at which object starts to experience buoyancy
        public float depthBefSub;
        // Amount of buoyant force applied
        public float displacementAmt;
        // Number of points applying buoyant force
        public int floaters;
        // Drag coefficient in water
        public float waterDrag;
        // Angular drag coefficient in water
        public float waterAngutarDrag;
        // Reference to the water surface management component
        [Inject] public WaterSurface water;
        // Holds parameters for searching the water surface
        WaterSearchParameters Search;
        // Stores result of water surface search
        WaterSearchResult SearchResutt;

        private void FixedUpdate()
        {
            // Apply a distributed gravitational force
            rb.AddForceAtPosition(Physics.gravity / floaters, transform.position, ForceMode.Acceleration);
            // Set up search parameters for projecting on water surface
            Search.startPositionWS = transform.position;
            // Project point onto water surface and get result
            water.ProjectPointOnWaterSurface(Search, out SearchResutt);

            // If object is below the water surface
            if (transform.position.y < SearchResutt.projectedPositionWS.y)
            {
                // Calculate displacement multiplier based on submersion depth
                float displacementMutti = Mathf.Clamp01((SearchResutt.projectedPositionWS.y - transform.position.y) / depthBefSub) * displacementAmt;
                // Apply buoyant force upwards
                rb.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMutti, 0f), transform.position, ForceMode.Acceleration);
                // Apply water drag force against velocity
                rb.AddForce(displacementMutti * -rb.linearVelocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                // Apply water angular drag torque against angular velocity
                rb.AddTorque(displacementMutti * -rb.angularVelocity * waterAngutarDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }
}