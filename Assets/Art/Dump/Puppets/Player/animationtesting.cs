using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationtesting : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform model;
    


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Dash");
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("Hurt");
        }


        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("IsRunning", true);

            if(model.localScale.x <0)
            { model.localScale = new Vector3(Mathf.Abs(model.localScale.x), model.localScale.y, model.localScale.z); }
            

        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsRunning", true);

            if (model.localScale.x > 0) 
            { model.localScale = new Vector3(model.localScale.x * -1, model.localScale.y, model.localScale.z); }
            
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetMouseButtonDown(0)) // Left click
        {
            animator.SetTrigger("Attack1");
        }
        if (Input.GetMouseButtonDown(1)) // Left click
        {
            animator.SetTrigger("Attack2");
        }

        if (Input.GetMouseButtonDown(2)) // Left click
        {
            animator.SetTrigger("Attack3");
        }
    }
}