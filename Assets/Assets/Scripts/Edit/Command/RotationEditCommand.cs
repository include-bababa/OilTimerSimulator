using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEditCommand : IEditCommand
{
    private Guid guid;
    private float from;

    public RotationEditCommand(Guid target, float from)
    {
        this.guid = target;
        this.from = from;
    }

    public void Undo(EditManager editManager)
    {
        var target = editManager.FindProp(this.guid);
        target.transform.rotation = Quaternion.Euler(0, 0, from);
    }
}
