using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class TimerSelectorPanel : TextSelectorPanelBase
{
    private List<TimerElement> elements;

    public int SelectedTimer => this.elements[this.Selected].Value;

    public override void Initialize()
    {
        var unitMinStr = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "UnitMinuteShort");
        var noneStr = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "TimerNone");
        var stopwatchStr = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "TimerStopwatch");

        this.elements = new List<TimerElement>();
        this.elements.Add(new TimerElement(-1, noneStr));
        this.elements.Add(new TimerElement(0, stopwatchStr));
#if UNITY_EDITOR
        this.elements.Add(new TimerElement(30, "30 sec"));
#endif
        this.elements.Add(new TimerElement(3 * 60, $"3 {unitMinStr}"));
        this.elements.Add(new TimerElement(4 * 60, $"4 {unitMinStr}"));
        this.elements.Add(new TimerElement(5 * 60, $"5 {unitMinStr}"));
        this.elements.Add(new TimerElement(6 * 60, $"6 {unitMinStr}"));
        this.elements.Add(new TimerElement(7 * 60, $"7 {unitMinStr}"));
        this.elements.Add(new TimerElement(8 * 60, $"8 {unitMinStr}"));
        this.elements.Add(new TimerElement(9 * 60, $"9 {unitMinStr}"));
        this.elements.Add(new TimerElement(10 * 60, $"10 {unitMinStr}"));
        this.elements.Add(new TimerElement(15 * 60, $"15 {unitMinStr}"));
        this.elements.Add(new TimerElement(20 * 60, $"20 {unitMinStr}"));
        this.elements.Add(new TimerElement(25 * 60, $"25 {unitMinStr}"));
        this.elements.Add(new TimerElement(30 * 60, $"30 {unitMinStr}"));
        this.elements.Add(new TimerElement(40 * 60, $"40 {unitMinStr}"));
        this.elements.Add(new TimerElement(50 * 60, $"50 {unitMinStr}"));
        this.elements.Add(new TimerElement(60 * 60, $"60 {unitMinStr}"));
        this.elements.Add(new TimerElement(90 * 60, $"90 {unitMinStr}"));

        base.Initialize();
    }

    public string GetLabelFromTimer(int seconds)
    {
        foreach (var element in this.elements)
        {
            if (element.Value == seconds)
            {
                return element.DisplayName;
            }
        }

        return string.Empty;
    }

    public void OpenSelectorWithTimer(int seconds)
    {
        var selected = 0;
        for (int index = 0; index < this.elements.Count; index++)
        {
            var element = this.elements[index];
            if (element.Value == seconds)
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

    private struct TimerElement
    {
        public TimerElement(int val, string name)
        {
            this.Value = val;
            this.DisplayName = name;
        }

        public int Value { get; set; }

        public string DisplayName { get; set; }
    }
}
