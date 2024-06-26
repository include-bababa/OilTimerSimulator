using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSelectorPanel : TextSelectorPanelBase
{
    [SerializeField]
    private string[] values;

    protected override string[] GetValues()
    {
        return this.values;
    }
}
