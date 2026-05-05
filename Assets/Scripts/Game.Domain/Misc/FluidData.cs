using SaintsField;
using UnityEngine;

namespace Game.Domain;

[System.Serializable]
public class FluidData
{
    [Required] public string Name;
    [Required] public float Density;

    public Color FoamColor;
    [ColorUsage(false)] public Color RefractionColor;
    public float RefractionAbsorptionDistance;
    [ColorUsage(false)] public Color ScatteringColor;
}