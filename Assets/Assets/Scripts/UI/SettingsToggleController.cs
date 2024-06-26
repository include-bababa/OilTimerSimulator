using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsToggleController : MonoBehaviour
{
    private EditManager editManager;
    private Toggle toggle;
    private RectTransform rectTransform;
    private Image backgroundImage;
    private Image labelImage;

    [SerializeField]
    private PanelController settingsPanel;

    [SerializeField]
    private Image trueForegroundImage;

    [SerializeField]
    private Image falseForegroundImage;

    [SerializeField]
    private Color trueBackgroundColor = new Color(1.0f, 1.0f, 0.75f, 0.75f);

    [SerializeField]
    private Color falseBackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.5f);

    private void Awake()
    {
        this.editManager = FindObjectOfType<EditManager>(true);
        this.toggle = this.GetComponent<Toggle>();
        this.rectTransform = this.GetComponent<RectTransform>();

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

        this.UpdateColors();
    }

    private void LateUpdate()
    {
        var pos = this.rectTransform.anchoredPosition;
        if (this.editManager.IsUIVisible)
        {
            pos.x = Mathf.Max(pos.x - 600 * Time.deltaTime, -48);
        }
        else
        {
            pos.x = Mathf.Min(pos.x + 600 * Time.deltaTime, 80);
        }
        this.rectTransform.anchoredPosition = pos;

        this.backgroundImage.raycastTarget = this.editManager.IsUIVisible;
        this.falseForegroundImage.raycastTarget = this.editManager.IsUIVisible;
        this.toggle.interactable = this.editManager.IsUIVisible;

        this.labelImage.gameObject.SetActive(this.editManager.IsLabelVisible);
        this.UpdateColors();

        /*
        if (this.editManager.IsUIVisible)
        {
            var activeSelf = this.backgroundImage.gameObject.activeSelf;
            if (!activeSelf)
            {
                this.backgroundImage.gameObject.SetActive(true);

                // Ç»Ç∫Ç©É{É^ÉìÇÃêFÇ™Ç®Ç©ÇµÇ≠Ç»ÇÈÇÃÇ≈Ç®Ç‹Ç∂Ç»Ç¢
                this.toggle.interactable = false;
                this.toggle.interactable = true;
            }

            this.labelImage.gameObject.SetActive(this.editManager.IsLabelVisible);

            this.UpdateColors();
        }
        else
        {
            this.backgroundImage.gameObject.SetActive(false);
            this.labelImage.gameObject.SetActive(false);
        }
        */
    }

    public void SetIsOpen(bool flag)
    {
        if (this.settingsPanel.IsTransitioning)
        {
            return;
        }

        if (flag)
        {
            this.settingsPanel.OpenPanel();
        }
        else
        {
            this.settingsPanel.ClosePanel();
        }

        this.toggle.isOn = flag;

        this.UpdateColors();
    }

    private void UpdateColors()
    {
        var flag = this.settingsPanel.IsOpen;

        this.trueForegroundImage.gameObject.SetActive(flag);
        this.falseForegroundImage.gameObject.SetActive(!flag);

        if (this.toggle != null)
        {
            var colors = this.toggle.colors;

            colors.normalColor = flag ?
                this.trueBackgroundColor : this.falseBackgroundColor;
            colors.highlightedColor = flag ?
                this.trueBackgroundColor : this.falseBackgroundColor;
            colors.selectedColor = flag ?
                this.trueBackgroundColor : this.falseBackgroundColor;

            this.toggle.colors = colors;
        }
    }
}
