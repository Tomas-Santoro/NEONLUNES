using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimatorEvents : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void EndSlash()
    {
        animator.Play("NinjaSwordIdle", 0,0f);
    }
}
