using Game.Common;
using Game.Domain;
using Game.Domain.PubSub.Messengers;
using Game.Domain.Utilities;
using Game.ScriptableObjects;
using ZBase.Foundation.PubSub;

namespace Game.Mono
{
    public class SinkAndFloatHandler : SaiMonoBehaviour
    {
        private ISubscription fluidChangedSubscription;
        public FloatersHolder floatersHolder;
        public InspectingObjectsSO inspectingObjectsSO;
        public InspectingObjectIdHolder inspectingObjectId;

        public FluidsSO fluidsSO;
        public WorldFluidIdHolder worldFluidId;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.floatersHolder);
            this.LoadComponentInCtrl(out this.inspectingObjectId);
            this.FindAnyObjectByType(out this.worldFluidId);
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