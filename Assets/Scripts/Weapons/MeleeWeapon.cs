using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : BaseWeapon
{
    Vector3 dir1 { get { return PlayerController.instance.cursorDirection2D.Rotate(data.angle / 2.0f).ProjectTo3D(); } }
    Vector3 dir2 { get { return PlayerController.instance.cursorDirection2D.Rotate(-data.angle / 2.0f).ProjectTo3D(); } }

    public override bool IsPointReachable(Vector3 point)
    {
        Vector3 dir = point - PlayerController.instance.transform.position;
        float dist = Vector3.SqrMagnitude(dir);

        bool isWithinDistance = dist < data.maxDistance * data.maxDistance && dist > data.minDistance * data.minDistance;
        bool isWithinAngle = Mathf.Sign(Vector3.Cross(dir, dir1).y) == -1 && Mathf.Sign(Vector3.Cross(dir, dir2).y) == 1;

        return isWithinDistance && isWithinAngle;
    }

    public override void Use()
    {
        base.Use();
        List<HealthComp> entities = World.instance.healthEntities.FindAll(e =>
        {
            return e.group == targetHealthGroup && IsPointReachable(e.transform.position);
        });

        foreach (var entity in entities)
        {
            entity.TakeDamage(CalculateDamage().finalDamage, data.bleedingSpeedFactor, data.bleedingPowerFactor);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (!Application.isPlaying) return;

        Vector3 originPos = PlayerController.instance.transform.position;
        Vector3 pos1 = originPos + data.maxDistance * dir1;
        Vector3 pos2 = originPos + data.maxDistance * dir2;

        Gizmos.DrawLine(originPos, pos1);
        Gizmos.DrawLine(originPos, pos2);
    }
}