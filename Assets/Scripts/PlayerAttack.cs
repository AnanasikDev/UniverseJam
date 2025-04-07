using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BaseWeapon currentWeapon;
    [ShowInInspector][ReadOnly] public bool isAttacking { get; private set; } = false;

    [SerializeField] private KeyCode key = KeyCode.Mouse0;

    public void UpdateAttack()
    {
        UpdateInput();
        if (isAttacking)
        {
            currentWeapon.Use();
        }
    }

    private void UpdateInput()
    {
        isAttacking = currentWeapon.IsReadyToUse() && Input.GetKeyDown(key);
    }
}