using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if FIREBASE_USE_TEST_LOOP

public interface ITestSequence
{
    string Name { get; }

    bool IsFinished { get; }

    void Start();

    void Update(float deltaTime);
}

#endif
