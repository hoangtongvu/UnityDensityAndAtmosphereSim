using SaintsField;
using UnityEngine;

namespace Game.Domain
{
    public class InspectingObjectHullScanPositions : MonoBehaviour
    {
        [PositionHandle, DrawLabel] public Vector3 LowerHullScanStartPos;
        [PositionHandle, DrawLabel] public Vector3 LowerHullScanEndPos;
    }
}
