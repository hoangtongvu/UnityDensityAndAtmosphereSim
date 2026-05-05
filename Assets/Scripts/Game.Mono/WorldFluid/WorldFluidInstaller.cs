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
                .Subscribe<ChangeFluidKindMessage>(ChangeFluid);
        }

        private void OnDisable()
        {
            this.changeFluidKindMessageSub.Dispose();
        }

        private void ChangeFluid(ChangeFluidKindMessage message)
        {
            this.worldFluidId.Value = message.FluidId.Value;
            var fluidData = this.fluidsSO.Value[this.worldFluidId.Value];

            this.waterSurface.foamColor = fluidData.FoamColor;
            this.waterSurface.refractionColor = fluidData.RefractionColor.linear;
            this.waterSurface.absorptionDistance = fluidData.RefractionAbsorptionDistance;
            this.waterSurface.scatteringColor = fluidData.ScatteringColor.linear;
        }

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.fluidsSO);
            builder.RegisterValue(this.waterSurface);
            builder.RegisterValue(this.worldFluidId);
        }
    }
}