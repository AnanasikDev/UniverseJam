using UnityEngine;

public class PlayerAnimator : MonoBehaviour
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
                animator.SetBool("IsRunning", true);
                if (Mathf.Abs(dir.x) > 0.001f)
                    Flip(Mathf.Sign(dir.x));
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
        };

        PlayerController.instance.onStoppedEvent += () =>
        {
            animator.SetBool("IsRunning", false);
        };

        PlayerController.instance.playerAttack.mainWeapon.onUsedEvent += () =>
        {
            if (Random.Range(0.0f, 1.0f) > 0.5f)
                animator.SetTrigger("Attack1");
            else
                animator.SetTrigger("Attack2");
        };
        PlayerController.instance.playerAttack.altWeapon.onUsedEvent += () =>
        {
            animator.SetTrigger("Attack3");
        };
    }

    private void Flip(float sign)
    {
        model.localScale = new Vector3(Mathf.Abs(model.localScale.x) * -sign, model.localScale.y, model.localScale.z);
    }
}