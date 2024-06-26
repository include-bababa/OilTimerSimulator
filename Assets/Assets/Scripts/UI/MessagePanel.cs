using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagePanel : MonoBehaviour
{
    public enum MessageResult
    {
        None,
        OK,
        Cancel,
    }

    private PanelController panelController;

    [SerializeField]
    private TextMeshProUGUI titleTextMesh;

    [SerializeField]
    private TextMeshProUGUI messageTextMesh;

    [SerializeField]
    private TextMeshProUGUI okTextMesh;

    [SerializeField]
    private TextMeshProUGUI cancelTextMesh;

    public bool IsOpen => this.panelController.IsOpen;

    public string Title { get; set; }

    public string Message { get; set; }

    public string OKLabel { get; set; } = "OK";

    public string CancelLabel { get; set; } = "Cancel";

    public bool IsCancelEnabled { get; set; } = true;

    public MessageResult Result { get; private set; } = MessageResult.None;

    public void Initialize()
    {
        this.panelController = this.GetComponent<PanelController>();
        this.panelController.Opening += (s_, e_) =>
        {
            this.SetLabels();
        };

        this.SetLabels();
    }

    public void OpenPanel()
    {
        this.panelController.OpenPanel();
    }

    public void Confirm()
    {
        this.Result = MessageResult.OK;
        this.panelController.ClosePanel();
    }

    public void Cancel()
    {
        this.Result = MessageResult.Cancel;
        this.panelController.ClosePanel();
    }

    public void ClearResult()
    {
        this.Result = MessageResult.None;
    }

    private void SetLabels()
    {
        this.titleTextMesh.text = this.Title;
        this.messageTextMesh.text = this.Message;
        this.okTextMesh.text = this.OKLabel;
        this.cancelTextMesh.text = this.CancelLabel;

        this.cancelTextMesh.transform.parent.gameObject.SetActive(this.IsCancelEnabled);
    }
}
