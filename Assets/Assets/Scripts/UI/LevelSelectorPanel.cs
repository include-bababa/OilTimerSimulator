using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LevelSelectorPanel : MonoBehaviour
{
    private EditManager editManager;
    private PanelController panelController;

    private List<BuiltinLevelSelectorElement> builtinElements;
    private List<UserLevelSelectorElement> userElements;
    private MonoBehaviour selected;

    private State currentState;
    private UserLevelSelectorElement deleting;

    [SerializeField]
    private MessagePanel messagePanel;

    [SerializeField]
    private GameObject builtinContentView;

    [SerializeField]
    private GameObject builtinElementPrefab;

    [SerializeField]
    private GameObject userContentView;

    [SerializeField]
    private GameObject userElementPrefab;

    [SerializeField]
    private Button createNewButton;

    [SerializeField]
    private ScrollRect userScrollView;

    public bool IsOpen => this.panelController.IsOpen;

    public LevelData SelectedLevel =>
        (this.selected as BuiltinLevelSelectorElement)?.LevelData ??
        (this.selected as UserLevelSelectorElement)?.LevelData;

    public void Initialize()
    {
        this.editManager = FindObjectOfType<EditManager>();
        this.panelController = this.GetComponent<PanelController>();

        this.InitializeBuiltinView();
    }

    private void Update()
    {
        if (!this.panelController.IsOpen)
        {
            this.currentState = State.None;
            return;
        }

        if (this.currentState == State.ConfirmDelete)
        {
            if (!this.messagePanel.IsOpen)
            {
                if (this.messagePanel.Result == MessagePanel.MessageResult.OK)
                {
                    this.editManager.DeleteLevel(this.deleting.LevelData);
                    this.deleting = null;

                    // 再構築
                    this.InitializeUserView();
                }

                this.currentState = State.None;
            }
        }
    }

    public void OpenSelector()
    {
        // 新しいレベルが追加されているかもなので、毎回構築する
        this.InitializeUserView();

        if (this.editManager.CurrentLevel?.IsBuiltIn == true)
        {
            // built-in
            var selected = this.FindBuiltinElement(this.editManager.CurrentLevel);
            this.selected = selected;
            Logger.Log($"selected(built-in): {selected.LevelData.FileName}");
        }
        else
        {
            // user
            var selected = this.FindUserElement(this.editManager.CurrentLevel);
            this.selected = selected;
            Logger.Log($"selected(user): {selected.LevelData.FileName}");
        }

        foreach (var element in this.builtinElements)
        {
            element.SetSelected(element == this.selected);
        }

        foreach (var element in this.userElements)
        {
            element.SetSelected(element == this.selected);
        }

        
        //if (this.editManager.IsEditMode)
        {
            this.createNewButton.gameObject.SetActive(true);
            this.userScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, -176.0f);
        }
        /*
        else
        {
            this.createNewButton.gameObject.SetActive(false);
            this.userScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(0.0f, 0.0f);
        }
        */

        this.currentState = State.None;

        this.panelController.OpenPanel();
    }

    public void Deselect()
    {
        this.selected = null;
        this.panelController.ClosePanel();
    }

    public void Select(MonoBehaviour element)
    {
        this.selected = element;
        this.panelController.ClosePanel();
    }

    public void Delete(UserLevelSelectorElement element)
    {
        var name = element.LevelData.DisplayName;
        var dict = new Dictionary<string, string> { { "level_name", name } };
        var arguments = new object[] { dict };

        this.deleting = element;
        this.messagePanel.Title = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "DialogTitleDeleteLevel");
        this.messagePanel.Message = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "DialogMessageDeleteLevel", arguments);
        this.messagePanel.IsCancelEnabled = true;
        this.messagePanel.OpenPanel();

        this.currentState = State.ConfirmDelete;
    }

    private void InitializeBuiltinView()
    {
        var margin = 8.0f;
        var diff = this.builtinElementPrefab.GetComponent<RectTransform>().rect.height + margin;

        this.builtinElements = new List<BuiltinLevelSelectorElement>();
        var y = 0.0f;
        foreach (var data in LevelManager.Instance.BuiltinLevels)
        {
            var instance = Instantiate(this.builtinElementPrefab, this.builtinContentView.transform);
            instance.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -y);

            var element = instance.GetComponent<BuiltinLevelSelectorElement>();
            element.InitializeFromPanel();
            element.SetLevel(data);

            this.builtinElements.Add(element);
            y += diff;
        }

        var rectTransform = this.builtinContentView.GetComponent<RectTransform>();
        var sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = y - margin;
        rectTransform.sizeDelta = sizeDelta;
    }

    private void InitializeUserView()
    {
        if (this.userElements != null)
        {
            foreach (var element in this.userElements)
            {
                Destroy(element.gameObject);
            }
        }

        var margin = 8.0f;
        var diff = this.userElementPrefab.GetComponent<RectTransform>().rect.height + margin;

        this.userElements = new List<UserLevelSelectorElement>();
        var y = 0.0f;
        foreach (var data in LevelManager.Instance.UserLevels)
        {
            var instance = Instantiate(this.userElementPrefab, this.userContentView.transform);
            instance.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -y);

            var element = instance.GetComponent<UserLevelSelectorElement>();
            element.InitializeFromPanel();
            element.SetLevel(data, true);

            this.userElements.Add(element);
            y += diff;
        }

        var rectTransform = this.userContentView.GetComponent<RectTransform>();
        var sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = y - margin;
        rectTransform.sizeDelta = sizeDelta;
    }

    private BuiltinLevelSelectorElement FindBuiltinElement(LevelData data)
    {
        foreach (var element in this.builtinElements)
        {
            if (element.LevelData == data)
            {
                return element;
            }
        }

        return this.builtinElements.Count > 0 ? this.builtinElements[0] : null;
    }

    private UserLevelSelectorElement FindUserElement(LevelData data)
    {
        foreach (var element in this.userElements)
        {
            if (element.LevelData == data)
            {
                return element;
            }
        }

        return this.userElements.Count > 0 ? this.userElements[0] : null;
    }

    private enum State
    {
        None,
        ConfirmDelete,
    }
}
