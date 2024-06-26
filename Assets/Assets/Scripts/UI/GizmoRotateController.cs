using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GizmoRotateController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler
{
    private Image image;
    private EditManager editManager;

    private float dragStart;

    private void Awake()
    {
        this.image = this.GetComponent<Image>();
        this.editManager = FindObjectOfType<EditManager>();
    }

    private void LateUpdate()
    {
        this.image.color = this.editManager.Selected?.IsRotating == true ?
            new Color(1.0f, 0.75f, 0.5f) : Color.white;

        if (this.editManager.Selected != null)
        {
            this.image.transform.rotation = this.editManager.Selected.transform.rotation;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.dragStart = this.editManager.Selected.transform.eulerAngles.z;
            this.editManager.Selected.IsRotating = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.IsRotating = true;

            var center3d = Camera.main.WorldToScreenPoint(this.transform.position);
            var center = new Vector2(center3d.x, center3d.y);
            // var center = new Vector2(this.transform.position.x, this.transform.position.y);
            var baseDir = Vector2.right * this.transform.localScale.x * -1;
            var dir = eventData.position - center;
            var angle = Vector2.SignedAngle(baseDir, dir);
            angle = this.editManager.RoundRotation(angle);

            this.editManager.Selected.TargetObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.AddCommand(new RotationEditCommand(
                this.editManager.Selected.PropController.Guid, this.dragStart));
            this.editManager.IsDirty = true;

            this.editManager.Selected.IsRotating = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.IsRotating = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.editManager.Selected != null)
        {
            this.editManager.Selected.IsRotating = true;
        }
    }
}
