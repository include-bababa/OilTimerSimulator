using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationEditCommand : IEditCommand
{
    private Guid guid;

    public CreationEditCommand(Guid target)
    {
        this.guid = target;
    }

    public void Undo(EditManager editManager)
    {
        var target = editManager.FindProp(this.guid);
        editManager.Delete(target);
    }
}
