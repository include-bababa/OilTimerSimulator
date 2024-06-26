using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditNewPropPanel : MonoBehaviour
{
    private EditManager editManager;
    private PanelController panelController;

    private GameObject selectedButton;

    [SerializeField]
    private PropSelectorPanel typeSelectorPanel;

    [SerializeField]
    private SizeSelectorPanel sizeSelectorPanel;

    [SerializeField]
    private RotationSelectorPanel rotationSelectorPanel;

    [SerializeField]
    private ColorSelectorPanel colorSelectorPanel;

    [SerializeField]
    private KeyValueButton typeSelectorButton;

    [SerializeField]
    private KeyValueButton sizeSelectorButton;

    [SerializeField]
    private KeyValueButton rotationSelectorButton;

    [SerializeField]
    private ColorButton colorSelectorButton;

    public void Initialize()
    {
        this.editManager = FindObjectOfType<EditManager>(true);

        this.typeSelectorButton.Initialize();
        this.sizeSelectorButton.Initialize();
        this.rotationSelectorButton.Initialize();
        this.colorSelectorButton.Initialize();
    }

    private void Awake()
    {
        this.panelController = this.GetComponentInParent<PanelController>();
        this.panelController.Opening += (s_, e_) =>
        {
            this.SetLabels();
        };

        this.SetLabels();
    }

    private void Update()
    {
        if (!this.panelController.IsOpen)
        {
            this.selectedButton = null;
            return;
        }

        this.SetLabels();

        if (this.selectedButton == this.typeSelectorButton.gameObject)
        {
            if (!this.typeSelectorPanel.IsOpen)
            {
                this.SetTypeToProp(this.typeSelectorPanel.SelectedProp);
                this.typeSelectorButton.SetHighlight(false);
                this.selectedButton = null;
            }
        }
        else if (this.selectedButton == this.sizeSelectorButton.gameObject)
        {
            if (!this.sizeSelectorPanel.IsOpen)
            {
                this.SetSizeToProp(this.sizeSelectorPanel.SelectedScale);
                this.sizeSelectorButton.SetHighlight(false);
                this.selectedButton = null;
            }
        }
        else if (this.selectedButton == this.rotationSelectorButton.gameObject)
        {
            if (!this.rotationSelectorPanel.IsOpen)
            {
                this.SetRotationToProp(this.rotationSelectorPanel.SelectedDirection);
                this.rotationSelectorButton.SetHighlight(false);
                this.selectedButton = null;
            }
        }
        else if (this.selectedButton == this.colorSelectorButton.gameObject)
        {
            if (!this.colorSelectorPanel.IsOpen)
            {
                this.SetColorToProp(this.colorSelectorPanel.Selected);
                this.colorSelectorButton.SetHighlight(false);
                this.selectedButton = null;
            }
        }
    }

    public void OpenPropSelector()
    {
        var propType = this.editManager.CurrentProp.PropType;
        this.typeSelectorPanel.OpenSelector((int)propType);

        this.selectedButton = this.typeSelectorButton.gameObject;
        this.typeSelectorButton.SetHighlight(true);
    }

    public void OpenSizeSelector()
    {
        var scale = this.editManager.CurrentProp.Scale;
        this.sizeSelectorPanel.OpenSelector((int)scale);

        this.selectedButton = this.sizeSelectorButton.gameObject;
        this.sizeSelectorButton.SetHighlight(true);
    }

    public void OpenRotationSelector()
    {
        var rot = this.editManager.CurrentProp.Rotation;
        this.rotationSelectorPanel.OpenSelector((int)rot);

        this.selectedButton = this.rotationSelectorButton.gameObject;
        this.rotationSelectorButton.SetHighlight(true);
    }

    public void OpenColorSelector()
    {
        var color = this.editManager.CurrentProp.Color;
        this.colorSelectorPanel.OpenSelector(color, false);

        this.selectedButton = this.colorSelectorButton.gameObject;
        this.colorSelectorButton.SetHighlight(true);
    }

    private void SetTypeToProp(PropType propType)
    {
        this.editManager.CurrentProp.PropType = propType;
    }

    private void SetSizeToProp(ScaleGrade grade)
    {
        this.editManager.CurrentProp.Scale = grade;
    }

    private void SetRotationToProp(RotationDirection rot)
    {
        this.editManager.CurrentProp.Rotation = rot;
    }

    private void SetColorToProp(Color color)
    {
        this.editManager.CurrentProp.Color = color;
    }

    private void SetLabels()
    {
        var type = this.typeSelectorPanel.GetLabel((int)this.editManager.CurrentProp.PropType);
        this.typeSelectorButton.SetValue(type);

        if (this.editManager.CurrentProp.PropType != PropType.PoweredGear)
        {
            var size = this.sizeSelectorPanel.GetLabel((int)this.editManager.CurrentProp.Scale);
            this.sizeSelectorButton.gameObject.SetActive(true);
            this.sizeSelectorButton.SetValue(size);

            this.rotationSelectorButton.gameObject.SetActive(false);
        }
        else
        {
            this.sizeSelectorButton.gameObject.SetActive(false);

            var rot = this.rotationSelectorPanel.GetLabel((int)this.editManager.CurrentProp.Rotation);
            this.rotationSelectorButton.gameObject.SetActive(true);
            this.rotationSelectorButton.SetValue(rot);
        }

        this.colorSelectorButton.SetValue(this.editManager.CurrentProp.Color);
    }
}
