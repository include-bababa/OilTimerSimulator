using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    [NonSerialized]
    private bool isBuiltin;

    [NonSerialized]
    private string filename;

    [SerializeField]
    private string displayName;

    [SerializeField]
    private long lastUpdate;

    [SerializeField]
    private string themeName;

    [SerializeField]
    private Color mainColor;

    [SerializeField]
    private Color subColor;

    [SerializeField]
    private float mainInterval;

    [SerializeField]
    private float subInterval;

    [SerializeField]
    private InstanceInfo[] instances;

    public bool IsBuiltIn
    {
        get => this.isBuiltin;
        set => this.isBuiltin = value;
    }

    public string FileName
    {
        get => this.filename;
        set => this.filename = value;
    }

    public string DisplayName
    {
        get => this.displayName;
        set => this.displayName = value;
    }

    public DateTime LastUpdate
    {
        get => new DateTime(this.lastUpdate);
        set => this.lastUpdate = value.Ticks;
    }

    public string ThemeName
    {
        get => this.themeName;
        set => this.themeName = value;
    }

    public Color MainColor
    {
        get => this.mainColor;
        set => this.mainColor = value;
    }

    public Color SubColor
    {
        get => this.subColor;
        set => this.subColor = value;
    }

    public float MainInterval
    {
        get => this.mainInterval;
        set => this.mainInterval = value;
    }

    public float SubInterval
    {
        get => this.subInterval;
        set => this.subInterval = value;
    }

    public InstanceInfo[] Instances
    {
        get => this.instances;
        set => this.instances = value;
    }

    [Serializable]
    public class InstanceInfo
    {
        [SerializeField]
        private PropType propType;

        [SerializeField]
        private ScaleGrade scale;

        [SerializeField]
        private RotationDirection rotation;

        [SerializeField]
        private Color color;

        [SerializeField]
        private Vector2 pos;

        [SerializeField]
        private float angle;

        public PropType PropType
        {
            get => this.propType;
            set => this.propType = value;
        }

        public ScaleGrade ScaleGrade
        {
            get => this.scale;
            set => this.scale = value;
        }

        public RotationDirection Rotation
        {
            get => this.rotation;
            set => this.rotation = value;
        }

        public Color Color
        {
            get => this.color;
            set => this.color = value;
        }

        public Vector2 Position
        {
            get => this.pos;
            set => this.pos = value;
        }

        public float Angle
        {
            get => this.angle;
            set => this.angle = value;
        }
    }
}
