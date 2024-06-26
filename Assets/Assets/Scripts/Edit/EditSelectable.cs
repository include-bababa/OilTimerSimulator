using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditSelectable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private EditManager editManager;
    private PropController propController;
    private bool isDragging;
    private bool isRotating;

    private Vector3 dragStart;

    [SerializeField]
    private bool canSelect = true;

    [SerializeField]
    private bool canRotate = true;

    public bool CanSelect => this.canSelect;

    public bool CanRotate => this.canRotate;

    public bool IsDragging
    {
        get => this.isDragging;
        set => this.isDragging = value;
    }

    public bool IsRotating
    {
        get => this.isRotating;
        set => this.isRotating = value;
    }

    public GameObject TargetObject => this.transform.parent.gameObject;

    public PropController PropController => this.propController;

    public ScaleSetter ScaleSettter => this.propController.ScaleSettter;

    public RotationSetter RotationSetter => this.propController.RotationSetter;

    public ColorSetter ColorSetter => this.propController.ColorSetter;

    public event EventHandler<EditSelectablePointerEventData> BeginDrag;

    public event EventHandler<EditSelectablePointerEventData> Drag;

    public event EventHandler<EditSelectablePointerEventData> EndDrag;

    public event EventHandler<EditSelectablePointerEventData> PointerDown;


    public event EventHandler<EditSelectablePointerEventData> PointerClicked;

    private void Awake()
    {
        this.editManager = FindObjectOfType<EditManager>(true);
        this.propController = this.TargetObject.GetComponent<PropController>();

        this.isDragging = false;
        this.isRotating = false;
    }

    public void Update()
    {
        if (!this.editManager.IsEditMode)
        {
            this.isDragging = false;
            this.isRotating = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!this.editManager.IsEditMode)
        {
            return;
        }

        var prev = this.editManager.Selected;

        if (this.canSelect)
        {
            this.dragStart = this.TargetObject.transform.position;

            this.isDragging = true;
            this.editManager.Select(this);
        }

        // send event
        {
            var selected = this.editManager.Selected;
            var data = new EditSelectablePointerEventData
            {
                PointerEventData = eventData,
                SelectedPrevious = prev,
                SelectedNext = selected,
            };
            this.BeginDrag?.Invoke(this, data);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!this.editManager.IsEditMode)
        {
            return;
        }

        var prev = this.editManager.Selected;

        if (this.canSelect)
        {
            this.isDragging = true;
            this.editManager.Select(this);

            var pos = Camera.main.ScreenToWorldPoint(eventData.position);
            pos.x = this.editManager.RoundPositionX(pos.x);
            pos.y = this.editManager.RoundPositionY(pos.y);
            pos.z = this.TargetObject.transform.position.z;

            this.TargetObject.transform.position = pos;
        }
        else
        {
            this.isDragging = false;
            this.editManager.Select(null);
        }

        // send event
        {
            var selected = this.editManager.Selected;
            var data = new EditSelectablePointerEventData
            {
                PointerEventData = eventData,
                SelectedPrevious = prev,
                SelectedNext = selected,
            };
            this.Drag?.Invoke(this, data);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!this.editManager.IsEditMode)
        {
            return;
        }

        var prev = this.editManager.Selected;

        if (this.canSelect)
        {
            this.editManager.AddCommand(new TranslationEditCommand(
                this.propController.Guid, this.dragStart));
            this.editManager.IsDirty = true;
        }

        this.isDragging = false;
        this.editManager.Select(this.canSelect ? this : null);

        // send event
        {
            var selected = this.editManager.Selected;
            var data = new EditSelectablePointerEventData
            {
                PointerEventData = eventData,
                SelectedPrevious = prev,
                SelectedNext = selected,
            };
            this.EndDrag?.Invoke(this, data);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!this.editManager.IsEditMode)
        {
            return;
        }

        var prev = this.editManager.Selected;

        this.isDragging = false;
        this.editManager.Select(this.canSelect ? this : null);

        // send event
        {
            var data = new EditSelectablePointerEventData
            {
                PointerEventData = eventData,
                SelectedPrevious = prev,
                SelectedNext = this.canSelect ? this : null,
            };
            this.PointerClicked?.Invoke(this, data);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!this.editManager.IsEditMode)
        {
            return;
        }

        var prev = this.editManager.Selected;

        this.isDragging = true;
        this.editManager.Select(this.canSelect ? this : null);

        // send event
        {
            var data = new EditSelectablePointerEventData
            {
                PointerEventData = eventData,
                SelectedPrevious = prev,
                SelectedNext = this.canSelect ? this : null,
            };
            this.PointerDown?.Invoke(this, data);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!this.editManager.IsEditMode)
        {
            return;
        }

        this.isDragging = false;
        
        //this.manager.Select(this.canSelect ? this : null);
    }
}
