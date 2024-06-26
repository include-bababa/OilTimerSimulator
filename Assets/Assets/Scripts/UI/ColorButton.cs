using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    private Image backgroundImage;

    private Color backgroundDefaultColor;

    [SerializeField]
    private TextMeshProUGUI emptyText;

    [SerializeField]
    private Image valueImage;

    public void Initialize()
    {
        if (this.backgroundImage == null)
        {
            this.backgroundImage = this.GetComponent<Image>();
            this.backgroundDefaultColor = this.backgroundImage.color;
        }
    }

    private void Awake()
    {
        this.Initialize();
    }

    public void UnsetValue()
    {
        this.emptyText?.gameObject.SetActive(true);
        this.valueImage.gameObject.SetActive(false);
    }

    public void SetValue(Color color)
    {
        this.emptyText?.gameObject.SetActive(false);
        this.valueImage.gameObject.SetActive(true);
        this.valueImage.color = color;
    }

    public void SetHighlight(bool flag)
    {
        var color = flag ?
            Constants.GetHighlightColor(this.backgroundDefaultColor.a) :
            this.backgroundDefaultColor;
        this.backgroundImage.color = color;
    }
}
