using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRotationEditCommand : IEditCommand
{
    private Guid guid;
    private RotationDirection before;

    public ChangeRotationEditCommand(Guid target, RotationDirection before)
    {
        this.guid = target;
        this.before = before;
    }

    public void Undo(EditManager editManager)
    {
        var target = editManager.FindProp(this.guid);
        target.RotationSetter.SetRotation(before);
    }
}
