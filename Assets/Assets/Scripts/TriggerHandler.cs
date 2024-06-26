using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetLayer = ~0;

    public event Action<Collider2D> OnTriggerEnter2DHandler;
    public event Action<Collider2D> OnTriggerExit2DHandler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherLayerMask = 1 << collision.gameObject.layer;
        if ((otherLayerMask & this.targetLayer.value) != 0)
        {
            this.OnTriggerEnter2DHandler?.Invoke(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var otherLayerMask = 1 << collision.gameObject.layer;
        if ((otherLayerMask & this.targetLayer.value) != 0)
        {
            this.OnTriggerExit2DHandler?.Invoke(collision);
        }
    }

}
