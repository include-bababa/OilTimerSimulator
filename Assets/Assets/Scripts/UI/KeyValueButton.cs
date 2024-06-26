using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyValueButton : MonoBehaviour
{
    private Image backgroundImage;
    private Button button;

    private Color backgroundDefaultColor;

    [SerializeField]
    private TextMeshProUGUI labelTextMesh;

    [SerializeField]
    private TextMeshProUGUI valueTextMesh;

    public void Initialize()
    {
        if (this.backgroundImage == null)
        {
            this.backgroundImage = this.GetComponent<Image>();
            this.button = this.GetComponent<Button>();

            this.backgroundDefaultColor = this.backgroundImage?.color ?? Color.white;
        }
    }

    private void Awake()
    {
        this.Initialize();
    }

    public void SetLabel(string text)
    {
        this.labelTextMesh.text = text;
    }

    public void SetValue(string text)
    {
        this.valueTextMesh.text = text;
    }

    public void SetHighlight(bool flag)
    {
        var color = flag ?
            Constants.GetHighlightColor(this.backgroundDefaultColor.a) :
            this.backgroundDefaultColor;
        this.backgroundImage.color = color;
    }

    public void SetEnable(bool flag)
    {
        this.button.interactable = flag;
        this.labelTextMesh.color = flag ? Color.white : Constants.DisabledColor;
        this.valueTextMesh.color = flag ? Color.white : Constants.DisabledColor;
    }
}
