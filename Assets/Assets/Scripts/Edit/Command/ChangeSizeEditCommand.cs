using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSizeEditCommand : IEditCommand
{
    private Guid guid;
    private ScaleGrade before;

    public ChangeSizeEditCommand(Guid target, ScaleGrade before)
    {
        this.guid = target;
        this.before = before;
    }

    public void Undo(EditManager editManager)
    {
        var target = editManager.FindProp(this.guid);
        target.ScaleSettter.SetScale(before);
    }
}
