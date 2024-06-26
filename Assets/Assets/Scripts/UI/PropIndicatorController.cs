using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class PropIndicatorController : MonoBehaviour
{
    private EditManager editManager;
    private Toggle toggle;
    private Image backgroundImage;
    private Image labelImage;

    private EditCurrentProp lastFrameProp;
    private float inflateTimer;

    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private PanelController settingsPanel;

    [SerializeField]
    private PanelController editPanel;

    [SerializeField]
    private Color trueBackgroundColor = new Color(1.0f, 1.0f, 0.75f, 0.75f);

    [SerializeField]
    private Color falseBackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    private void Awake()
    {
        this.toggle = this.GetComponent<Toggle>();
        this.editManager = FindObjectOfType<EditManager>();

        this.backgroundImage = this.transform.Find("Background").GetComponent<Image>();
        this.labelImage = this.transform.Find("Label").GetComponent<Image>();

        this.settingsPanel.Closing += (s_, e_) =>
        {
            if (this.toggle.isOn == true)
            {
                this.toggle.isOn = false;
                this.UpdateColors();
            }
        };
        this.editPanel.Closing += (s_, e_) =>
        {
            if (this.toggle.isOn == true)
            {
                this.toggle.isOn = false;
                this.UpdateColors();
            }
        };

        this.lastFrameProp = new EditCurrentProp();
        this.inflateTimer = 0.0f;
        this.UpdateColors();
    }

    private void LateUpdate()
    {
        if (this.editManager.IsEditMode)
        {
            var activeSelf = this.backgroundImage.gameObject.activeSelf;
            if (!activeSelf)
            {
                this.backgroundImage.gameObject.SetActive(true);

                // ‚È‚º‚©ƒ{ƒ^ƒ“‚ÌF‚ª‚¨‚©‚µ‚­‚È‚é‚Ì‚Å‚¨‚Ü‚¶‚È‚¢
                this.toggle.interactable = false;
                this.toggle.interactable = true;
            }

            this.labelImage.gameObject.SetActive(this.editManager.IsLabelVisible);

            if (this.editManager.Selected == null)
            {
                if (!IsSame(this.lastFrameProp, this.editManager.CurrentProp))
                {
                    this.textMesh.text = this.BuildString(this.editManager.CurrentProp);

                    this.lastFrameProp.PropType = this.editManager.CurrentProp.PropType;
                    this.lastFrameProp.Scale = this.editManager.CurrentProp.Scale;
                    this.lastFrameProp.Color = this.editManager.CurrentProp.Color;
                }
            }
            else
            {
                this.textMesh.text = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "PropIndicatorEditSelected");

                this.lastFrameProp.Color = new Color(0, 0, 0, 0); // ‚Æ‚è‚ ‚¦‚¸‚©‚Ô‚ç‚È‚¢‚à‚Ì‚É‚µ‚Ä‚¨‚­
            }

            if (this.inflateTimer > 0)
            {
                this.inflateTimer -= Time.deltaTime;
                this.inflateTimer = Mathf.Max(0.0f, this.inflateTimer);
            }

            this.UpdateColors();
            this.UpdateScale();
        }
        else
        {
            this.backgroundImage.gameObject.SetActive(false);
            this.labelImage.gameObject.SetActive(false);
            this.inflateTimer = 0.0f;
        }
    }

    public void SetIsOpen(bool flag)
    {
        if (this.settingsPanel.IsTransitioning || this.editPanel.IsTransitioning)
        {
            return;
        }

        if (flag)
        {
            if (this.editManager.Selected == null)
            {
                this.settingsPanel.OpenPanel();
            }
            else
            {
                this.editPanel.OpenPanel();
            }
        }
        else
        {
            if (this.editManager.Selected == null)
            {
                this.settingsPanel.ClosePanel();
            }
            else
            {
                this.editPanel.ClosePanel();
            }
        }

        this.toggle.isOn = flag;

        this.UpdateColors();
    }

    public void InflateMoment()
    {
        this.inflateTimer = 0.25f;
    }

    private static bool IsSame(EditCurrentProp lhs, EditCurrentProp rhs)
    {
        return lhs.PropType == rhs.PropType &&
            lhs.Scale == rhs.Scale &&
            lhs.Rotation == rhs.Rotation &&
            lhs.Color == rhs.Color;
    }

    private string BuildString(EditCurrentProp prop)
    {
        var propData = LevelManager.Instance.GetPropData(prop.PropType);
        var propName = propData.DisplayName.GetLocalizedString();
        if (propData.PropType == PropType.PoweredGear)
        {
            var rotationData = LevelManager.Instance.GetRotationData(prop.Rotation);
            var rotationString = rotationData.ShortName.GetLocalizedString();
            return propName + "(" + rotationString + ")";
        }
        else
        {
            var scaleData = LevelManager.Instance.GetScaleData(prop.Scale);
            var scaleString = scaleData.ShortName.GetLocalizedString();
            return propName + "(" + scaleString + ")";
        }
    }

    private void UpdateColors()
    {
        var flag = this.settingsPanel.IsOpen || this.editPanel.IsOpen;

        if (this.toggle != null)
        {
            var ratio = flag ? 1.0f : this.inflateTimer / 0.18f;
            var color = this.trueBackgroundColor * ratio + this.falseBackgroundColor * (1 - ratio);

            var colors = this.toggle.colors;
            colors.normalColor = color;
            colors.highlightedColor = color;
            colors.selectedColor = color;
            this.toggle.colors = colors;
        }
    }

    private void UpdateScale()
    {
        var scale = 1.0f + this.inflateTimer;
        this.backgroundImage.transform.localScale = new Vector3(scale, scale, scale);
    }
}
