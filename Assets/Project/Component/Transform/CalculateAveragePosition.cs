using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class CalculateAveragePosition 
{
    public static Vector3 WeightedAverage(List<Vector3> positions, List<float> weights)
    {
        if (positions.Count != weights.Count)
        {
            Debug.LogError("Positions and weights count mismatch!");
            return Vector3.zero;
        }

        Vector3 sum = Vector3.zero;
        float weightSum = 0f;

        for (int i = 0; i < positions.Count; i++)
        {
            sum += positions[i] * weights[i];
            weightSum += weights[i];
        }

        if (weightSum <= 0f)
            return Vector3.zero;

        return sum / weightSum;
    }
    public static Vector3 WeightedAverage(List<Transform> transforms, List<float> weights)
    {
        if (transforms.Count != weights.Count)
        {
            Debug.LogError("Positions and weights count mismatch!");
            return Vector3.zero;
        }

        Vector3 sum = Vector3.zero;
        float weightSum = 0f;

        for (int i = 0; i < transforms.Count; i++)
        {
            sum += transforms[i].position * weights[i];
            weightSum += weights[i];
        }

        if (weightSum <= 0f)
            return Vector3.zero;

        return sum / weightSum;
    }
}
