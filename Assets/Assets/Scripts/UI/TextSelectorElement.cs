using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextSelectorElement : MonoBehaviour
{
    private Image backgroundImage;
    private TextMeshProUGUI textMesh;

    private Color backgroundDefaultColor;

    private string label;
    private int val;

    public string Label => this.label;

    public int Value => this.val;

    public void InitializeFromPanel()
    {
        this.backgroundImage = this.GetComponent<Image>();
        this.textMesh = this.GetComponentInChildren<TextMeshProUGUI>();

        this.backgroundDefaultColor = this.backgroundImage.color;
    }

    public void SetValue(string label, int val)
    {
        this.label = label;
        this.val = val;

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
        var panel = this.GetComponentInParent<TextSelectorPanelBase>();
        panel.Select(this);
    }
}
