using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeEditCommand : IEditCommand
{
    private ThemeInfo before;

    public ThemeEditCommand(ThemeInfo before)
    {
        this.before = before;
    }

    public void Undo(EditManager editManager)
    {
        editManager.SetTheme(this.before);
    }
}
