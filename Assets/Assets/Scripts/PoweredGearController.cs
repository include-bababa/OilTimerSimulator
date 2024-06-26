using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredGearController : MonoBehaviour
{
    [SerializeField]
    private float angularVelocity = 30.0f;

    private void Awake()
    {
        var rigidbody2d = this.GetComponent<Rigidbody2D>();
        rigidbody2d.angularVelocity = this.angularVelocity;
    }

    public void Reset()
    {
        this.transform.rotation = Quaternion.identity;
    }
}
