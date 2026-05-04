using Game.Common;
using Game.Domain;
using Game.Domain.PubSub.Messengers;
using Game.Domain.Utilities;
using Game.Domain.WorldFluid;
using Game.ScriptableObjects;
using Reflex.Attributes;
using ZBase.Foundation.PubSub;

namespace Game.Mono
{
    public class SinkAndFloatHandler : SaiMonoBehaviour
    {
        private ISubscription fluidChangedSubscription;
        public FloatersHolder floatersHolder;
        public InspectingObjectsSO inspectingObjectsSO;
        public InspectingObjectIdHolder inspectingObjectId;

        [Inject] public FluidsSO fluidsSO;
        [Inject] public WorldFluidId worldFluidId;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.floatersHolder);
            this.LoadComponentInCtrl(out this.inspectingObjectId);
        }

        private void Start()
        {
            this.InitValues();
        }

        private void OnEnable()
        {
            this.fluidChangedSubscription = GameplayMessenger.MessageSubscriber
                .Subscribe<FluidKindChangedMessage>(_ => this.InitValues());
        }

        private void OnDisable()
        {
            this.fluidChangedSubscription.Dispose();
        }

        private void InitValues()
        {
            var objectData = this.inspectingObjectsSO.Value[this.inspectingObjectId.Value];
            var fluidData = this.fluidsSO.Value[this.worldFluidId.Value];

            if (objectData.Density < fluidData.Density)
            {
                float submergedVolume = objectData.Weight / fluidData.Density;
                float submergedHeight = objectData.GetSubmergedHeight(submergedVolume, 0.01f, 50);
                this.floatersHolder.SetSubmergedHeight(submergedHeight * 2);
            }
            else if (objectData.Density == fluidData.Density)
            {
            }
            else
            {
                this.floatersHolder.SetDisplacementAmt(0f);
            }
        }
    }
}