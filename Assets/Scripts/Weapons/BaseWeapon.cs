using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponData data;
    protected float lastTimeUsed = -100;
    public HealthComp.HealthGroup targetHealthGroup = HealthComp.HealthGroup.Enemy;
    public event System.Action onUsedEvent;

    public virtual bool IsReadyToUse()
    {
        return Time.time - lastTimeUsed > data.reloadTimeSeconds;
    }

    public virtual void Use()
    {
        lastTimeUsed = Time.time;
        onUsedEvent?.Invoke();
        Debug.Log(data.name + " has been used towards " + targetHealthGroup);
    }

    public virtual float CalculateDamage()
    {
        float critFactor = (Random.Range(0.0f, 1.0f) < data.critChange) ? data.critFactor : 1;
        float randFactor = (Random.Range(1.0f - data.damageRandomization, 1.0f + data.damageRandomization));
        float damage = data.damagePerUse * critFactor * randFactor;

        return damage;
    }

    private void OnDrawGizmos()
    {
        Vector3 position = Application.isPlaying ? PlayerController.instance.transform.position : transform.position;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(position, data.minDistance);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(position, data.maxDistance);
    }
}