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
    [SourceGeneratorInjectable]
    public partial class SinkAndFloatHandler : SaiMonoBehaviour
    {
        private ISubscription changeFluidMessageSub;
        public FloatersHolder floatersHolder;
        public InspectingObjectIdHolder inspectingObjectId;

        [Inject] private FluidsSO fluidsSO;
        [Inject] private InspectingObjectsSO inspectingObjectsSO;
        [Inject] private WorldFluidId worldFluidId;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.floatersHolder);
            this.LoadComponentInCtrl(out this.inspectingObjectId);
        }

        private void Start()
        {
            this.InitValues(worldFluidId);
        }

        private void OnEnable()
        {
            this.changeFluidMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeFluidKindMessage>(message => this.InitValues(message.FluidId));
        }

        private void OnDisable()
        {
            this.changeFluidMessageSub.Dispose();
        }

        private void InitValues(WorldFluidId fluidId)
        {
            var objectData = this.inspectingObjectsSO.Value[this.inspectingObjectId.Value];
            var fluidData = this.fluidsSO.Value[fluidId.Value];

            if (objectData.Density < fluidData.Density)
            {
                this.floatersHolder.SetDisplacementAmt(1f);
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