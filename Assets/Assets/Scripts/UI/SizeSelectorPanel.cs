using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSelectorPanel : TextSelectorPanelBase
{
    public ScaleGrade SelectedScale => (ScaleGrade)this.Selected;

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override string[] GetValues()
    {
        var num = LevelManager.Instance.NumScaleData;
        var ret = new string[num];
        for (var index = 0; index < num; index++)
        {
            var data = LevelManager.Instance.GetScaleData((ScaleGrade)index);
            ret[index] = data.DisplayName.GetLocalizedString();
        }

        return ret;
    }
}
