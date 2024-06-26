using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if FIREBASE_USE_TEST_LOOP

public class SimpleTestSequence : ITestSequence
{
    private State state;
    private float stateTimer;

    public string Name => "SimpleTest";

    public bool IsFinished => this.state == State.Finished;

    public void Start()
    {
        this.ChangeState(State.Title);
    }

    public void Update(float deltaTime)
    {
        var prevTimer = this.stateTimer;
        this.stateTimer += deltaTime;
        switch (this.state)
        {
            case State.Title:
                {
                    // process to main scene
                    if (this.stateTimer > 5.0f &&
                        SceneManager.GetActiveScene().name != "TitleScreen")
                    {
                        this.ChangeState(State.Main);
                    }

                    if ((int)(this.stateTimer / 5) != (int)(prevTimer / 5))
                    {
                        var title = Object.FindObjectOfType<TitleSequence>();
                        title?.Proceed();
                    }
                }
                break;
            case State.Main:
                {
                    // wait 10 seconds
                    if (this.stateTimer > 10.0f)
                    {
                        this.ChangeState(State.ToggleUI);
                    }
                }
                break;
            case State.ToggleUI:
                {
                    if ((int)(this.stateTimer / 5) != (int)(prevTimer / 5))
                    {
                        var editManager = Object.FindObjectOfType<EditManager>(true);
                        editManager?.SetUIVisibilityForViewMode(!editManager.IsUIVisible);
                    }

                    if (this.stateTimer > 20.0f)
                    {
                        this.ChangeState(State.Finished);
                    }
                }
                break;
            case State.Finished:
                break;
        }
    }

    private void ChangeState(State state)
    {
        this.state = state;
        this.stateTimer = 0.0f;
    }

    private enum State
    {
        Title,
        Main,
        ToggleUI,
        Finished,
    }
}

#endif
