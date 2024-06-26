using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditChangePropPanel : MonoBehaviour
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

        this.panelController = this.GetComponent<PanelController>();
        this.panelController.Opening += (s_, e_) =>
        {
            this.SetLabels();
        };

        this.typeSelectorButton.Initialize();
        this.sizeSelectorButton.Initialize();
        this.rotationSelectorButton.Initialize();
        this.colorSelectorButton.Initialize();
    }

    private void Update()
    {
        if (!this.panelController.IsOpen)
        {
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

    public void OpenTypeSelector()
    {
        var selected = this.editManager.Selected;
        var type = selected?.PropController?.PropType ?? PropType.Platform;
        this.typeSelectorPanel.OpenSelector((int)type);

        this.selectedButton = this.typeSelectorButton.gameObject;
        this.typeSelectorButton.SetHighlight(true);
    }

    public void OpenSizeSelector()
    {
        var selected = this.editManager.Selected;
        var scale = selected?.ScaleSettter?.Scale ?? ScaleGrade.Medium;
        this.sizeSelectorPanel.OpenSelector((int)scale);

        this.selectedButton = this.sizeSelectorButton.gameObject;
        this.sizeSelectorButton.SetHighlight(true);
    }

    public void OpenRotationSelector()
    {
        var selected = this.editManager.Selected;
        var rot = selected?.RotationSetter?.Rotation ?? RotationDirection.ClockwiseSlow;
        this.rotationSelectorPanel.OpenSelector((int)rot);

        this.selectedButton = this.rotationSelectorButton.gameObject;
        this.rotationSelectorButton.SetHighlight(true);
    }

    public void OpenColorSelector()
    {
        var selected = this.editManager.Selected;
        var color = selected?.ColorSetter?.Color ?? Color.white;
        this.colorSelectorPanel.OpenSelector(color, false);

        this.selectedButton = this.colorSelectorButton.gameObject;
        this.colorSelectorButton.SetHighlight(true);
    }

    private void SetTypeToProp(PropType propType)
    {
        var selected = this.editManager.Selected;
        var command = new ChangeTypeEditCommand(selected.PropController);

        var guid = selected.PropController.Guid;
        var pos = selected.TargetObject.transform.position;
        var scale = selected.ScaleSettter?.Scale ?? ScaleGrade.Medium;
        var rot = selected.RotationSetter?.Rotation ?? RotationDirection.ClockwiseSlow;
        var color = selected.ColorSetter?.Color ?? Color.white;

        this.editManager.Delete(selected.PropController);
        var created = this.editManager.Create(propType, pos, scale, rot, color);
        created.Guid = guid;
        this.editManager.Select(created.GetComponentInChildren<EditSelectable>());

        this.editManager.AddCommand(command);
        this.editManager.IsDirty = true;
    }

    private void SetSizeToProp(ScaleGrade grade)
    {
        var selected = this.editManager.Selected;

        if (selected?.ScaleSettter != null)
        {
            var before = selected.ScaleSettter.Scale;
            if (before != grade)
            {
                selected.ScaleSettter.SetScale(grade);
                this.editManager.AddCommand(
                    new ChangeSizeEditCommand(selected.PropController.Guid, before));
                this.editManager.IsDirty = true;
            }
        }
    }

    private void SetRotationToProp(RotationDirection rot)
    {
        var selected = this.editManager.Selected;

        if (selected?.RotationSetter != null)
        {
            var before = selected.RotationSetter.Rotation;
            if (before != rot)
            {
                selected.RotationSetter.SetRotation(rot);
                this.editManager.AddCommand(
                    new ChangeRotationEditCommand(selected.PropController.Guid, before));
                this.editManager.IsDirty = true;
            }
        }

        selected.RotationSetter?.SetRotation(rot);
    }

    private void SetColorToProp(Color color)
    {
        var selected = this.editManager.Selected;

        if (selected?.ColorSetter != null)
        {
            var before = selected.ColorSetter.Color;
            if (before != color)
            {
                selected.ColorSetter.SetColor(color);
                this.editManager.AddCommand(
                    new ChangeColorEditCommand(selected.PropController.Guid, before));
                this.editManager.IsDirty = true;
            }
        }

    }

    private void SetLabels()
    {
        var type = this.editManager.Selected?.PropController?.PropType;
        if (type != null)
        {
            var text = this.typeSelectorPanel.GetLabel((int)type.Value);
            this.typeSelectorButton.SetValue(text);
        }

        if (type != PropType.PoweredGear)
        {
            var scale = this.editManager.Selected?.ScaleSettter?.Scale;
            if (scale != null)
            {
                var text = this.sizeSelectorPanel.GetLabel((int)scale.Value);
                this.sizeSelectorButton.SetValue(text);
            }

            this.sizeSelectorButton.gameObject.SetActive(true);
            this.rotationSelectorButton.gameObject.SetActive(false);
        }
        else
        {
            var rot = this.editManager.Selected?.RotationSetter?.Rotation;
            if (rot != null)
            {
                var text = this.rotationSelectorPanel.GetLabel((int)rot.Value);
                this.rotationSelectorButton.SetValue(text);
            }

            this.sizeSelectorButton.gameObject.SetActive(false);
            this.rotationSelectorButton.gameObject.SetActive(true);
        }

        var color = this.editManager.Selected?.ColorSetter?.Color;
        if (color != null)
        {
            this.colorSelectorButton.SetValue(color.Value);
        }
    }
}
