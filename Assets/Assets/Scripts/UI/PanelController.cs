using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    private const float OpenTime = 0.06f;

    private UIManager manager;
    private Image image;
    private float defaultAlpha;

    private bool isOpen;
    private float transitTimer;

    public event EventHandler Opening;
    public event EventHandler Open;
    public event EventHandler Closing;
    public event EventHandler Closed;

    public bool IsOpen => this.isOpen;

    public bool IsTransitioning => this.transitTimer > 0;

    private void Awake()
    {
        this.image = this.GetComponent<Image>();
        this.defaultAlpha = this.image.color.a;
        this.manager = FindObjectOfType<UIManager>(true);
    }

    private void LateUpdate()
    {
        if (this.transitTimer > 0)
        {
            this.transitTimer -= Time.deltaTime;
            if (this.transitTimer <= 0)
            {
                this.transitTimer = 0.0f;
                this.gameObject.SetActive(this.isOpen);

                if (this.isOpen)
                {
                    this.Open?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    this.Closed?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        var ratio = 0.0f;
        if (this.isOpen)
        {
            ratio = (OpenTime - this.transitTimer) / OpenTime;
        }
        else
        {
            ratio = this.transitTimer / OpenTime;
        }

        var scale = this.transform.localScale;
        scale.x = 0.5f * ratio + 0.5f;
        scale.y = 0.5f * ratio + 0.5f;
        this.transform.localScale = scale;

        var color = this.image.color;
        color.a = ratio * this.defaultAlpha;
        this.image.color = color;
    }

    public virtual void OpenPanel()
    {
        if (this.transitTimer > 0)
        {
            return;
        }

        this.isOpen = true;
        this.transitTimer = OpenTime;

        this.gameObject.SetActive(true);
        this.manager.PushPanel(this);

        this.Opening?.Invoke(this, EventArgs.Empty);
    }

    public virtual void ClosePanel()
    {
        if (this.transitTimer > 0)
        {
            return;
        }

        this.isOpen = false;
        this.transitTimer = OpenTime;

        this.manager.PopPanel(this);

        this.Closing?.Invoke(this, EventArgs.Empty);
    }

    public void CloseAll()
    {
        this.manager.CloseAllPanel();
    }
}
