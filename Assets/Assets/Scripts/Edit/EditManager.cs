using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class EditCurrentProp
{
    public PropType PropType { get; set; }

    public ScaleGrade Scale { get; set; }

    public RotationDirection Rotation { get; set; }

    public Color Color { get; set; }
}

public class EditManager : InputObserverBase
{
    private const float MoveUnit = 0.025f;
    private const float RotateUnit = 5.0f;
    private const float PropMaxX = 0.8f;
    private const float PropMaxY = 1.05f;
    private const int UndoCommandCapacity = 100;

    private ParticleManager particleManager;
    private LightingManager lightingManager;
    private OilPoolManager oilPoolManager;
    private SpriteRenderer backgroundRenderer;
    private CameraController cameraController;

    private PropIndicatorController propIndicator;

    private LevelData currentLevel;
    private ThemeInfo currentTheme;

    private bool isUIVisible;
    private bool isEditMode;
    private float elapsedTime;
    private int setTime;
    private bool isQuitting;

    private List<PropController> props;
    private EditSelectable selected;

    private List<IEditCommand> commands;

    [SerializeField]
    private EditSelectable background;

    [SerializeField]
    private GameObject platformRoot;

    [SerializeField]
    private GameObject gizmo;

    [SerializeField]
    private GameObject[] rotateHandles;

    [SerializeField]
    private MessagePanel messagePanel;

    [SerializeField]
    private ConfirmSavePanel confirmSavePanel;

    public OilPoolManager OilPoolManager => this.oilPoolManager;

    public bool IsEditMode
    {
        get => this.isEditMode;
        set => this.SetIsEditMode(value);
    }

    public bool IsUIVisible => this.isUIVisible || this.isEditMode;

    public bool IsLabelVisible => this.IsUIVisible && SaveManager.Instance.IsLabelVisible;

    public float ElapsedTime => this.elapsedTime;

    public int SetTime
    {
        get => this.isEditMode ? -1 : this.setTime;
        set => this.setTime = value;
    }

    public bool IsDirty { get; set; }

    public bool IsDropPaused
    {
        get => Physics2D.simulationMode == SimulationMode2D.Script || AdManager.Instance.IsInterstitialShowing;
        set => this.SetIsDropPaused(value);
    }

    public LevelData CurrentLevel => this.currentLevel;

    public ThemeInfo CurrentTheme => this.currentTheme;

    public EditSelectable Selected => this.selected;

    public EditCurrentProp CurrentProp { get; } = new EditCurrentProp();

    public bool HasCommand => this.commands.Count > 0;

    private void Awake()
    {
        this.particleManager = FindObjectOfType<ParticleManager>(true);
        this.lightingManager = FindObjectOfType<LightingManager>(true);
        this.oilPoolManager = FindObjectOfType<OilPoolManager>(true);
        this.cameraController = FindObjectOfType<CameraController>(true);
        this.propIndicator = FindObjectOfType<PropIndicatorController>(true);
        this.props = FindObjectsOfType<PropController>().ToList();
        this.commands = new List<IEditCommand>(UndoCommandCapacity);

        this.backgroundRenderer = this.background.GetComponentInParent<SpriteRenderer>();
        this.background.BeginDrag += this.Background_OnBeginDrag;
        this.background.Drag += this.Background_OnDrag;
        this.background.EndDrag += this.Background_OnEndDrag;
        this.background.PointerDown += this.Background_OnPointerDown;
        /*
        this.currentLevel = new LevelData()
        {
            FileName = Guid.NewGuid().ToString() + ".json",
            DisplayName = "Hourglass",
            LastUpdate = DateTime.Now,
            ThemeName = "beach",
        };

        this.SetTheme(LevelManager.Instance.GetTheme(this.currentLevel.ThemeName));
        */
        this.CurrentProp.PropType = PropType.Platform;
        this.CurrentProp.Scale = ScaleGrade.Medium;
        this.CurrentProp.Color = Color.white;
    }

    private void Start()
    {
        this.LoadLevel(LevelManager.Instance.BuiltinLevels.First());

        this.elapsedTime = 0.0f;
        this.setTime = 0;
        this.isUIVisible = false;
        this.isQuitting = false;
    }

    private void Update()
    {
        if (!this.IsDropPaused)
        {
            this.elapsedTime += Time.deltaTime / Time.timeScale;
        }

        if (this.isQuitting)
        {
            if (!this.confirmSavePanel.IsOpen && !this.messagePanel.IsOpen)
            {
                if (this.confirmSavePanel.Result == ConfirmSavePanel.PanelResult.Save)
                {
                    // save
                    this.SaveLevel();
                    this.confirmSavePanel.ClearResult();
                    //this.OpenConfirmQuitPanel();
                    ApplicationHelper.MoveTaskToBack();
                }
                else if (this.confirmSavePanel.Result == ConfirmSavePanel.PanelResult.Discard)
                {
                    // discard
                    this.IsDirty = false;
                    this.confirmSavePanel.ClearResult();
                    //this.OpenConfirmQuitPanel();
                    ApplicationHelper.MoveTaskToBack();
                }
                else if (this.messagePanel.Result == MessagePanel.MessageResult.OK)
                {
                    Application.Quit();
                }
                else
                {
                    this.isQuitting = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
        // show/hide gizmo
        var showGizmo = this.selected != null;
        if (this.gizmo.activeSelf != showGizmo)
        {
            this.gizmo.SetActive(showGizmo);
        }

        // show/hide rotation handles
        foreach (var handle in this.rotateHandles)
        {
            handle.SetActive(this.selected?.CanRotate == true);
        }

        if (showGizmo)
        {
            // move gizmo over selected object
            var pos = Camera.main.WorldToScreenPoint(this.selected.transform.position);
            //pos.z = this.gizmo.transform.position.z;
            //this.gizmo.transform.position = pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                this.gizmo.transform.parent.GetComponent<RectTransform>(),
                pos,
                Camera.main,
                out var localPos);
            this.gizmo.transform.localPosition = localPos;
        }
    }

    public void SetTheme(ThemeInfo theme)
    {
        Logger.Log($"Change theme: {this.currentTheme?.AssetName ?? "null"} -> {theme.AssetName}");

        if (this.currentTheme == theme)
        {
            return;
        }

        if (this.currentTheme != null)
        {
            // unload background texture
            this.currentTheme.BackgroundTexture.ReleaseAsset();
        }

        this.currentTheme = theme;
        var handle = this.currentTheme.BackgroundTexture.LoadAssetAsync<Sprite>();
        this.backgroundRenderer.sprite = handle.WaitForCompletion();

        this.currentLevel.ThemeName = this.currentTheme.AssetName;

        this.lightingManager.SetLightingEnable(this.currentTheme.UseLighting);
    }

    public void Select(EditSelectable target)
    {
        this.selected = target;
    }

    public PropController Create(
        PropType propType, Vector3 pos, ScaleGrade scale, RotationDirection rot, Color color)
    {
        var propData = LevelManager.Instance.GetPropData(propType);
        var obj = Instantiate(
            propData.Prefab, pos, Quaternion.identity, this.platformRoot.transform);

        var propController = obj.GetComponent<PropController>();
        this.props.Add(propController);

        propController.ScaleSettter?.SetScale(scale);
        propController.RotationSetter?.SetRotation(rot);
        propController.ColorSetter?.SetColor(color);

        return propController;
    }

    public void Delete(PropController target)
    {
        this.Select(null);

        this.props.Remove(target);

        Destroy(target.gameObject);
    }

    public PropController FindProp(Guid guid)
    {
        foreach (var prop in this.props)
        {
            if (prop.Guid == guid)
            {
                return prop;
            }
        }

        return null;
    }

    public void SetUIVisibilityForViewMode(bool flag)
    {
        this.isUIVisible = flag;
    }

    public float RoundPositionX(float pos)
    {
        if (pos > PropMaxX)
        {
            return PropMaxX;
        }
        else if (pos < -PropMaxX)
        {
            return -PropMaxX;
        }

        return pos > 0.0f ?
            (int)((pos + MoveUnit * 0.5f) / MoveUnit) * MoveUnit :
            (int)((pos - MoveUnit * 0.5f) / MoveUnit) * MoveUnit;
    }

    public float RoundPositionY(float pos)
    {
        if (pos > PropMaxY)
        {
            return PropMaxY;
        }
        else if (pos < -PropMaxY)
        {
            return -PropMaxY;
        }

        return pos > 0.0f ?
            (int)((pos + MoveUnit * 0.5f) / MoveUnit) * MoveUnit :
            (int)((pos - MoveUnit * 0.5f) / MoveUnit) * MoveUnit;
    }

    public float RoundRotation(float rot)
    {
        return rot > 0.0f ?
            (int)((rot + RotateUnit * 0.5f) / RotateUnit) * RotateUnit :
            (int)((rot - RotateUnit * 0.5f) / RotateUnit) * RotateUnit;
    }

    public void ClearLevel()
    {
        this.Select(null);

        foreach (var prop in this.props)
        {
            Destroy(prop.gameObject);
        }
        this.props.Clear();

        this.oilPoolManager.MainSpawner.ClearDrops();
        this.oilPoolManager.SubSpawner.ClearDrops();

        this.commands.Clear();
        this.elapsedTime = 0.0f;
    }

    public void ResetLevel()
    {
        this.oilPoolManager.MainSpawner.ClearDrops();
        this.oilPoolManager.SubSpawner.ClearDrops();

        foreach (var prop in this.props)
        {
            prop.Reset();
        }

        this.commands.Clear();

        this.elapsedTime = 0.0f;
    }

    public void CreateNewLevel(string name)
    {
        var data = new LevelData()
        {
            FileName = Guid.NewGuid().ToString() + ".json",
            DisplayName = name,
            LastUpdate = DateTime.Now,
            ThemeName = "beach",
            MainColor = Color.yellow,
            SubColor = Color.cyan,
            MainInterval = 4.0f,
            SubInterval = 4.0f,
            Instances = new LevelData.InstanceInfo[0] { },
        };

        this.LoadLevel(data);
    }

    public void LoadLevel(LevelData data)
    {
        this.ClearLevel();

        this.currentLevel = data;
        this.SetTheme(LevelManager.Instance.GetTheme(data.ThemeName));

        this.oilPoolManager.SetMainColor(data.MainColor);
        if (data.SubColor.a <= 0.0f)
        {
            this.oilPoolManager.UnsetSubColor();
        }
        else
        {
            this.oilPoolManager.SetSubColor(data.SubColor);
        }

        this.oilPoolManager.MainSpawner.DropInterval = data.MainInterval;
        this.oilPoolManager.SubSpawner.DropInterval = data.SubInterval;

        foreach (var instance in data.Instances)
        {
            var prop = this.Create(
                instance.PropType,
                new Vector3(instance.Position.x, instance.Position.y, 0.0f),
                instance.ScaleGrade,
                instance.Rotation,
                instance.Color);
            prop.transform.rotation = Quaternion.Euler(0, 0, instance.Angle);
        }

        this.IsDirty = false;
    }

    public void SaveLevel()
    {
        var instances = this.props.Select(prop => new LevelData.InstanceInfo()
        {
            PropType = prop.PropType,
            ScaleGrade = prop.ScaleSettter?.Scale ?? ScaleGrade.Medium,
            Rotation = prop.RotationSetter?.Rotation ?? RotationDirection.ClockwiseSlow,
            Color = prop.ColorSetter?.Color ?? Color.white,
            Position = prop.transform.position,
            Angle = prop.EditSelectable.CanRotate ? prop.transform.rotation.eulerAngles.z : 0,
        }).ToArray();

        this.currentLevel.MainColor = this.oilPoolManager.MainColor;
        this.currentLevel.SubColor = this.oilPoolManager.SubColor ?? new Color(0, 0, 0, 0);
        this.currentLevel.MainInterval = this.oilPoolManager.MainSpawner.DropInterval;
        this.currentLevel.SubInterval = this.oilPoolManager.SubSpawner.DropInterval;

        this.currentLevel.LastUpdate = DateTime.Now;
        this.currentLevel.Instances = instances;

        if (this.currentLevel.IsBuiltIn)
        {
            this.currentLevel.FileName = Guid.NewGuid().ToString() + ".json";
            this.currentLevel.IsBuiltIn = false;
        }

        LevelUtility.Save(this.currentLevel);

        if (!LevelManager.Instance.UserLevels.Contains(this.currentLevel))
        {
            LevelManager.Instance.AddUserLevel(this.currentLevel);
        }

        this.IsDirty = false;
    }

    public void DeleteLevel(LevelData data)
    {
        LevelManager.Instance.DeleteUserLevel(data);
        LevelUtility.Delete(data);

        if (this.currentLevel == data)
        {
            this.LoadLevel(LevelManager.Instance.BuiltinLevels.First());
        }
    }

    public void AddCommand(IEditCommand command)
    {
        if (this.commands.Count >= UndoCommandCapacity)
        {
            this.commands.RemoveAt(0);
        }

        this.commands.Add(command);
    }

    public void UndoCommand()
    {
        if (this.commands.Count <= 0)
        {
            return;
        }

        var command = this.commands[this.commands.Count - 1];
        this.commands.RemoveAt(this.commands.Count - 1);

        command.Undo(this);

        this.IsDirty = true;
    }

    public override bool ProcessInput(InputType inputType)
    {
        if (this.isEditMode && this.commands.Count > 0)
        {
            this.UndoCommand();
            return true;
        }

        if (this.IsDirty)
        {
            // check save
            this.confirmSavePanel.OpenPanel(this.currentLevel.DisplayName);
            this.isQuitting = true;
            return true;
        }

        // confirm
        this.confirmSavePanel.ClearResult();
        this.messagePanel.ClearResult();
        //this.OpenConfirmQuitPanel();
        ApplicationHelper.MoveTaskToBack();
        this.isQuitting = true;
        return true;
    }

    private void OpenConfirmQuitPanel()
    {
        this.messagePanel.Title = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "DialogTitleConfirmQuit");
        this.messagePanel.Message = LocalizationSettings.StringDatabase.GetLocalizedString("Strings", "DialogMessageConfirmQuit");
        this.messagePanel.IsCancelEnabled = true;
        this.messagePanel.OpenPanel();
    }

    private void SetIsEditMode(bool flag)
    {
        this.isEditMode = flag;
        if (!this.isEditMode)
        {
            this.Select(null);

            this.ResetLevel();

            this.IsDropPaused = false;
        }
    }

    private void SetIsDropPaused(bool flag)
    {
        Physics2D.simulationMode =
            flag ? SimulationMode2D.Script : SimulationMode2D.FixedUpdate;
    }

    private void Background_OnBeginDrag(object sender, EditSelectablePointerEventData eventData)
    {
        if (!this.isEditMode)
        {
            return;
        }

        if (eventData.SelectedPrevious == null)
        {
            return;
        }

        eventData.SelectedPrevious.OnBeginDrag(eventData.PointerEventData);
    }

    private void Background_OnDrag(object sender, EditSelectablePointerEventData eventData)
    {
        if (!this.isEditMode)
        {
            return;
        }

        if (eventData.SelectedPrevious == null)
        {
            return;
        }

        eventData.SelectedPrevious.OnDrag(eventData.PointerEventData);
    }

    private void Background_OnEndDrag(object sender, EditSelectablePointerEventData eventData)
    {
        if (!this.isEditMode)
        {
            return;
        }

        if (eventData.SelectedPrevious == null)
        {
            return;
        }

        eventData.SelectedPrevious.OnEndDrag(eventData.PointerEventData);
    }

    private void Background_OnPointerDown(object sender, EditSelectablePointerEventData eventData)
    {
        if (!this.isEditMode)
        {
            return;
        }

        if (eventData.SelectedPrevious != null)
        {
            this.Select(null);
            return;
        }

        const float xMax = 0.8f;
        const float yMax = 1.1f;

        var pos = Camera.main.ScreenToWorldPoint(eventData.PointerEventData.position);
        if (pos.x < -xMax || pos.x > xMax || pos.y < -yMax || pos.y > yMax)
        {
            // 配置可能範囲外
            return;
        }


        pos.x = this.RoundPositionX(pos.x);
        pos.y = this.RoundPositionY(pos.y);
        pos.z = 0.0f;

        var created = this.Create(
            this.CurrentProp.PropType,
            pos,
            this.CurrentProp.Scale,
            this.CurrentProp.Rotation,
            this.CurrentProp.Color);
        //this.Select(created.GetComponentInChildren<EditSelectable>());
        created.GetComponentInChildren<EditSelectable>()?.OnDrag(eventData.PointerEventData);

        this.particleManager.EmitCreateParticle(pos);
        this.propIndicator.InflateMoment();
        //this.cameraController.Shake(); // 見づらいのでコメントアウト

        this.AddCommand(new CreationEditCommand(created.Guid));

        this.IsDirty = true;
    }
}
