using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorEditCommand : IEditCommand
{
    private Guid guid;
    private Color before;

    public ChangeColorEditCommand(Guid target, Color before)
    {
        this.guid = target;
        this.before = before;
    }

    public void Undo(EditManager editManager)
    {
        var target = editManager.FindProp(this.guid);
        target.ColorSetter.SetColor(this.before);
    }
}
