using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public WeaponData data;
    protected float lastTimeUsed = -100;
    public HealthComp.HealthGroup targetHealthGroup = HealthComp.HealthGroup.Enemy;
    public event System.Action onUsedEvent;
    public event System.Action onFinishedUsingEvent;

    public virtual bool IsReadyToUse()
    {
        return Time.time - lastTimeUsed > data.reloadTimeSeconds;
    }

    public abstract bool IsPointReachable(Vector3 point);

    public virtual void Use()
    {
        lastTimeUsed = Time.time;
        onUsedEvent?.Invoke();
        StartCoroutine(FinishUsing());
        Debug.Log(data.name + " has been used towards " + targetHealthGroup);
    }

    protected virtual IEnumerator FinishUsing()
    {
        yield return new WaitForSeconds(data.reloadTimeSeconds);
        onFinishedUsingEvent?.Invoke();
    }

    public virtual WeaponHitInfo CalculateDamage()
    {
        bool isCrit = Random.Range(0.0f, 1.0f) < data.critChange;
        float critFactor = isCrit ? data.critFactor : 1;
        float randFactor = (Random.Range(1.0f - data.damageRandomization, 1.0f + data.damageRandomization));
        float damage = data.damagePerUse * critFactor * randFactor;

        return new WeaponHitInfo(damage, isCrit);
    }

    protected virtual void OnDrawGizmos()
    {
        Vector3 position = Application.isPlaying ? PlayerController.instance.transform.position : transform.position;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(position, data.minDistance);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(position, data.maxDistance);
    }
}

public struct WeaponHitInfo
{
    public float finalDamage;
    public bool isCrit;

    public WeaponHitInfo(float _damage, bool _isCrit)
    {
        this.finalDamage = _damage;
        this.isCrit = _isCrit;
    }
}