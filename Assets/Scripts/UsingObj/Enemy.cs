using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Chara
{
    [Header("×é¼þ")]
    public Animator animator;
    protected override void ObjAwake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void ObjStart()
    {

    }

    protected override void ObjUpdate()
    {

    }

    public override void Hurt(float _damage)
    {
        Debug.Log("?"); 
        nowHp -= _damage;
        animator.Play("Hurt"); 
    }
}
