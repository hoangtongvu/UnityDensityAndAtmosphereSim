using Game.Common;
using Game.Domain.WorldFluid;
using Game.ScriptableObjects;
using Reflex.Core;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Game.Mono.WorldFluid
{
    public class WorldFluidInstaller : SaiMonoBehaviour, IInstaller
    {
        [SerializeField] private FluidsSO fluidsSO;
        [SerializeField] private WaterSurface waterSurface;
        [SerializeField] private WorldFluidId worldFluidId;

        protected override void LoadComponents()
        {
            base.LoadComponents();
            this.LoadComponentInCtrl(out this.waterSurface);
        }

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.fluidsSO);
            builder.RegisterValue(this.waterSurface);
            builder.RegisterValue(this.worldFluidId);
        }
    }
}