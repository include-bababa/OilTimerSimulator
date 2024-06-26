using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    private EditManager editManager;

    private State state;

    [SerializeField]
    private TextMeshProUGUI minute;

    [SerializeField]
    private TextMeshProUGUI colon;

    [SerializeField]
    private TextMeshProUGUI second;

    [SerializeField]
    private Button button;

    [SerializeField]
    private TimerSelectorPanel timerSelectorPanel;

    private void Start()
    {
        this.editManager = FindObjectOfType<EditManager>(true);
        this.state = State.None;
    }

    private void Update()
    {
        if (this.state == State.SelectTimer)
        {
            if (!this.timerSelectorPanel.IsOpen)
            {
                var selected = this.timerSelectorPanel.SelectedTimer;
                if (this.editManager.SetTime != selected)
                {
                    this.editManager.SetTime = selected;
                    this.editManager.ResetLevel();
                }

                this.state = State.None;
            }
        }
    }

    private void LateUpdate()
    {
        if (this.editManager.IsEditMode || this.editManager.SetTime < 0)
        {
            this.minute.gameObject.SetActive(false);
            this.colon.gameObject.SetActive(false);
            this.second.gameObject.SetActive(false);
            this.button.gameObject.SetActive(false);
            return;
        }

        this.minute.gameObject.SetActive(true);
        this.second.gameObject.SetActive(true);
        this.button.gameObject.SetActive(true);

        var time = this.editManager.ElapsedTime;
        if (this.editManager.SetTime > 0)
        {
            time = Mathf.Max(this.editManager.SetTime - time, 0.0f);
        }

        var totalSeconds = Mathf.CeilToInt(time);
        var minutes = totalSeconds / 60;
        var seconds = totalSeconds - minutes * 60;

        // var showColon = totalSeconds - time < 0.5f;
        // this.colon.gameObject.SetActive(showColon);
        this.colon.gameObject.SetActive(true);

        this.minute.text = minutes.ToString("00");
        this.second.text = seconds.ToString("00");

        var color = Color.white;
        if (this.editManager.SetTime > 0 && this.editManager.SetTime < this.editManager.ElapsedTime)
        {
            color = new Color(1.0f, 0.25f, 0.25f, 1.0f);
        }

        this.minute.color = color;
        this.colon.color = color;
        this.second.color = color;
    }

    public void OpenTimerSelector()
    {
        this.timerSelectorPanel.OpenSelectorWithTimer(this.editManager.SetTime);

        this.state = State.SelectTimer;
    }

    private enum State
    {
        None,
        SelectTimer,
    }
}
