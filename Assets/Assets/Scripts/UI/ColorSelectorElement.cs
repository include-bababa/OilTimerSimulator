using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectorElement : MonoBehaviour
{
    private Image backgroundImage;
    private Image colorImage;
    private TextMeshProUGUI textMesh;

    private Color backgroundDefaultColor;

    private string label;
    private Color color;

    public string Label => this.label;

    public bool HasColor => this.color.a > 0.0f;

    public Color Color => this.color;

    public void InitializeFromPanel()
    {
        this.backgroundImage = this.GetComponent<Image>();
        this.colorImage = this.transform.Find("Image").GetComponent<Image>();
        this.textMesh = this.GetComponentInChildren<TextMeshProUGUI>();

        this.backgroundDefaultColor = this.backgroundImage.color;
    }

    public void SetValue(string label, Color color)
    {
        this.label = label;
        this.color = color;

        this.colorImage.color = color;
        this.textMesh.text = label;
    }

    public void SetHighlight(bool flag)
    {
        var color = flag ?
            Constants.GetHighlightColor(this.backgroundDefaultColor.a) :
            this.backgroundDefaultColor;
        this.backgroundImage.color = color;
    }

    public void Select()
    {
        var panel = this.GetComponentInParent<ColorSelectorPanel>();
        panel.Select(this);
    }
}
