using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButtonController : MonoBehaviour
{
    private EditManager editManager;
    private Button button;
    private Image backgroundImage;
    private Image labelImage;

    private void Awake()
    {
        this.editManager = FindObjectOfType<EditManager>(true);

        this.button = this.GetComponent<Button>();

        this.backgroundImage = this.transform.Find("Background").GetComponent<Image>();
        this.labelImage = this.transform.Find("Label").GetComponent<Image>();
    }

    private void LateUpdate()
    {
        if (this.editManager.IsUIVisible)
        {
            var activeSelf = this.backgroundImage.gameObject.activeSelf;
            if (!activeSelf)
            {
                this.backgroundImage.gameObject.SetActive(true);

                // Ç»Ç∫Ç©É{É^ÉìÇÃêFÇ™Ç®Ç©ÇµÇ≠Ç»ÇÈÇÃÇ≈Ç®Ç‹Ç∂Ç»Ç¢
                this.button.interactable = false;
                this.button.interactable = true;
            }

            this.labelImage.gameObject.SetActive(this.editManager.IsLabelVisible);
        }
        else
        {
            this.backgroundImage.gameObject.SetActive(false);
            this.labelImage.gameObject.SetActive(false);
        }
    }

    public void RunReset()
    {
        this.editManager.ResetLevel();
    }
}
