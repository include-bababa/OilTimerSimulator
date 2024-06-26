using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSelectorPanel : MonoBehaviour
{
    private PanelController panelController;

    private List<ThemeSelectorElement> elements;
    private ThemeSelectorElement selected;

    [SerializeField]
    private GameObject contentView;

    [SerializeField]
    private GameObject elementPrefab;

    public ThemeInfo Selected => this.selected.ThemeInfo;

    public bool IsOpen => this.panelController.IsOpen;

    public void Initialize()
    {
        this.panelController = this.GetComponent<PanelController>();

        this.elements = new List<ThemeSelectorElement>();
        foreach (var theme in LevelManager.Instance.Themes)
        {
            var instance = Instantiate(this.elementPrefab, this.contentView.transform);
            var element = instance.GetComponent<ThemeSelectorElement>();
            element.InitializeFromPanel();

            element.SetValue(theme);

            this.elements.Add(element);
        }

        this.PlaceElements();
    }

    public void OpenSelector(ThemeInfo select)
    {
        this.selected = this.FindElement(select);

        foreach (var element in this.elements)
        {
            element.SetHighlight(element == this.selected);
        }

        this.panelController.OpenPanel();
        this.ScrollIntoView(this.selected);
    }

    public void Select(ThemeSelectorElement element)
    {
        this.selected = element;
        this.panelController.ClosePanel();
    }

    private void PlaceElements()
    {
        var margin = 8.0f;
        var y = 0.0f;
        foreach (var element in this.elements)
        {
            var rectTransform = element.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -y);

            y += rectTransform.rect.height + margin;
        }

        var contentRectTransform = this.contentView.GetComponent<RectTransform>();
        var sizeDelta = contentRectTransform.sizeDelta;
        sizeDelta.y = y - margin;
        contentRectTransform.sizeDelta = sizeDelta;
    }

    private ThemeSelectorElement FindElement(ThemeInfo theme)
    {
        foreach (var element in this.elements)
        {
            if (element.ThemeInfo == theme)
            {
                return element;
            }
        }

        return this.elements[0];
    }

    private void ScrollIntoView(ThemeSelectorElement element)
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
}
