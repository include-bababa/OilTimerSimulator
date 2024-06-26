using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TextSelectorPanelBase : MonoBehaviour
{
    private PanelController panelController;

    private List<TextSelectorElement> elements;
    private TextSelectorElement selected;

    [SerializeField]
    private GameObject contentView;

    [SerializeField]
    private GameObject elementPrefab;

    public int Selected => this.selected.Value;

    public bool IsOpen => this.panelController.IsOpen;

    public virtual void Initialize()
    {
        this.panelController = this.GetComponent<PanelController>();

        var margin = 8.0f;
        var diff = this.elementPrefab.GetComponent<RectTransform>().rect.height + margin;

        this.elements = new List<TextSelectorElement>();
        var y = 0.0f;
        var index = 0;
        foreach (var val in this.GetValues())
        {
            var instance = Instantiate(this.elementPrefab, this.contentView.transform);
            var element = instance.GetComponent<TextSelectorElement>();
            instance.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -y);
            element.InitializeFromPanel();
            element.SetValue(val, index);

            this.elements.Add(element);
            y += diff;
            index++;
        }

        var rectTransform = this.contentView.GetComponent<RectTransform>();
        var sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = y - margin;
        rectTransform.sizeDelta = sizeDelta;
    }

    public void OpenSelector(int val)
    {
        this.selected = this.FindElement(val);

        foreach (var element in this.elements)
        {
            element.SetHighlight(element == this.selected);
        }

        this.panelController.OpenPanel();
    }

    public void Select(TextSelectorElement element)
    {
        this.selected = element;
        this.panelController.ClosePanel();
    }

    public string GetLabel(int val)
    {
        var element = this.FindElement(val);
        return element.Label;
    }

    protected abstract string[] GetValues();

    private TextSelectorElement FindElement(int val)
    {
        foreach (var element in this.elements)
        {
            if (element.Value == val)
            {
                return element;
            }
        }

        return this.elements[0];
    }
}
