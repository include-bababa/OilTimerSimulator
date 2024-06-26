using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewModeBackground : MonoBehaviour, IPointerDownHandler
{
    private EditManager editManager;
    private Image image;

    private CanvasScaler canvasScaler;
    private RectTransform uiRootRectTransform;

    [SerializeField]
    private GameObject uiRoot;

    private void Awake()
    {
        this.editManager = FindObjectOfType<EditManager>(true);
        this.image = this.GetComponent<Image>();
    }

    private void Start()
    {
        this.canvasScaler = this.uiRoot.GetComponentInParent<CanvasScaler>();
        this.uiRootRectTransform = this.uiRoot.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        this.image.enabled = !this.editManager.IsEditMode;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        var screenSize = new Vector2(Screen.width, Screen.height);
        var scaleFactor = screenSize.x / this.canvasScaler.referenceResolution.x;
        var marginSize = new Vector2(0, 256.0f * scaleFactor);
        var rectMin = this.uiRootRectTransform.offsetMin + marginSize;
        var rectMax = screenSize + this.uiRootRectTransform.offsetMax * scaleFactor - marginSize;
        var invalidRect = new Rect(rectMin, rectMax - rectMin);
        if (invalidRect.Contains(eventData.position))
        {
            return;
        }
        */
        this.ToggleUIVisibility();
    }

    private void ToggleUIVisibility()
    {
        if (this.editManager.IsEditMode)
        {
            return;
        }

        this.editManager.SetUIVisibilityForViewMode(!this.editManager.IsUIVisible);
    }
}
