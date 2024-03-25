using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackArea : MonoBehaviour
{
    [Header("基础数值")]
    [SerializeField] private bool isAttack = false;  // 避免重复计算
    [SerializeField] private float attack;


    public void setAttackArea(float _attack)
    {
        this.attack = _attack; // 为PlayerBase用的
    }


}
