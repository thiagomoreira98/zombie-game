using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    public void Attack(bool attacking)
    {
        this.animator.SetBool("attacking", attacking);
    }

    public void Walk(float speed)
    {
        this.animator.SetFloat("walking", speed);
    }
}
