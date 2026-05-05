using Reflex.Core;
using UnityEngine;

namespace Game.Mono.Player
{
    public class PlayerCtrl : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.RegisterValue(this);
        }
    }
}