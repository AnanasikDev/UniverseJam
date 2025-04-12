using UnityEngine;

public class PlayerAsZombieAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform model;


    private void Start()
    {
        PlayerController.instance.healthComp.onDamagedEvent += (float val) => animator.SetTrigger("Hurt");
        PlayerController.instance.onMovingEvent += (Vector2 dir) =>
        {
            if (dir.x != 0)
            {
                animator.SetBool("Moving", true);
                Flip(Mathf.Sign(dir.x));
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        };

        PlayerController.instance.onStoppedEvent += () =>
        {
            animator.SetBool("Moving", false);
        };

        PlayerController.instance.playerAttack.mainWeapon.onUsedEvent += () =>
        {
            animator.SetTrigger("Attack");
        };
        PlayerController.instance.playerAttack.altWeapon.onUsedEvent += () =>
        {
            animator.SetTrigger("Attack");
        };

        PlayerController.instance.onDiedEvent += () =>
        {
            animator.SetTrigger("Dead");
        };
    }

    private void Flip(float sign)
    {
        model.localScale = new Vector3(Mathf.Abs(model.localScale.x) * sign, model.localScale.y, model.localScale.z);
    }
}