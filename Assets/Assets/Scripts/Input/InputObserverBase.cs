using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputObserverBase : MonoBehaviour
{
    public enum InputType
    {
        Back,
    }

    public abstract bool ProcessInput(InputType inputType);
}
