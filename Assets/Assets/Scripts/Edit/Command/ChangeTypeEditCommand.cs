using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTypeEditCommand : IEditCommand
{
    private Guid guid;

    private Vector3 position;
    private float angle;
    private PropType propType;
    private ScaleGrade scale;
    private RotationDirection rotation;
    private Color color;

    public ChangeTypeEditCommand(PropController before)
    {
        this.guid = before.Guid;

        this.position = before.gameObject.transform.position;
        this.angle = before.gameObject.transform.eulerAngles.z;
        this.propType = before.PropType;
        this.scale = before.ScaleSettter?.Scale ?? ScaleGrade.Medium;
        this.rotation = before.RotationSetter?.Rotation ?? RotationDirection.ClockwiseSlow;
        this.color = before.ColorSetter?.Color ?? Color.white;
    }

    public void Undo(EditManager editManager)
    {
        var target = editManager.FindProp(this.guid);
        editManager.Delete(target);

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
