using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : BaseWeapon
{
    public override void Use()
    {
        base.Use();
        List<HealthComp> entities = World.instance.healthEntities.FindAll(e => Vector3.SqrMagnitude(e.transform.position - PlayerController.instance.transform.position) < data.maxDistance * data.maxDistance);
        foreach (var entity in entities)
        {
            if (entity.group != targetHealthGroup) continue;

            entity.TakeDamage(CalculateDamage());
        }
    }
}