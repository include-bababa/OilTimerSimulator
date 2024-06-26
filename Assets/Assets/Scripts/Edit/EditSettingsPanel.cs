using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditSettingsPanel : MonoBehaviour
{
    private EditManager editManager;
    private PanelController panelController;

    private State currentState;

    [SerializeField]
    private EnterNamePanel enterNamePanel;

    [SerializeField]
    private ThemeSelectorPanel themeSelectorPanel;

    [SerializeField]
    private ColorSelectorPanel colorSelectorPanel;

    [SerializeField]
    private IntervalSelectorPanel intervalSelectorPanel;

    [SerializeField]
    private MessagePanel messagePanel;

    [SerializeField]
    private KeyValueButton themeButton;

    [SerializeField]
    private ColorButton mainOilButton;

    [SerializeField]
    private ColorButton subOilButton;

    [SerializeField]
    private KeyValueButton mainIntervalButton;

    [SerializeField]
    private KeyValueButton subIntervalButton;

    public void Initialize()
    {
        this.editManager = FindObjectOfType<EditManager>(true);

        this.themeButton.Initialize();
        this.mainOilButton.Initialize();
        this.subOilButton.Initialize();
        this.mainIntervalButton.Initialize();
        this.subIntervalButton.Initialize();
    }

    private void Awake()
    {
        this.panelController = this.GetComponentInParent<PanelController>();
        this.panelController.Opening += (s_, e_) =>
        {
            this.SetLabels();
        };

        this.SetLabels();
    }

    private void Update()
    {
        if (!this.panelController.IsOpen)
        {
            this.currentState = State.None;
            return;
        }

        if (this.currentState == State.EnterName)
        {
            if (!this.enterNamePanel.IsOpen)
            {
                if (!string.IsNullOrEmpty(this.enterNamePanel.Text))
                {
                    this.editManager.CurrentLevel.DisplayName = this.enterNamePanel.Text;
                }

                this.currentState = State.None;
            }
        }
        else if (this.currentState == State.Theme)
        {
            if (!this.themeSelectorPanel.IsOpen)
            {
                var command = new ThemeEditCommand(this.editManager.CurrentTheme);

                var theme = this.themeSelectorPanel.Selected;
                this.editManager.SetTheme(theme);
                this.editManager.AddCommand(command);

                this.themeButton.SetHighlight(false);
                this.currentState = State.None;
            }
        }
        else if (this.currentState == State.MainColor)
        {
            if (!this.colorSelectorPanel.IsOpen)
            {
                var command = new OilColorEditCommand(
                    OilColorEditCommand.OilType.Main, this.editManager.OilPoolManager.MainColor);

                var color = this.colorSelectorPanel.Selected;
                this.editManager.OilPoolManager.SetMainColor(color);
                this.editManager.AddCommand(command);

                this.mainOilButton.SetHighlight(false);
                this.currentState = State.None;
            }
        }
        else if (this.currentState == State.SubColor)
        {
            if (!this.colorSelectorPanel.IsOpen)
            {
                var command = new OilColorEditCommand(
                    OilColorEditCommand.OilType.Sub, this.editManager.OilPoolManager.SubColor);

                var color = this.colorSelectorPanel.Selected;
                if (color.a <= 0.0f)
                {
                    this.editManager.OilPoolManager.UnsetSubColor();
                }
                else
                {
                    this.editManager.OilPoolManager.SetSubColor(color);
                }

                this.editManager.AddCommand(command);

                this.subOilButton.SetHighlight(false);
                this.currentState = State.None;
            }
        }
        else if (this.currentState == State.MainInterval)
        {
            if (!this.intervalSelectorPanel.IsOpen)
            {
                var command = new OilIntervalEditCommand(
                    OilIntervalEditCommand.OilType.Main,
                    this.editManager.OilPoolManager.MainSpawner.DropInterval);

                var interval = this.intervalSelectorPanel.SelectedInterval;
                this.editManager.OilPoolManager.MainSpawner.DropInterval = interval;
                this.editManager.AddCommand(command);

                this.mainIntervalButton.SetHighlight(false);
                this.currentState = State.None;
            }
        }
        else if (this.currentState == State.SubInterval)
        {
            if (!this.intervalSelectorPanel.IsOpen)
            {
                var command = new OilIntervalEditCommand(
                    OilIntervalEditCommand.OilType.Sub,
                    this.editManager.OilPoolManager.SubSpawner.DropInterval);

                var interval = this.intervalSelectorPanel.SelectedInterval;
                this.editManager.OilPoolManager.SubSpawner.DropInterval = interval;
                this.editManager.AddCommand(command);

                this.subIntervalButton.SetHighlight(false);
                this.currentState = State.None;
            }
        }
        else if (this.currentState == State.Save)
        {
            if (!this.messagePanel.IsOpen)
            {
                if (this.messagePanel.Result == MessagePanel.MessageResult.OK)
                {
                    this.editManager.SaveLevel();
                }

                this.currentState = State.None;
            }
        }

        this.SetLabels();
    }

    public void OpenEnterNamePanel()
    {
        var name = this.editManager.CurrentLevel.DisplayName;
        this.enterNamePanel.OpenPanel(name);

        this.currentState = State.EnterName;
    }

    public void OpenThemePanel()
    {
        var currentTheme = this.editManager.CurrentTheme;
        this.themeSelectorPanel.OpenSelector(currentTheme);

        this.currentState = State.Theme;
        this.themeButton.SetHighlight(true);
    }

    public void OpenMainColorPanel()
    {
        var color = this.editManager.OilPoolManager.MainColor;
        this.colorSelectorPanel.OpenSelector(color, false);

        this.currentState = State.MainColor;
        this.mainOilButton.SetHighlight(true);
    }

    public void OpenSubColorPanel()
    {
        var color = this.editManager.OilPoolManager.SubColor;
        this.colorSelectorPanel.OpenSelector(color ?? new Color(), true);

        this.currentState = State.SubColor;
        this.subOilButton.SetHighlight(true);
    }

    public void OpenMainIntervalPanel()
    {
        var interval = this.editManager.OilPoolManager.MainSpawner.DropInterval;
        this.intervalSelectorPanel.OpenSelectorWithInterval(interval);

        this.currentState = State.MainInterval;
        this.mainIntervalButton.SetHighlight(true);
    }

    public void OpenSubIntervalPanel()
    {
        var interval = this.editManager.OilPoolManager.SubSpawner.DropInterval;
        this.intervalSelectorPanel.OpenSelectorWithInterval(interval);

        this.currentState = State.SubInterval;
        this.subIntervalButton.SetHighlight(true);
    }

    public void OpenSavePanel()
    {
        var name = this.editManager.CurrentLevel.DisplayName;
        this.messagePanel.Title = "Save";
        this.messagePanel.Message = "Are you sure to save '" + name + "'?";
        this.messagePanel.OpenPanel();

        this.currentState = State.Save;
    }

    private void SetLabels()
    {
        var themeName = this.editManager.CurrentTheme.DisplayName.GetLocalizedString();
        this.themeButton.SetValue(themeName);

        var mainColor = this.editManager.OilPoolManager.MainColor;
        var subColor = this.editManager.OilPoolManager.SubColor;
        this.mainOilButton.SetValue(mainColor);
        if (subColor.HasValue)
        {
            this.subOilButton.SetValue(subColor.Value);
        }
        else
        {
            this.subOilButton.UnsetValue();
        }

        var mainInterval = this.editManager.OilPoolManager.MainSpawner.DropInterval;
        var mainIntervalLabel = this.intervalSelectorPanel.GetLabelFromInterval(mainInterval);
        this.mainIntervalButton.SetValue(mainIntervalLabel);
        if (subColor.HasValue)
        {
            var subInterval = this.editManager.OilPoolManager.SubSpawner.DropInterval;
            var subIntervalLabel = this.intervalSelectorPanel.GetLabelFromInterval(subInterval);
            this.subIntervalButton.SetValue(subIntervalLabel);
        }
        else
        {
            this.subIntervalButton.SetValue("-");
        }
    }

    private enum State
    {
        None,
        EnterName,
        Theme,
        MainColor,
        SubColor,
        MainInterval,
        SubInterval,
        Save,
    }
}
