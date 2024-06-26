using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabItem : MonoBehaviour
{
    private Image backgroundImage;

    private Color backgroundDefaultColor;

    [SerializeField]
    private GameObject activeObject;

    public void Initialize()
    {
        if (this.backgroundImage == null)
        {
            this.backgroundImage = this.GetComponent<Image>();
            this.backgroundDefaultColor = this.backgroundImage?.color ?? Color.white;
        }
    }

    private void Awake()
    {
        this.Initialize();
    }

    public void SetHighlight(bool flag)
    {
        var color = flag ?
            Constants.GetHighlightColor(this.backgroundDefaultColor.a) :
            this.backgroundDefaultColor;
        this.backgroundImage.color = color;

        this.activeObject.SetActive(flag);
    }
}
