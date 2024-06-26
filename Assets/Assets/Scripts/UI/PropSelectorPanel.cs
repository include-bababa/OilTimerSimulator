using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSelectorPanel : TextSelectorPanelBase
{
    public PropType SelectedProp => (PropType)this.Selected;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override string[] GetValues()
    {
        var num = LevelManager.Instance.Props.Length;
        var ret = new string[num];
        for (var index = 0; index < num; index++)
        {
            var data = LevelManager.Instance.GetPropData((PropType)index);
            ret[index] = data.DisplayName.GetLocalizedString();
        }

        return ret;
    }
}
