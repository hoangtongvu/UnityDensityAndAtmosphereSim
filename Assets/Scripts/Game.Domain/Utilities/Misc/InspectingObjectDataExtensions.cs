using EzySlice;
using Unity.Mathematics;
using UnityEngine;
using static Game.Common.Meshing.MeshVolumeHelper;

namespace Game.Domain.Utilities;

public static class InspectingObjectDataExtensions
{
    public static float GetSubmergedHeight(
        this InspectingObjectData data,
        float submergedVolume,
        float epsilon,
        float maxIterationCount)
    {
        float low = data.LowerHullScanStartY;
        float high = data.LowerHullScanEndY;

        float bestHeight = low;

        for (int i = 0; i < maxIterationCount; i++)
        {
            float mid = (low + high) / 2;

            // Move cutter to mid
            var pos = new Vector3(0f, mid, 0f);

            var sliceHull = data.Prefab.Slice(
                pos,
                data.Prefab.transform.up);

            if (sliceHull == null)
            {
                // Treat as invalid → shrink range
                high = mid;
                continue;
            }

            float volume = VolumeOfMesh(sliceHull.lowerHull);
            float delta = volume - submergedVolume;

            // Found good enough
            if (math.abs(delta) <= epsilon)
                return mid - data.LowerHullScanStartY;

            if (volume < submergedVolume)
            {
                low = mid; // Need MORE volume → go UP
            }
            else
            {
                high = mid; // Too much volume → go DOWN
            }

            bestHeight = mid;
        }

        return bestHeight - data.LowerHullScanStartY;
    }
}