using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class ColorSelectorPanel : MonoBehaviour
{
    private PanelController panelController;

    private List<ColorSelectorElement> elements;
    private ColorSelectorElement selected;

    [SerializeField]
    private GameObject contentView;

    [SerializeField]
    private GameObject elementPrefab;

    public Color Selected => this.selected.Color;

    public bool IsOpen => this.panelController.IsOpen;

    public void Initialize()
    {
        this.panelController = this.GetComponent<PanelController>();

        var elementInfos = new ElementInfo[]
        {
            new ElementInfo("None", new Color(0.0f, 0.0f, 0.0f, 0.0f)),
            new ElementInfo("White", Color.white),
            new ElementInfo("Silver", new Color(0.75f, 0.75f, 0.75f)),
            new ElementInfo("Gray", Color.gray),
            new ElementInfo("Black", Color.black),
            new ElementInfo("Red", Color.red),
            new ElementInfo("Brown", new Color(0.5f, 0.0f, 0.0f)),
            new ElementInfo("Yellow", Color.yellow),
            new ElementInfo("Olive", new Color(0.5f, 0.5f, 0.0f)),
            new ElementInfo("Lime", Color.green),
            new ElementInfo("Green", new Color(0.0f, 0.5f, 0.0f)),
            new ElementInfo("Cyan", Color.cyan),
            new ElementInfo("Teal", new Color(0.0f, 0.5f, 0.5f)),
            new ElementInfo("Blue", Color.blue),
            new ElementInfo("Navy", new Color(0.0f, 0.0f, 0.5f)),
            new ElementInfo("Magenta", Color.magenta),
            new ElementInfo("Purple", new Color(0.5f, 0.0f, 0.5f)),
        };

        this.elements = new List<ColorSelectorElement>();
        foreach (var info in elementInfos)
        {
            var instance = Instantiate(this.elementPrefab, this.contentView.transform);
            var element = instance.GetComponent<ColorSelectorElement>();
            element.InitializeFromPanel();

            var label = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "ColorName" + info.Label);
            element.SetValue(label, info.Color);

            this.elements.Add(element);
        }

        this.PlaceElements(true);
    }

    public void OpenSelector(Color select, bool useNone)
    {
        this.PlaceElements(useNone);

        this.selected = this.FindElement(select);

        foreach (var element in this.elements)
        {
            element.SetHighlight(element == this.selected);
        }

        this.panelController.OpenPanel();
        this.ScrollIntoView(this.selected);
    }

    public void Select(ColorSelectorElement element)
    {
        this.selected = element;
        this.panelController.ClosePanel();
    }

    private void PlaceElements(bool useNone)
    {
        var margin = 8.0f;
        var y = 0.0f;
        foreach (var element in this.elements)
        {
            if (!element.HasColor)
            {
                if (!useNone)
                {
                    element.gameObject.SetActive(false);
                    continue;
                }
                else
                {
                    element.gameObject.SetActive(true);
                }
            }

            var rectTransform = element.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -y);

            y += rectTransform.rect.height + margin;
        }

        var contentRectTransform = this.contentView.GetComponent<RectTransform>();
        var sizeDelta = contentRectTransform.sizeDelta;
        sizeDelta.y = y - margin;
        contentRectTransform.sizeDelta = sizeDelta;
    }

    private ColorSelectorElement FindElement(Color color)
    {
        foreach (var element in this.elements)
        {
            if (element.Color == color)
            {
                return element;
            }
        }

        return this.elements[0];
    }

    private void ScrollIntoView(ColorSelectorElement element)
    {
        var contentRect = this.contentView.GetComponent<RectTransform>();
        var viewportRect = this.contentView.transform.parent.GetComponent<RectTransform>();

        var pos = contentRect.anchoredPosition;
        var viewportHeight = viewportRect.rect.height;
        var yMin = -pos.y - viewportHeight;
        var yMax = -pos.y;

        var elementRect = element.GetComponent<RectTransform>();

        if (elementRect.offsetMax.y < yMin)
        {
            var diff = yMin - elementRect.offsetMin.y;
            contentRect.anchoredPosition += new Vector2(0, diff);
        }
        else if (elementRect.offsetMin.y > yMax)
        {
            var diff = yMax - elementRect.offsetMax.y;
            contentRect.anchoredPosition += new Vector2(0, diff);
        }
    }

    private class ElementInfo
    {
        public ElementInfo(string label, Color color)
        {
            this.Label = label;
            this.Color = color;
        }

        public string Label { get; }

        public Color Color { get; }
    }
}
