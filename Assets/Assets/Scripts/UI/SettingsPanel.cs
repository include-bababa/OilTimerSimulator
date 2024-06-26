using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
#if UNITY_EDITOR
    private const float ReviewRequestTime = 10;
#else
    private const float ReviewRequestTime = 180;
#endif

    private EditManager editManager;
    private PanelController panelController;

    private State currentState;

    [SerializeField]
    private LevelSelectorPanel levelSelectorPanel;

    [SerializeField]
    private EnterNamePanel enterNamePanel;

    [SerializeField]
    private TimerSelectorPanel timerSelectorPanel;

    [SerializeField]
    private ConfirmSavePanel confirmSavePanel;

    [SerializeField]
    private ReviewRequestPanel reviewRequestPanel;

    [SerializeField]
    private KeyValueButton timerSelectorButton;

    [SerializeField]
    private Button editModeButton;

    [SerializeField]
    private Button viewModeButton;

    public void Initialize()
    {
        this.editManager = FindObjectOfType<EditManager>();
        this.panelController = this.GetComponent<PanelController>();

        this.panelController.Opening += (s_, e_) =>
        {
            this.SetLabels();
        };
    }

    private void Update()
    {
        if (!this.panelController.IsOpen)
        {
            this.currentState = State.None;
            return;
        }

        this.SetLabels();

        if (this.currentState == State.SelectLevel)
        {
            if (!this.levelSelectorPanel.IsOpen)
            {
                var selected = this.levelSelectorPanel.SelectedLevel;
                if (this.editManager.CurrentLevel != selected)
                {
                    if (this.editManager.IsDirty)
                    {
                        this.confirmSavePanel.OpenPanel(this.editManager.CurrentLevel.DisplayName);

                        this.currentState = State.ConfirmSaveBeforeLoadLevel;
                    }
                    else if (selected == null)
                    {
                        // create new level
                        var name = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "HourglassDefaultName");
                        this.enterNamePanel.OpenPanel(name);

                        this.currentState = State.CreateNew;
                    }
                    else
                    {
                        if (!SaveManager.Instance.IsReviewRequested &&
                            this.editManager.ElapsedTime > ReviewRequestTime)
                        {
#if ENABLE_ANDROID_REVIEW
                            this.reviewRequestPanel.OpenPanel();
                            SaveManager.Instance.IsReviewRequested = true;
#else
                            AdManager.Instance.ShowInterstitialAd(true);
#endif
                        }
                        else
                        {
                            AdManager.Instance.ShowInterstitialAd(true);
                        }

                        // load level
                        this.editManager.LoadLevel(selected);
                        this.panelController.CloseAll();

                        this.currentState = State.None;
                    }
                }
                else
                {
                    this.currentState = State.None;
                }
            }
        }
        else if (this.currentState == State.SelectTimer)
        {
            if (!this.timerSelectorPanel.IsOpen)
            {
                var selected = this.timerSelectorPanel.SelectedTimer;
                if (this.editManager.SetTime != selected)
                {
                    this.editManager.SetTime = selected;
                    this.editManager.ResetLevel();
                }
            }
        }
        else if (this.currentState == State.ConfirmSaveBeforeLoadLevel)
        {
            if (!this.confirmSavePanel.IsOpen)
            {
                if (this.confirmSavePanel.Result == ConfirmSavePanel.PanelResult.None)
                {
                    // return to level selector
                    this.OpenLevelSelector();
                }
                else
                {
                    if (this.confirmSavePanel.Result == ConfirmSavePanel.PanelResult.Save)
                    {
                        this.editManager.SaveLevel();
                    }

                    var selected = this.levelSelectorPanel.SelectedLevel;
                    if (selected == null)
                    {
                        // create new level
                        var name = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "HourglassDefaultName");
                        this.enterNamePanel.OpenPanel(name);

                        this.currentState = State.CreateNew;
                    }
                    else
                    {
                        if (!SaveManager.Instance.IsReviewRequested &&
                            this.editManager.ElapsedTime > ReviewRequestTime)
                        {
#if ENABLE_ANDROID_REVIEW
                            this.reviewRequestPanel.OpenPanel();
                            SaveManager.Instance.IsReviewRequested = true;
#else
                            AdManager.Instance.ShowInterstitialAd(true);
#endif
                        }
                        else
                        {
                            AdManager.Instance.ShowInterstitialAd(true);
                        }

                        // load level
                        this.editManager.LoadLevel(selected);
                        this.panelController.CloseAll();

                        this.currentState = State.None;
                    }
                }
            }
        }
        else if (this.currentState == State.ConfirmSaveBeforeViewMode)
        {
            if (!this.confirmSavePanel.IsOpen)
            {
                if (this.confirmSavePanel.Result != ConfirmSavePanel.PanelResult.None)
                {
                    if (this.confirmSavePanel.Result == ConfirmSavePanel.PanelResult.Save)
                    {
                        this.editManager.SaveLevel();
                    }
                    else
                    {
                        // discard changes
                        this.editManager.LoadLevel(this.editManager.CurrentLevel);
                    }

                    this.editManager.IsEditMode = false;
                    this.panelController.CloseAll();
                    this.currentState = State.None;
                }
            }
        }
        else if (this.currentState == State.CreateNew)
        {
            if (!this.enterNamePanel.IsOpen)
            {
                if (this.enterNamePanel.Result == EnterNamePanel.PanelResult.OK &&
                    !string.IsNullOrEmpty(this.enterNamePanel.Text))
                {
                    this.editManager.CreateNewLevel(this.enterNamePanel.Text);
                    this.panelController.CloseAll();

                    this.editManager.IsEditMode = true;

                    this.currentState = State.None;
                }
                else
                {
                    this.OpenLevelSelector();
                }
            }
        }
    }

    public void OpenLevelSelector()
    {
        this.levelSelectorPanel.OpenSelector();

        this.currentState = State.SelectLevel;
    }

    public void OpenTimerSelector()
    {
        this.timerSelectorPanel.OpenSelectorWithTimer(this.editManager.SetTime);

        this.currentState = State.SelectTimer;
    }

    public void SwitchToEditMode()
    {
        this.editManager.IsEditMode = true;
        this.panelController.ClosePanel();
    }

    public void SwitchToViewMode()
    {
        if (this.editManager.IsDirty)
        {
            // open save confirm dialog
            this.confirmSavePanel.OpenPanel(this.editManager.CurrentLevel.DisplayName);
            this.currentState = State.ConfirmSaveBeforeViewMode;
            return;
        }

        this.editManager.IsEditMode = false;
        this.panelController.ClosePanel();
    }

    private void SetLabels()
    {
        this.editModeButton.gameObject.SetActive(!this.editManager.IsEditMode);
        this.viewModeButton.gameObject.SetActive(this.editManager.IsEditMode);

        var timer = this.timerSelectorPanel.GetLabelFromTimer(this.editManager.SetTime);
        this.timerSelectorButton.SetValue(timer);

        this.timerSelectorButton.SetEnable(!this.editManager.IsEditMode);
    }

    private enum State
    {
        None,
        SelectLevel,
        SelectTimer,
        ConfirmSaveBeforeLoadLevel,
        ConfirmSaveBeforeViewMode,
        CreateNew,
    }
}
