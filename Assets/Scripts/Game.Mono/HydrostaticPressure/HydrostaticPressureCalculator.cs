using Game.Domain.PubSub.Messengers;
using Game.Domain.WorldFluid;
using Game.ScriptableObjects;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using ZBase.Foundation.PubSub;

namespace Game.Mono.HydrostaticPressure
{
    [SourceGeneratorInjectable]
    public partial class HydrostaticPressureCalculator : MonoBehaviour
    {
        [Inject] private WaterSurface water;

        private WaterSearchParameters Search;
        private WaterSearchResult SearchResutt;

        [Inject] private FluidsSO fluidsSO;
        [Inject] private WorldFluidId worldFluidId;
        private ISubscription changeFluidMessageSub;

        private const float atmospherePressure = 101325; // Pascal
        private float fluidDensity; //kg/m^3
        public float depth; // meter
        public float pressure; // Pascal

        private void Start()
        {
            SetFluidDensity(worldFluidId);
        }

        private void OnEnable()
        {
            changeFluidMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeFluidKindMessage>(message => SetFluidDensity(message.FluidId));
        }

        private void OnDisable()
        {
            changeFluidMessageSub.Dispose();
        }

        private void SetFluidDensity(WorldFluidId id)
        {
            fluidDensity = fluidsSO.Value[id.Value].Density;
        }

        private void FixedUpdate()
        {
            Search.startPositionWS = transform.position;
            water.ProjectPointOnWaterSurface(Search, out SearchResutt);

            depth = SearchResutt.projectedPositionWS.y - transform.position.y;
            if (depth < 0)
                depth = 0;

            pressure = atmospherePressure + fluidDensity * Mathf.Abs(Physics.gravity.y) * depth;
        }
    }
}