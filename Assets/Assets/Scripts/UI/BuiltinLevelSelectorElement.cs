using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuiltinLevelSelectorElement : MonoBehaviour
{
    private Button button;
    private Image backgroundImage;

    private Color backgroundDefaultColor;

    private LevelData levelData;

    [SerializeField]
    private Image thumbnailImage;

    [SerializeField]
    private TextMeshProUGUI labelTextMesh;

    public LevelData LevelData => this.levelData;

    public void InitializeFromPanel()
    {
        this.button = this.GetComponent<Button>();
        this.backgroundImage = this.GetComponent<Image>();

        this.backgroundDefaultColor = this.backgroundImage.color;
    }

    public void SetLevel(LevelData data)
    {
        this.levelData = data;

        var theme = LevelManager.Instance.GetTheme(data.ThemeName);
        var handle = theme.ThumbnailTexture.LoadAssetAsync<Sprite>();
        this.thumbnailImage.sprite = handle.WaitForCompletion();

        this.labelTextMesh.text = theme.DisplayName.GetLocalizedString();
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

        this.labelTextMesh.color = flag ? new Color(0.875f, 0.875f, 0.875f) : Color.white;
    }

    public void Select()
    {
        var panel = this.GetComponentInParent<LevelSelectorPanel>();
        panel.Select(this);
    }
}
