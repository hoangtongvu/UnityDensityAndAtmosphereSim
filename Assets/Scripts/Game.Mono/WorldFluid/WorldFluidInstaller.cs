using Game.Common;
using Game.Domain.PubSub.Messengers;
using Game.Domain.WorldFluid;
using Game.ScriptableObjects;
using Reflex.Core;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using ZBase.Foundation.PubSub;

namespace Game.Mono.WorldFluid
{
    public class WorldFluidInstaller : SaiMonoBehaviour, IInstaller
    {
        [SerializeField] private FluidsSO fluidsSO;
        [SerializeField] private WaterSurface waterSurface;
        [SerializeField] private WorldFluidId worldFluidId;
        private ISubscription changeFluidKindMessageSub;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.waterSurface);
        }

        private void OnEnable()
        {
            this.changeFluidKindMessageSub = GameplayMessenger.MessageSubscriber
                .Subscribe<ChangeFluidKindMessage>(message => this.worldFluidId.Value = message.FluidId.Value);
        }

        private void OnDisable()
        {
            this.changeFluidKindMessageSub.Dispose();
        }

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.fluidsSO);
            builder.RegisterValue(this.waterSurface);
            builder.RegisterValue(this.worldFluidId);
        }
    }
}