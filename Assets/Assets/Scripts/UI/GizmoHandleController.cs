using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GizmoHandleController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private EditManager editManager;
    private Image image;

    [SerializeField]
    private Vector2 positionOffset = Vector2.zero;

    private void Awake()
    {
        this.image = this.GetComponent<Image>();
        this.editManager = FindObjectOfType<EditManager>();
    }

    private void LateUpdate()
    {
        if (this.editManager.Selected != null)
        {
            this.image.color = this.editManager.Selected.IsDragging ?
                new Color(1.0f, 0.75f, 0.5f, 0.75f) : new Color(1.0f, 1.0f, 1.0f, 0.75f);
        }
    }

    public void DeleteTarget()
    {
        if (this.editManager.Selected != null)
        {
            var command = new DeletionEditCommand(this.editManager.Selected.PropController);
            this.editManager.Delete(this.editManager.Selected.PropController);
            this.editManager.AddCommand(command);
            this.editManager.IsDirty = true;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.OnBeginDrag(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            eventData.position += this.positionOffset * (Screen.width / 1080.0f);
            this.editManager.Selected.OnDrag(eventData);
            /*
            this.editManager.Selected.IsDragging = true;

            var screenPosition = eventData.position + this.positionOffset;
            var pos = Camera.main.ScreenToWorldPoint(screenPosition);
            pos.x = this.editManager.RoundPosition(pos.x);
            pos.y = this.editManager.RoundPosition(pos.y);
            pos.z = this.editManager.Selected.TargetObject.transform.position.z;
            this.editManager.Selected.TargetObject.transform.position = pos;
            */
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.OnEndDrag(eventData);
            //this.editManager.Selected.IsDragging = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.OnPointerClick(eventData);
            //this.editManager.Selected.IsDragging = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.OnPointerDown(eventData);
            //this.editManager.Selected.IsDragging = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.OnPointerUp(eventData);
            //this.editManager.Selected.IsDragging = false;
        }
    }
}
