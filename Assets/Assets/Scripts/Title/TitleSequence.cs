using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSequence : InputObserverBase
{
    private UIManager uiManager;

    private State state;
    private float waitTimer;

    [SerializeField]
    private PanelController infoPanel;

    private void Start()
    {
        this.uiManager = FindObjectOfType<UIManager>();
        this.state = State.WaitSplash;
        this.waitTimer = 0.1f;
    }

    private void Update()
    {
        if (this.state == State.WaitSplash)
        {
            if (!this.uiManager.IsFading && SplashScreen.isFinished)
            {
                this.waitTimer -= Time.deltaTime;
                if (this.waitTimer < 0)
                {
                    this.state = State.None;
                }
            }
        }
        else if (this.state == State.Proceeded)
        {
            if (!this.uiManager.IsFading)
            {
                // change scene
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void Proceed()
    {
        if (this.state == State.Proceeded || this.state == State.WaitSplash)
        {
            return;
        }

        this.uiManager.StartFadeOut();
        this.state = State.Proceeded;
    }

    public void OpenInfoPanel()
    {
        if (this.state == State.Proceeded || this.state == State.WaitSplash)
        {
            return;
        }

        this.infoPanel.OpenPanel();

        this.state = State.ShowInfo;
    }

    public override bool ProcessInput(InputType inputType)
    {
        if (inputType == InputType.Back)
        {
            ApplicationHelper.MoveTaskToBack();
        }

        return true;
    }

    private enum State
    {
        None,
        WaitSplash,
        Proceeded,
        ShowInfo,
    }
}
