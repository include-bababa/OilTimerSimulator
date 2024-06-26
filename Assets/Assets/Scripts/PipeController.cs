using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeController : MonoBehaviour
{
    private void Start()
    {
        var innerTrigger = this.transform.Find("InnerTrigger").gameObject;
        var triggerHandler = innerTrigger.GetComponent<TriggerHandler>();
        triggerHandler.OnTriggerEnter2DHandler += this.InnerTrigger_OnTriggerEnter2D;
        triggerHandler.OnTriggerExit2DHandler += this.InnerTrigger_OnTriggerExit2D;
    }

    private void InnerTrigger_OnTriggerEnter2D(Collider2D collision)
    {
        var drop = collision.GetComponentInParent<DropController>();
        if (drop != null)
        {
            if (drop.BoundTo == null)
            {
                drop.BoundTo = this.gameObject;
            }
        }
    }

    private void InnerTrigger_OnTriggerExit2D(Collider2D collision)
    {
        var drop = collision.GetComponentInParent<DropController>();
        if (drop != null)
        {
            if (drop.BoundTo == this.gameObject)
            {
                drop.BoundTo = null;
            }
        }
    }
}
