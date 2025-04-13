using UnityEngine;

public class FinalCutsceneEnding : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        PlayerController.instance.onDiedEvent += Show;
        animator = GetComponent<Animator>();
    }

    public void Show()
    {
        PlayerController.instance.onDiedEvent -= Show;
        animator.SetTrigger("Show");
    }
}