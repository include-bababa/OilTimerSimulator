using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class IntervalSelectorPanel : TextSelectorPanelBase
{
    private List<IntervalElement> elements;

    public float SelectedInterval => this.elements[this.Selected].Value;

    public override void Initialize()
    {
        var unitStr = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "UnitSecondShort");

        this.elements = new List<IntervalElement>();
        this.elements.Add(new IntervalElement(1.5f, $"1.5 {unitStr}"));
        this.elements.Add(new IntervalElement(2.0f, $"2.0 {unitStr}"));
        this.elements.Add(new IntervalElement(2.5f, $"2.5 {unitStr}"));
        this.elements.Add(new IntervalElement(3.0f, $"3.0 {unitStr}"));
        this.elements.Add(new IntervalElement(3.5f, $"3.5 {unitStr}"));
        this.elements.Add(new IntervalElement(4.0f, $"4.0 {unitStr}"));
        this.elements.Add(new IntervalElement(4.5f, $"4.5 {unitStr}"));
        this.elements.Add(new IntervalElement(5.0f, $"5.0 {unitStr}"));
        this.elements.Add(new IntervalElement(5.5f, $"5.5 {unitStr}"));
        this.elements.Add(new IntervalElement(6.0f, $"6.0 {unitStr}"));

        base.Initialize();
    }

    public string GetLabelFromInterval(float interval)
    {
        foreach (var element in this.elements)
        {
            if (Mathf.Approximately(interval, element.Value))
            {
                return element.DisplayName;
            }
        }

        return string.Empty;
    }

    public void OpenSelectorWithInterval(float interval)
    {
        var selected = 0;
        for (int index = 0; index < this.elements.Count; index++)
        {
            var element = this.elements[index];
            if (Mathf.Approximately(interval, element.Value))
            {
                selected = index;
                break;
            }
        }

        this.OpenSelector(selected);
    }

    protected override string[] GetValues()
    {
        var num = this.elements.Count;
        var ret = new string[num];
        for (var index = 0; index < num; index++)
        {
            ret[index] = this.elements[index].DisplayName;
        }

        return ret;
    }

    private struct IntervalElement
    {
        public IntervalElement(float val, string name)
        {
            this.Value = val;
            this.DisplayName = name;
        }

        public float Value { get; set; }

        public string DisplayName { get; set; }
    }
}
