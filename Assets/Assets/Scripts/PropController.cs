using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PropController : MonoBehaviour
{
    private Guid guid;

    private ScaleSetter scaleSetter;
    private RotationSetter rotationSetter;
    private ColorSetter colorSetter;
    private EditSelectable editSelectable;
    private Collider2D[] colliders;

    private EditManager editManager;

    [SerializeField]
    private PropType propType;

    [SerializeField]
    private UnityEvent reseter;

    [SerializeField]
    private GameObject[] outlines;

    public Guid Guid
    {
        get => this.guid;
        set => this.guid = value;
    }

    public PropType PropType => this.propType;

    public ScaleSetter ScaleSettter => this.scaleSetter;

    public RotationSetter RotationSetter => this.rotationSetter;

    public ColorSetter ColorSetter => this.colorSetter;

    public EditSelectable EditSelectable => this.editSelectable;

    public Collider2D[] Colliders => this.colliders;

    private void Awake()
    {
        this.guid = Guid.NewGuid();

        this.scaleSetter = this.GetComponent<ScaleSetter>();
        this.rotationSetter = this.GetComponent<RotationSetter>();
        this.colorSetter = this.GetComponent<ColorSetter>();
        this.editSelectable = this.GetComponentInChildren<EditSelectable>();
        this.colliders = this.GetComponentsInChildren<Collider2D>();

        this.editManager = FindObjectOfType<EditManager>(true);
    }

    private void LateUpdate()
    {
        var selected = this == this.editManager.Selected?.PropController;
        if (this.outlines != null)
        {
            foreach (var outline in this.outlines)
            {
                outline.SetActive(selected);
            }
        }
    }

    public void Reset()
    {
        this.reseter?.Invoke();
    }
}
