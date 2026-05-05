using Game.Domain;
using SaintsField;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FluidsSO", menuName = "SO/FluidsSO")]
    public class FluidsSO : ScriptableObject
    {
        public SaintsDictionary<int, FluidData> Value;
    }
}