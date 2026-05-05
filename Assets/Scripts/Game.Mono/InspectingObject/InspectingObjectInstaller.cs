using Game.Common;
using Game.ScriptableObjects;
using Reflex.Core;
using UnityEngine;

namespace Game.Mono.InspectingObject
{
    public class InspectingObjectInstaller : SaiMonoBehaviour, IInstaller
    {
        [SerializeField] private InspectingObjectsSO inspectingObjectsSO;

        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this.inspectingObjectsSO);
        }
    }
}