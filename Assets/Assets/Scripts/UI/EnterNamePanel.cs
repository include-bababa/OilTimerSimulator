using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterNamePanel : MonoBehaviour
{
    public enum PanelResult
    {
        None,
        OK,
        Cancel,
    }

    private PanelController panelController;

    private string text;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private Button submitButton;

    public bool IsOpen => this.panelController.IsOpen;

    public string Text => this.text;

    public PanelResult Result { get; private set; } = PanelResult.None;

    public void Initialize()
    {
        this.panelController = this.GetComponent<PanelController>();
        this.panelController.Opening += (s_, e_) =>
        {
            this.Result = PanelResult.None;
        };
    }

    private void LateUpdate()
    {
        this.submitButton.interactable = !string.IsNullOrEmpty(this.inputField.text);
    }

    public void OpenPanel(string text)
    {
        this.text = text;
        this.inputField.text = text;

        this.panelController.OpenPanel();
    }

    public void Submit()
    {
        this.Result = PanelResult.OK;
        this.text = this.inputField.text;
        this.panelController.ClosePanel();
    }

    public void Cancel()
    {
        this.Result = PanelResult.Cancel;
        this.text = string.Empty;
        this.panelController.ClosePanel();
    }
}
