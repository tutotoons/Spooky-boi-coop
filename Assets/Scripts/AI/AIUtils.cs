using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIUtils
{
    public const float DISTANCE_THRESHOLD = 0.2f;

    public static bool ApproximatePositionReached(Vector3 _a, Vector3 _b, float _dist = DISTANCE_THRESHOLD)
    {
        return Vector2.Distance(new Vector2(_a.x, _a.z), new Vector2(_b.x, _b.z)) <= _dist;
    }
}
