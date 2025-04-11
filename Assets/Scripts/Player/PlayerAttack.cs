using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private BaseWeapon mainWeapon;
    [SerializeField] private BaseWeapon altWeapon;
    [ShowInInspector][ReadOnly] public bool isAttacking { get; private set; } = false;
    [ReadOnly] public bool isUsingMain;
    [ReadOnly] public bool isUsingAlt;

    [SerializeField] private KeyCode mainAttackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode altAttackKey = KeyCode.Mouse1;
    private bool bothReady;

    [SerializeField] private Transform aimObject;
    [SerializeField] private MeshRenderer aimRenderer;

    private void Start()
    {
        mainWeapon.onFinishedUsingEvent += () =>
        {
            isUsingMain = false;
        };

        altWeapon.onFinishedUsingEvent += () =>
        {
            isUsingAlt = false;
        };
    }

    public void UpdateAttack()
    {
        UpdatePreview();

        isAttacking = !bothReady;

        bothReady = mainWeapon.IsReadyToUse() && altWeapon.IsReadyToUse();

        if (Input.GetKeyDown(mainAttackKey) && bothReady)
        {
            mainWeapon?.Use();
            isUsingMain = true;
        }
        else if (Input.GetKeyDown(altAttackKey) && bothReady)
        {
            altWeapon?.Use();
            isUsingAlt = true;
        }
    }

    private void UpdatePreview()
    {
        float dir = GetAngleBetweenVectors(PlayerController.instance.cursorDirection2D.normalized, Vector2.right)
            * Mathf.Rad2Deg;
        float weaponAngle = isUsingAlt ? altWeapon.data.angle : mainWeapon.data.angle;
        aimObject.eulerAngles = new Vector3(aimObject.eulerAngles.x, dir + 90, aimObject.eulerAngles.z);
        aimRenderer.material.SetFloat("_Angle", weaponAngle);
    }

    private float GetAngleBetweenVectors(Vector2 v1, Vector2 v2)
    {
        return Mathf.Repeat(Mathf.Acos(Vector2.Dot(v1, v2)) * Mathf.Sign(Vector3.Cross(v1, v2).z), 2 * Mathf.PI);
    }
}