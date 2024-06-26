using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletionEditCommand : IEditCommand
{
    private Guid guid;

    private Vector3 position;
    private float angle;
    private PropType propType;
    private ScaleGrade scale;
    private RotationDirection rotation;
    private Color color;

    public DeletionEditCommand(PropController prop)
    {
        this.guid = prop.Guid;

        this.position = prop.gameObject.transform.position;
        this.angle = prop.gameObject.transform.eulerAngles.z;
        this.propType = prop.PropType;
        this.scale = prop.ScaleSettter?.Scale ?? ScaleGrade.Medium;
        this.rotation = prop.RotationSetter?.Rotation ?? RotationDirection.ClockwiseSlow;
        this.color = prop.ColorSetter?.Color ?? Color.white;
    }

    public void Undo(EditManager editManager)
    {
        var prop = editManager.Create(
            this.propType,
            this.position,
            this.scale,
            this.rotation,
            this.color);
        prop.Guid = this.guid;
        prop.gameObject.transform.rotation = Quaternion.Euler(0, 0, this.angle);
    }
}
