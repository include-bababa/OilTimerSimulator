using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSetter : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    private RotationDirection rot = RotationDirection.ClockwiseSlow;

    [SerializeField]
    private float slowAngularVelocity = 15.0f;

    [SerializeField]
    private float fastAngularVelocity = 30.0f;

    public RotationDirection Rotation => this.rot;

    private void Awake()
    {
        this.rigidbody2d = this.GetComponent<Rigidbody2D>();
    }

    public void SetRotation(RotationDirection rot)
    {
        if (this.rigidbody2d == null)
        {
            this.rigidbody2d = this.GetComponent<Rigidbody2D>();
        }

        switch (rot)
        {
            case RotationDirection.ClockwiseSlow:
                this.rigidbody2d.angularVelocity = -this.slowAngularVelocity;
                break;
            case RotationDirection.ClockwiseFast:
                this.rigidbody2d.angularVelocity = -this.fastAngularVelocity;
                break;
            case RotationDirection.CounterClockwiseSlow:
                this.rigidbody2d.angularVelocity = this.slowAngularVelocity;
                break;
            case RotationDirection.CounterClockwiseFast:
                this.rigidbody2d.angularVelocity = this.fastAngularVelocity;
                break;
        }

        this.rot = rot;
    }
}
