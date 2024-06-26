using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

public class DropController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private CircleCollider2D collider2d;
    private DropTriggerCollision triggerController;

    private GameObject boundTo;

    private float currentDensity;
    private List<PropClosestPointInfo> propClosestPointInfos;

    [SerializeField]
    private GameObject lightingChild;

    public DropSpawner Spawner { get; set; }

    public GameObject BoundTo
    {
        get => this.boundTo;
        set => this.Bind(value);
    }

    public bool IsLightingEnabled
    {
        get => this.lightingChild.activeSelf;
        set => this.SetLightingEnable(value);
    }

    private void Awake()
    {
        this.rigidbody2d = this.GetComponent<Rigidbody2D>();
        this.collider2d = this.GetComponent<CircleCollider2D>();
        this.triggerController = this.GetComponentInChildren<DropTriggerCollision>();

        this.propClosestPointInfos = new List<PropClosestPointInfo>();
    }

    private void OnDestroy()
    {
        this.Spawner?.RemoveDrop(this);
    }

    private void Update()
    {
        var y = this.transform.position.y;
        if (y > 1.4f || y < -1.4f)
        {
            Destroy(this.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (UnityEditor.Selection.activeGameObject == this.gameObject)
        {
            GUI.Label(new Rect(25, 25, 200, 50), $"Density: {currentDensity}");
        }
    }
#endif

    private void FixedUpdate()
    {
        if (this.Spawner.EditManager.IsDropPaused)
        {
            return;
        }

        if (this.boundTo != null)
        {
            this.FixedUpdateBound();
        }
        else
        {
            this.FixedUpdateFree();
        }

        this.FixedUpdateRadius();
    }

    private void FixedUpdateBound()
    {
        var boundTo = this.BoundTo;

        var axis = new Vector2(boundTo.transform.right.x, boundTo.transform.right.y);
        var center = new Vector2(boundTo.transform.position.x, boundTo.transform.position.y);
        var length = boundTo.transform.lossyScale.x + 0.1f;
        var start = center - axis * length * 0.5f;

        var pos = new Vector2(this.transform.position.x, this.transform.position.y);
        var dot = Vector2.Dot(pos - start, axis);
        dot = Mathf.Clamp(dot, 0.0f, length);
        var target = start + axis * dot;

        var diff = target - pos;
        var diffLength = diff.magnitude;
        var diffMax = 1.0f * Time.fixedDeltaTime;
        if (diffLength > diffMax)
        {
            pos = pos + diff / diffLength * diffMax;
            this.transform.position = new Vector3(pos.x, pos.y, 0.0f);
        }
        else
        {
            this.transform.position = new Vector3(target.x, target.y, 0.0f);
        }

        this.rigidbody2d.velocity -= axis * Vector2.Dot(this.rigidbody2d.velocity, axis);

        this.currentDensity = 0.0f;
    }

    private void FixedUpdateFree()
    {
        /*
        const float ResistanceCoeff = 90.0f;
        const float PressureCoeff = 20.0f;
        const float TensionCoeff = 8.2f;
        const float ArtificalPressureCoeff = 2.0f;
        const float EdgeDistance = 0.55f;
        const float EdgeDistanceSq = EdgeDistance * EdgeDistance;
        */

        const float ResistanceCoeff = 80.0f;
        const float PressureCoeff = 13.0f;
        const float TensionCoeff = 14.0f;
        const float ArtificalPressureCoeff = 3.5f;
        const float EdgeDistance = 0.6f;
        const float EdgeDistanceSq = EdgeDistance * EdgeDistance;
        const float EdgeDistanceForPlatforms = 0.55f;
        const float EdgeDistanceSqForPlatforms = EdgeDistanceForPlatforms * EdgeDistanceForPlatforms;

        var density = EdgeDistanceSq * EdgeDistanceSq * EdgeDistanceSq;
        foreach (var drop in this.triggerController.NeighborDrops)
        {
            var diff = drop.transform.position - this.transform.position;
            var distSq = diff.sqrMagnitude;
            if (distSq < EdgeDistanceSq)
            {
                var edgeSqDist = EdgeDistanceSq - distSq;
                density += edgeSqDist * edgeSqDist * edgeSqDist;
            }
        }

        this.propClosestPointInfos.Clear();
        foreach (var prop in this.triggerController.NeighborProps)
        {
            var minDistSq = EdgeDistanceSqForPlatforms;
            var closestPoint = Vector2.zero;
            foreach (var collider in prop.Colliders)
            {
                var point = collider.ClosestPoint(this.transform.position);
                var diff = new Vector2(point.x - this.transform.position.x, point.y - this.transform.position.y);
                var distSq = diff.sqrMagnitude;
                if (distSq < minDistSq && distSq > Mathf.Epsilon)
                {
                    minDistSq = distSq;
                    closestPoint = point;
                }
            }

            if (minDistSq < EdgeDistanceSqForPlatforms)
            {
                this.propClosestPointInfos.Add(new PropClosestPointInfo(minDistSq, closestPoint));

                var edgeSqDist = EdgeDistanceSqForPlatforms - minDistSq;
                density += edgeSqDist * edgeSqDist * edgeSqDist * 1.5f;
            }
        }

        foreach (var drop in this.triggerController.NeighborDrops)
        {
            var diff = drop.transform.position - this.transform.position;
            var distSq = diff.sqrMagnitude;
            if (distSq < EdgeDistanceSq)
            {
                var edgeSqDist = EdgeDistanceSq - distSq;
                var localDensityRatio = edgeSqDist * edgeSqDist * edgeSqDist / (EdgeDistanceSq * EdgeDistanceSq * EdgeDistanceSq);

                var dist = Mathf.Sqrt(distSq);
                var dir = diff / dist;
                var edgeDist = EdgeDistance - dist;
                var pressure = PressureCoeff * edgeDist * edgeDist;
                var tension = TensionCoeff * (EdgeDistanceSq - 5 * distSq) * (EdgeDistanceSq - distSq) / density;
                var artifical = ArtificalPressureCoeff * localDensityRatio * localDensityRatio * localDensityRatio * localDensityRatio;
                this.rigidbody2d.AddForce(dir * (tension - pressure - artifical));
            }
        }

        foreach (var point in this.propClosestPointInfos)
        {
            var pos = point.Point;
            var diff = new Vector2(pos.x - this.transform.position.x, pos.y - this.transform.position.y);
            var distSq = point.DistSq;
            var edgeSqDist = EdgeDistanceSqForPlatforms - distSq;
            var localDensityRatio = edgeSqDist * edgeSqDist * edgeSqDist / (EdgeDistanceSqForPlatforms * EdgeDistanceSqForPlatforms * EdgeDistanceSqForPlatforms);
            var dist = Mathf.Sqrt(distSq);
            var dir = diff / dist;
            var edgeDist = EdgeDistanceForPlatforms - dist;
            var pressure = PressureCoeff * edgeDist * edgeDist;
            var tension = TensionCoeff * (EdgeDistanceSqForPlatforms - 5 * distSq) * (EdgeDistanceSqForPlatforms - distSq) / density * 0.5f;
            var artifical = ArtificalPressureCoeff * localDensityRatio * localDensityRatio * localDensityRatio * localDensityRatio;
            this.rigidbody2d.AddForce(new Vector2(dir.x * (tension - pressure - artifical) * 0.3f, Mathf.Max(dir.y * (tension - pressure - artifical) * 0.4f, 0.0f)));
        }

        this.currentDensity = density;

        /*
        const float PullCoeff = 0.0035f;
        const float PushCoeff = 0.00105f;

        const float EdgeDistance = 0.55f;

        var density = 0.0f;
        foreach (var neighbor in this.triggerController.NeighborDrops)
        {
            var diff = neighbor.transform.position - this.transform.position;
            var dist = diff.magnitude;
            density += Mathf.Max(EdgeDistance - dist, 0.0f) * 1.0f;
        }

        var closestPropPoints = new List<Vector2>();
        foreach (var neighbor in this.triggerController.NeighborProps)
        {
            var minDistSq = EdgeDistance;
            var closestPoint = Vector2.zero;
            foreach (var collider in neighbor.Colliders)
            {
                var point = collider.ClosestPoint(this.transform.position);
                var diff = new Vector2(point.x - this.transform.position.x, point.y - this.transform.position.y);
                var distSq = diff.sqrMagnitude;
                if (distSq < minDistSq)
                {
                    minDistSq = distSq;
                    closestPoint = point;
                }
            }

            if (minDistSq < EdgeDistance)
            {
                closestPropPoints.Add(closestPoint);
                density += Mathf.Max(EdgeDistance - Mathf.Sqrt(minDistSq), 0.0f) * 2.0f;
            }
        }

        foreach (var neighbor in this.triggerController.NeighborDrops)
        {
            var diff = neighbor.transform.position - this.transform.position;
            var dist = diff.magnitude;
            dist = Mathf.Max(dist, 0.015f);
            var dir = diff / dist;

            var pull = PullCoeff / (dist * dist);
            var push = PushCoeff * density / (dist * dist);
            this.rigidbody2d.AddForce(dir * (pull - push));
        }

        foreach (var point in closestPropPoints)
        {
            var diff = new Vector2(point.x - this.transform.position.x, point.y - this.transform.position.y);
            var dist = diff.magnitude;
            dist = Mathf.Max(dist, 0.015f);
            var dir = diff / dist;

            var push = PushCoeff * density / (dist * dist) * 0.2f;
            this.rigidbody2d.AddForce(dir * -push);
        }
        */
        // fluid resistance
        var velSq = this.rigidbody2d.velocity.sqrMagnitude;
        if (!Mathf.Approximately(velSq, 0.0f))
        {
            var vel = Mathf.Sqrt(velSq);
            var velDir = this.rigidbody2d.velocity / vel;
            this.rigidbody2d.AddForce(-velDir * velSq * ResistanceCoeff);
        }

        // finger pressure
        if (this.Spawner?.EditManager.IsEditMode == false)
        {
            const float FingerRadius = 0.42f;
            const float FingerRadiusSq = FingerRadius * FingerRadius;
            const float FingerForceCoeff = 3.6f;
            foreach (var touch in InputManager.Instance.Touches)
            {
                var diff = new Vector2(
                    touch.WorldPosition.x - this.transform.position.x,
                    touch.WorldPosition.y - this.transform.position.y);
                var distSq = diff.sqrMagnitude;
                if (distSq < FingerRadiusSq && distSq > Mathf.Epsilon)
                {
                    var dist = Mathf.Sqrt(distSq);
                    var forceDir = diff / dist;
                    var coeff = dist < FingerRadius * 0.33f ? FingerForceCoeff * 3.0f :
                        dist < FingerRadius * 0.66f ? FingerForceCoeff * 1.5f : FingerForceCoeff;
                    this.rigidbody2d.AddForce(-forceDir * coeff);
                }
            }
        }
    }

    private void FixedUpdateRadius()
    {
        this.collider2d.radius = Mathf.Max(this.currentDensity / 0.2f, 1.0f) * 0.015f;
    }

    private void Bind(GameObject bindTo)
    {
        if (this.boundTo == bindTo)
        {
            return;
        }

        this.boundTo = bindTo;
        this.collider2d.enabled = this.boundTo == null;
    }

    private void SetLightingEnable(bool enable)
    {
        this.lightingChild.SetActive(enable);
    }

    private struct PropClosestPointInfo
    {
        public PropClosestPointInfo(float distSq, Vector2 point)
        {
            this.DistSq = distSq;
            this.Point = point;
        }

        public float DistSq { get; set; }

        public Vector2 Point { get; set; }
    }
}
