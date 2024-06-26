using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : InputObserverBase
{
    private const float FadeTime = 1.0f;

    private List<PanelController> panels = new List<PanelController>();

    private float faderTimer;

    [SerializeField]
    private GameObject background;

    [SerializeField]
    private Image blackFader;

    [SerializeField]
    private bool useAd = false;

    public bool IsFading => this.faderTimer != 0.0f;

    private void Start()
    {
        // fade-in
        this.faderTimer = FadeTime;
    }

    private void Update()
    {
        /*
        // back key
        if (Input.GetKey(KeyCode.Escape))
        {
            if (this.panels.Count > 0)
            {
                var top = this.panels[this.panels.Count - 1];
                top.ClosePanel();
            }
        }
        */

        // fader
        if (this.faderTimer > 0)
        {
            // fade-in
            this.faderTimer = Mathf.Max(this.faderTimer - Time.deltaTime, 0);

            var color = this.blackFader.color;
            color.a = this.faderTimer / FadeTime;
            this.blackFader.color = color;

            // show/hide banner
            if (this.useAd)
            {
                var showBanner = this.faderTimer <= 0;
                if (showBanner != AdManager.Instance.IsBannerShowing)
                {
                    if (showBanner)
                    {
                        AdManager.Instance.ShowBannerAd();
                    }
                    else
                    {
                        AdManager.Instance.HideBannerAd();
                    }
                }
            }
        }
        else if (this.faderTimer < 0)
        {
            // fade-out
            this.faderTimer = Mathf.Min(this.faderTimer + Time.deltaTime, 0);

            var color = this.blackFader.color;
            color.a = 1.0f + this.faderTimer / FadeTime;
            this.blackFader.color = color;

            // show/hide banner
            if (this.useAd)
            {
                var showBanner = this.faderTimer >= 0;
                if (showBanner != AdManager.Instance.IsBannerShowing)
                {
                    if (showBanner)
                    {
                        AdManager.Instance.ShowBannerAd();
                    }
                    else
                    {
                        AdManager.Instance.HideBannerAd();
                    }
                }
            }
        }
    }

    public void StartFadeOut()
    {
        this.faderTimer = -FadeTime;
    }

    public override bool ProcessInput(InputType inputType)
    {
        if (this.panels.Count <= 0)
        {
            return false;
        }

        var top = this.panels[this.panels.Count - 1];
        top.ClosePanel();
        return true;
    }

    public void CloseAllPanel()
    {
        for (int index = this.panels.Count - 1; index >= 0; index--)
        {
            var panel = this.panels[index];
            panel.ClosePanel();
        }
    }

    // only for PanelController
    public void PushPanel(PanelController panel)
    {
        this.panels.Add(panel);

        if (!this.background.activeSelf)
        {
            this.background.SetActive(true);
        }
    }

    // only for PanelController
    public void PopPanel(PanelController panel)
    {
        this.panels.Remove(panel);

        if (this.panels.Count <= 0)
        {
            this.background.SetActive(false);
        }
        else
        {
            var nextPanel = this.panels[this.panels.Count - 1];
            if (!nextPanel.gameObject.activeSelf)
            {
                nextPanel.gameObject.SetActive(true);
            }
        }
    }
}
