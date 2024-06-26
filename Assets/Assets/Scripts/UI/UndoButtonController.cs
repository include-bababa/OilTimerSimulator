using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UndoButtonController : MonoBehaviour
{
    private EditManager editManager;
    private Button button;
    private Image backgroundImage;
    private Image labelImage;

    [SerializeField]
    private Image foregroundImage;

    [SerializeField]
    private Color enableForegroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    [SerializeField]
    private Color disableForegroundColor = new Color(1.0f, 1.0f, 1.0f, 0.375f);

    private void Awake()
    {
        this.editManager = FindObjectOfType<EditManager>(true);

        this.button = this.GetComponent<Button>();
        this.backgroundImage = this.transform.Find("Background").GetComponent<Image>();
        this.labelImage = this.transform.Find("Label").GetComponent<Image>();
    }

    private void Start()
    {
        this.UpdateColors();
    }

    private void LateUpdate()
    {
        if (this.editManager.IsEditMode)
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

            this.UpdateColors();
        }
        else
        {
            this.backgroundImage.gameObject.SetActive(false);
            this.labelImage.gameObject.SetActive(false);
        }
    }

    public void RunUndo()
    {
        this.editManager.UndoCommand();
    }

    private void UpdateColors()
    {
        var flag = this.editManager.HasCommand;

        this.foregroundImage.color = flag ?
            this.enableForegroundColor : this.disableForegroundColor;
    }
}
