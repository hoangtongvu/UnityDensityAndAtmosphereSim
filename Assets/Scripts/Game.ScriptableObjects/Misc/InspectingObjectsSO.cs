using Game.Common.Meshing;
using Game.Domain;
using SaintsField;
using UnityEngine;

namespace Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InspectingObjectsSO", menuName = "SO/InspectingObjectsSO")]
    public class InspectingObjectsSO : ScriptableObject
    {
        public SaintsDictionary<int, InspectingObjectData> Value;

        private void OnValidate()
        {
            this.InitValues();
        }

        [ContextMenu("Init Values")]
        public void InitValues()
        {
            foreach (var data in this.Value)
            {
                var temp = data.Value;

                var inspectingObjectHullScanPositions = temp.Prefab.GetComponent<InspectingObjectHullScanPositions>();
                temp.LowerHullScanStartY = inspectingObjectHullScanPositions.LowerHullScanStartPos.y;
                temp.LowerHullScanEndY = inspectingObjectHullScanPositions.LowerHullScanEndPos.y;

                temp.MeshVolume = MeshVolumeHelper.VolumeOfMesh(temp.Prefab.GetComponent<MeshFilter>().sharedMesh);
                temp.Weight = temp.MeshVolume * temp.Density;
            }
        }
    }
}
