using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilColorEditCommand : IEditCommand
{
    private OilType target;
    private Color? before;

    public enum OilType
    {
        Main,
        Sub,
    }

    public OilColorEditCommand(OilType oil, Color? before)
    {
        this.target = oil;
        this.before = before;
    }

    public void Undo(EditManager editManager)
    {
        if (this.target == OilType.Main)
        {
            editManager.OilPoolManager.SetMainColor(this.before ?? Color.white);
        }
        else if (this.target == OilType.Sub)
        {
            if (this.before.HasValue)
            {
                editManager.OilPoolManager.SetSubColor(this.before.Value);
            }
            else
            {
                editManager.OilPoolManager.UnsetSubColor();
            }
        }
    }
}
