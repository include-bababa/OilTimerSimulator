using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.TestLab;

#if FIREBASE_USE_TEST_LOOP

public class TestSequenceManager
{
    public static TestSequenceManager Instance { get; private set; }

    private TestLabManager testLabManager;
    private int currentScenario;
    private ITestSequence currentSequence;
    private bool isStarted;

    public static void CreateInstance()
    {
        Instance = new TestSequenceManager();
    }

    public TestSequenceManager()
    {
        this.testLabManager = TestLabManager.Instantiate();
        this.currentScenario = -1;
        this.isStarted = false;
    }

    public ITestSequence CurrentTest => this.currentSequence;

    public void StartTest()
    {
        this.isStarted = true;
    }

    public void Update(float deltaTime)
    {
        if (!this.isStarted)
        {
            return;
        }

        var scenario = this.testLabManager.ScenarioNumber;
        if (this.currentScenario != scenario)
        {
            Debug.Log($"[Test] Running scenario #{scenario}...");
            this.currentScenario = scenario;

            switch (scenario)
            {
                case 1:
                case 2:
                    this.currentSequence = new SimpleTestSequence();
                    break;
                default:
                    this.currentSequence = null;
                    break;
            }

            this.currentSequence?.Start();
        }

        if (this.currentSequence != null)
        {
            this.currentSequence.Update(deltaTime);
            if (this.currentSequence.IsFinished)
            {
                Debug.Log($"[Test] Scenario #{this.currentScenario} completed.");
                this.testLabManager.NotifyHarnessTestIsComplete();
            }
        }
    }
}

#endif
