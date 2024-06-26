using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEditCommand
{
    void Undo(EditManager editManager);
}
