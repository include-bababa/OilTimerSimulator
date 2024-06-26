using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    const float ViewWidth = 1.89f;
    const float ViewHeight = 3.35f;

    private Camera cameraComponent;

    private CanvasScaler canvasScaler;
    private RectTransform uiRootRectTransform;

    private float shakeTimer;

    [SerializeField]
    private int screenTopMargin = 90;

    [SerializeField]
    private SpriteRenderer background;

    [SerializeField]
    private GameObject uiRoot;

    private void Start()
    {
        this.cameraComponent = this.GetComponent<Camera>();
        this.canvasScaler = this.uiRoot.GetComponentInParent<CanvasScaler>();
        this.uiRootRectTransform = this.uiRoot.GetComponent<RectTransform>();

        this.shakeTimer = 0.0f;

        this.AdjustCamera();
    }

    private void Update()
    {
        this.AdjustCamera();
        this.AdjustShake();
    }

    public void Shake(float time = 0.16f)
    {
        this.shakeTimer = time;
    }

    private void AdjustCamera()
    {
        var ScreenValidHeight = Screen.height - this.screenTopMargin;
        var wRatio = Screen.width / ViewWidth;
        var hRatio = ScreenValidHeight / ViewHeight;
        var minRatio = Mathf.Min(wRatio, hRatio);
        var cameraWidth = wRatio / minRatio * ViewWidth;
        var cameraHeight = cameraWidth * ((float)Screen.height / Screen.width);

        this.cameraComponent.orthographicSize = cameraHeight * 0.5f;

        var pos = this.transform.position;
        pos.x = 0.0f;
        pos.y = this.screenTopMargin * 0.5f / Screen.height * cameraHeight;
        this.transform.position = pos;

        // fit background
        var backgroundPos = this.background.transform.position;
        backgroundPos.y = pos.y;
        this.background.transform.position = backgroundPos;
        this.background.transform.localScale = Vector3.one * (cameraHeight * 0.25f * 1.05f); // 1.05 ‚Í“K“–‚É‘å‚«‚­‚µ‚Ä‚¢‚é‚¾‚¯

        // fit UI
        this.AdjustUI();
    }

    private void AdjustUI()
    {
        var referenceWidth = this.canvasScaler.referenceResolution.x;
        var actualWidth = Screen.width;
        var transformTopMargin = (float)this.screenTopMargin * referenceWidth / actualWidth;

        this.uiRootRectTransform.offsetMax =
            new Vector2(this.uiRootRectTransform.offsetMax.x, -transformTopMargin);
    }

    private void AdjustShake()
    {
        if (this.shakeTimer > 0.0f)
        {
            this.shakeTimer -= Time.deltaTime;

            var dx = Mathf.Sin(this.shakeTimer * 960.0f) * this.shakeTimer * 0.02f;
            var dy = Mathf.Cos(this.shakeTimer * 960.0f) * this.shakeTimer * 0.02f;
            var pos = this.transform.position;
            pos.x += dx;
            pos.y += dy;
            this.transform.position = pos;
        }
    }

}
