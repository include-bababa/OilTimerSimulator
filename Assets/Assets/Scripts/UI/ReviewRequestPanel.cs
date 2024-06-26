using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReviewRequestPanel : MonoBehaviour
{
    private PanelController panelController;

    [SerializeField]
    private Button notNowButton;

    [SerializeField]
    private Button reviewButton;

    private float openedTimer;

    public void Initialize()
    {
        this.panelController = this.GetComponent<PanelController>();
    }

    private void Update()
    {
        if (this.openedTimer > 0)
        {
            this.openedTimer -= Time.deltaTime;

            if (this.openedTimer <= 0)
            {
                this.openedTimer = 0;

                this.notNowButton.interactable = true;
                this.reviewButton.interactable = true;
            }
        }
    }

    public void OpenPanel()
    {
        this.panelController.OpenPanel();
        this.openedTimer = 1.0f;

        this.notNowButton.interactable = false;
        this.reviewButton.interactable = false;
    }

    public void Close()
    {
        this.panelController.ClosePanel();
    }

    public void Review()
    {
#if ENABLE_ANDROID_REVIEW
        StartCoroutine(AndroidReviewManager.Instance.RequestAndLaunchReviewFlow());
#endif
        this.panelController.ClosePanel();
    }
}
