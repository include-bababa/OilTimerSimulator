using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSelectorElement : MonoBehaviour
{
    private Button button;
    private Image backgroundImage;
    private Image thumbnailImage;
    private TextMeshProUGUI textMesh;

    private Color backgroundDefaultColor;

    private string label;
    private ThemeInfo themeInfo;

    public string Label => this.label;

    public ThemeInfo ThemeInfo => this.themeInfo;

    public void InitializeFromPanel()
    {
        this.button = this.GetComponent<Button>();
        this.backgroundImage = this.GetComponent<Image>();
        this.thumbnailImage = this.transform.Find("Image").GetComponent<Image>();
        this.textMesh = this.GetComponentInChildren<TextMeshProUGUI>();

        this.backgroundDefaultColor = this.backgroundImage.color;
    }

    public void SetValue(ThemeInfo theme)
    {
        this.themeInfo = theme;

        if (theme.ThumbnailTexture.IsValid())
        {
            this.thumbnailImage.sprite = theme.ThumbnailTexture.OperationHandle.Convert<Sprite>().Result;
        }
        else
        {
            var handle = theme.ThumbnailTexture.LoadAssetAsync<Sprite>();
            this.thumbnailImage.sprite = handle.WaitForCompletion();
        }

        this.textMesh.text = this.themeInfo.DisplayName.GetLocalizedString();
    }

    public void SetHighlight(bool flag)
    {
        var color = flag ?
            Constants.GetHighlightColor(this.backgroundDefaultColor.a) :
            this.backgroundDefaultColor;
        this.backgroundImage.color = color;
    }

    public void SetSelected(bool flag)
    {
        var color = flag ?
            Constants.GetHighlightColor(this.backgroundDefaultColor.a) :
            this.backgroundDefaultColor;
        this.backgroundImage.color = color;
        this.button.interactable = !flag;

        this.textMesh.color = flag ? new Color(0.875f, 0.875f, 0.875f) : Color.white;
    }

    public void Select()
    {
        var panel = this.GetComponentInParent<ThemeSelectorPanel>();
        panel.Select(this);
    }
}
