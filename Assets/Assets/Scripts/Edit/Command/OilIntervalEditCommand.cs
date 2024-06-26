using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilIntervalEditCommand : IEditCommand
{
    private OilType target;
    private float before;

    public enum OilType
    {
        Main,
        Sub,
    }

    public OilIntervalEditCommand(OilType oil, float before)
    {
        this.target = oil;
        this.before = before;
    }

    public void Undo(EditManager editManager)
    {
        if (this.target == OilType.Main)
        {
            editManager.OilPoolManager.MainSpawner.DropInterval = this.before;
        }
        else if (this.target == OilType.Sub)
        {
            editManager.OilPoolManager.SubSpawner.DropInterval = this.before;
        }
    }
}
