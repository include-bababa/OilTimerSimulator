using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformDragHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Logger.Log("OnDrag");
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = transform.parent.position.z;
        transform.parent.position = pos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Logger.Log("OnPointerClick");
    }
}
