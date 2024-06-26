using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationEditCommand : IEditCommand
{
    private Guid guid;
    private Vector3 from;

    public TranslationEditCommand(Guid target, Vector3 from)
    {
        this.guid = target;
        this.from = from;
    }

    public void Undo(EditManager editManager)
    {
        var target = editManager.FindProp(this.guid);
        target.transform.position = this.from;
    }
}
