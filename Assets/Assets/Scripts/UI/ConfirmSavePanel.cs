using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ConfirmSavePanel : MonoBehaviour
{
    public enum PanelResult
    {
        None,
        Save,
        Discard,
    }

    private PanelController panelController;

    [SerializeField]
    private TextMeshProUGUI messageTextMesh;

    public bool IsOpen => this.panelController.IsOpen;

    public string DisplayName { get; set; }

    public PanelResult Result { get; private set; } = PanelResult.None;

    public void Initialize()
    {
        this.panelController = this.GetComponent<PanelController>();
        this.panelController.Opening += (s_, e_) =>
        {
            this.Result = PanelResult.None;
            this.SetLabels();
        };
    }

    public void OpenPanel(string displayName)
    {
        this.DisplayName = displayName;
        this.panelController.OpenPanel();
    }

    public void Save()
    {
        this.Result = PanelResult.Save;
        this.panelController.ClosePanel();
    }

    public void Discard()
    {
        this.Result = PanelResult.Discard;
        this.panelController.ClosePanel();
    }

    public void ClearResult()
    {
        this.Result = PanelResult.None;
    }

    private void SetLabels()
    {
        var name = this.DisplayName;
        var dict = new Dictionary<string, string> { { "level_name", name } };
        var arguments = new object[] { dict };

        var message = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "DialogMessageConfirmSaveBeforeUnload", arguments);
        this.messageTextMesh.text = message;
    }
}
