using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    private const float cGravity = 10.0f;

    private void Update()
    {
        if (SaveManager.Instance.IsRotationEnabled)
        {
            var acc = this.GetAccelerometerValue();
            if (!IsZeroVector(acc))
            {
                Physics2D.gravity = new Vector2(acc.x, acc.y) * cGravity;
            }
        }
        else
        {
            Physics2D.gravity = new Vector2(0, -cGravity);
        }
    }

    private Vector3 GetAccelerometerValue()
    {
        var acc = Vector3.zero;
        var period = 0.0f;

        foreach (AccelerationEvent evnt in Input.accelerationEvents)
        {
            acc += evnt.acceleration * evnt.deltaTime;
            period += evnt.deltaTime;
        }

        if (period > 0)
        {
            acc *= 1.0f / period;
        }
        return acc;
    }

    private static bool IsZeroVector(Vector3 vec, float eps = Vector3.kEpsilon)
    {
        if (vec.x > -eps && vec.x < eps &&
            vec.y > -eps && vec.y < eps &&
            vec.z > -eps && vec.z < eps)
        {
            return true;
        }

        return false;
    }
}
