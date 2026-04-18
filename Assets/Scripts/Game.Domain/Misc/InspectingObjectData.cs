using SaintsField;
using UnityEngine;

namespace Game.Domain;

[System.Serializable]
public class InspectingObjectData
{
    [Required] public string Name;
    [Required, AssetPreview] public GameObject Prefab;
    [Required] public float Density; // kg/m^3
    public float LowerHullScanStartY;
    public float LowerHullScanEndY;
    public float MeshVolume;
    public float Weight;
}
