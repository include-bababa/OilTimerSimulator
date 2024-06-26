using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UserLevelSelectorElement : MonoBehaviour
{
    private Button button;
    private Image backgroundImage;

    private Color backgroundDefaultColor;
    private string lastUpdatePrefix;

    private LevelData levelData;

    [SerializeField]
    private Image thumbnailImage;

    [SerializeField]
    private TextMeshProUGUI labelTextMesh;

    [SerializeField]
    private TextMeshProUGUI timestampTextMesh;

    [SerializeField]
    private Button trashButton;

    public LevelData LevelData => this.levelData;

    public void InitializeFromPanel()
    {
        this.button = this.GetComponent<Button>();
        this.backgroundImage = this.GetComponent<Image>();

        this.backgroundDefaultColor = this.backgroundImage.color;
        this.lastUpdatePrefix = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "SampleScene/UI/Canvas/EditorMiniPanel/EditorMiniPanel/SaveContent/LastSaved");
    }

    public void SetLevel(LevelData data, bool canDelete)
    {
        this.levelData = data;

        var theme = LevelManager.Instance.GetTheme(data.ThemeName);
        if (theme.ThumbnailTexture.IsValid())
        {
            this.thumbnailImage.sprite = theme.ThumbnailTexture.OperationHandle.Convert<Sprite>().Result;
        }
        else
        {
            var handle = theme.ThumbnailTexture.LoadAssetAsync<Sprite>();
            this.thumbnailImage.sprite = handle.WaitForCompletion();
        }

        this.labelTextMesh.text = this.levelData.DisplayName;
        this.timestampTextMesh.text = this.lastUpdatePrefix + this.levelData.LastUpdate.ToString();
        this.trashButton.gameObject.SetActive(canDelete);
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
        this.labelTextMesh.text = this.levelData.DisplayName;
    }

    public void Select()
    {
        var panel = this.GetComponentInParent<LevelSelectorPanel>();
        panel.Select(this);
    }

    public void Delete()
    {
        var panel = this.GetComponentInParent<LevelSelectorPanel>();
        panel.Delete(this);
    }
}
