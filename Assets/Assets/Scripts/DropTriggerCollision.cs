using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DropTriggerCollision : MonoBehaviour
{
    private List<GameObject> neighborDrops = new List<GameObject>();
    private List<NeighborPropInfo> neighborProps = new List<NeighborPropInfo>();

    [SerializeField]
    private LayerMask targetLayer;

    public IEnumerable<GameObject> NeighborDrops => this.neighborDrops;

    public IEnumerable<PropController> NeighborProps => this.neighborProps.Select(x => x.PropController);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherLayerMask = 1 << collision.gameObject.layer;
        if ((otherLayerMask & this.targetLayer.value) != 0)
        {
            // Logger.Log("OnTriggerEnter2D: " + collision.name);
            var otherParent = collision.transform.parent.gameObject;
            this.neighborDrops.Add(otherParent);
        }
        else
        {
            var controller = collision.GetComponentInParent<PropController>();
            if (controller != null)
            {
                var info = this.neighborProps.Find(x => x.PropController == controller);
                if (info != null)
                {
                    info.StayCount++;
                }
                else
                {
                    this.neighborProps.Add(new NeighborPropInfo() { PropController = controller, StayCount = 1 });
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var otherLayerMask = 1 << collision.gameObject.layer;
        if ((otherLayerMask & this.targetLayer.value) != 0)
        {
            // Logger.Log("OnTriggerExit2D: " + collision.name);
            var otherParent = collision.transform.parent.gameObject;
            this.neighborDrops.Remove(otherParent);
        }
        else 
        {
            var controller = collision.GetComponentInParent<PropController>();
            if (controller != null)
            {
                var info = this.neighborProps.Find(x => x.PropController == controller);
                if (info != null)
                {
                    info.StayCount--;
                    if (info.StayCount <= 0)
                    {
                        this.neighborProps.Remove(info);
                    }
                }
            }
        }
    }

    private class NeighborPropInfo
    {
        public PropController PropController { get; set; }

        public int StayCount { get; set; }
    }
}
