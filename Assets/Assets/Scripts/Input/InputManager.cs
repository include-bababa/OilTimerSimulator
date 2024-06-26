using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public static InputManager Instance => instance;

    public struct TouchInput
    {
        public TouchInput(Vector2 screen, Vector3 world)
        {
            this.ScreenPosition = screen;
            this.WorldPosition = world;
        }

        public Vector2 ScreenPosition { get; }

        public Vector3 WorldPosition { get; }
    }

    private List<TouchInput> touches;

    [SerializeField]
    private InputObserverBase[] observers;

    public IEnumerable<TouchInput> Touches => this.touches;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        this.touches = new List<TouchInput>(16);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        this.touches.Clear();
        var touchCount = Input.touchCount;
        for (var index = 0; index < touchCount; index++)
        {
            var touch = Input.GetTouch(index);
            var world = Camera.main.ScreenToWorldPoint(touch.position);
            this.touches.Add(new TouchInput(touch.position, world));
        }
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.touches.Add(new TouchInput(Input.mousePosition, world));
        }
#endif

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.observers != null)
            {
                foreach (var observer in this.observers)
                {
                    var result = observer.ProcessInput(InputObserverBase.InputType.Back);
                    if (result)
                    {
                        break;
                    }
                }
            }
        }
    }
}
