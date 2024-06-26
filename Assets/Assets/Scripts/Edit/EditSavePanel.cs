using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class EditSavePanel : MonoBehaviour
{
    private EditManager editManager;
    private PanelController panelController;

    private State currentState;

    private string lastUpdatePrefix;

    [SerializeField]
    private LevelSelectorPanel levelSelectorPanel;

    [SerializeField]
    private EnterNamePanel enterNamePanel;

    [SerializeField]
    private MessagePanel messagePanel;

    [SerializeField]
    private ConfirmSavePanel confirmSavePanel;

    [SerializeField]
    private KeyValueButton nameButton;

    [SerializeField]
    private TextMeshProUGUI lastUpdateText;

    public void Initialize()
    {
        this.editManager = FindObjectOfType<EditManager>(true);

        this.nameButton.Initialize();
    }

    private void Awake()
    {
        this.panelController = this.GetComponentInParent<PanelController>();
        this.panelController.Opening += (s_, e_) =>
        {
            this.SetLabels();
        };

        this.lastUpdatePrefix = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "SampleScene/UI/Canvas/EditorMiniPanel/EditorMiniPanel/SaveContent/LastSaved");

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
                if (this.enterNamePanel.Result == EnterNamePanel.PanelResult.OK &&
                    !string.IsNullOrEmpty(this.enterNamePanel.Text))
                {
                    this.editManager.CurrentLevel.DisplayName = this.enterNamePanel.Text;
                    this.SetLabels();
                }

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
                    this.SetLabels();

                    this.OpenSaveCompletedPanel();
                }
                else
                {
                    this.currentState = State.None;
                }
            }
        }
        else if (this.currentState == State.SaveCompleted)
        {
            if (!this.messagePanel.IsOpen)
            {
                this.SetLabels();
                this.currentState = State.None;
            }
        }
        else if (this.currentState == State.SelectLoadLevel)
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
        else if (this.currentState == State.ConfirmSaveBeforeLoadLevel)
        {
            if (!this.confirmSavePanel.IsOpen)
            {
                if (this.confirmSavePanel.Result == ConfirmSavePanel.PanelResult.None)
                {
                    // return to level selector
                    this.OpenLoadPanel();
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
                        // load level
                        this.editManager.LoadLevel(selected);
                        this.panelController.CloseAll();

                        this.currentState = State.None;
                    }

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
                }

                this.currentState = State.None;
            }
        }
    }

    public void OpenEnterNamePanel()
    {
        var name = this.editManager.CurrentLevel.DisplayName;
        this.enterNamePanel.OpenPanel(name);

        this.currentState = State.EnterName;
    }

    public void OpenSavePanel()
    {
        var name = this.editManager.CurrentLevel.DisplayName;
        var dict = new Dictionary<string, string> { { "level_name", name } };
        var arguments = new object[] { dict };

        this.messagePanel.Title = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "SampleScene/UI/Canvas/EnterNamePanel/EnterNamePanel/H1-Title/Text (TMP)");
        this.messagePanel.Message = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "DialogMessageSaveConfirm", arguments);
        this.messagePanel.IsCancelEnabled = true;
        this.messagePanel.OpenPanel();

        this.currentState = State.Save;
    }

    private void OpenSaveCompletedPanel()
    {
        var name = this.editManager.CurrentLevel.DisplayName;
        var dict = new Dictionary<string, string> { { "level_name", name } };
        var arguments = new object[] { dict };

        this.messagePanel.Title = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "SampleScene/UI/Canvas/EnterNamePanel/EnterNamePanel/H1-Title/Text (TMP)");
        this.messagePanel.Message = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "DialogMessageSaveCompleted", arguments);
        this.messagePanel.IsCancelEnabled = false;
        this.messagePanel.OpenPanel();

        this.currentState = State.SaveCompleted;
    }

    public void OpenLoadPanel()
    {
        this.levelSelectorPanel.OpenSelector();

        this.currentState = State.SelectLoadLevel;
    }

    private void SetLabels()
    {
        this.nameButton.SetValue(this.editManager.CurrentLevel.DisplayName);

        if (this.editManager.CurrentLevel.LastUpdate <= DateTime.MinValue)
        {
            this.lastUpdateText.text = string.Empty;
        }
        else
        {
            var lastUpdate = this.editManager.CurrentLevel.LastUpdate.ToString();
            var text = this.lastUpdatePrefix + lastUpdate;
            this.lastUpdateText.text = text;
        }
    }

    private enum State
    {
        None,
        EnterName,
        Save,
        SaveCompleted,
        SelectLoadLevel,
        ConfirmSaveBeforeLoadLevel,
        CreateNew,
    }
}
