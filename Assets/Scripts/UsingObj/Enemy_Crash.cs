using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_Crash : Enemy
{
    [Header("撞击敌人")]
    public float dashSpeed = 15f; // 冲撞速度
    public int attackDamage; //攻击伤害
    [SerializeField] private bool hasAttack = false; //已经攻击过 ， 防止多次重复结算
    public float maxDistance; //最大冲刺距离

    public override void Attack()
    {
        // 冲撞的部分需要修改Nav ， 暂不做
        hasAttack = false;
        // 如果目标物体存在
        if (target != null)
        {
            //Debug.Log("CrashAttack");
            //Audio_attack.Play();

            // 计算冲撞的方向，单位向量
            Vector2 direction = (target.transform.position - transform.position).normalized;

            // 获取物体的刚体组件，如果有的话
            Rigidbody2D rb = GetComponent<Rigidbody2D>();

            // 如果刚体组件存在
            if (rb != null)
            {
                // 给刚体施加力，使其向目标物体冲撞
                rb.velocity = direction * dashSpeed; 
            }


        }
    }

    // 检查距离的协程

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!hasAttack)
            {
                hasAttack = true;
                PlayerSc.Hurt(attackDamage,this);
                hasAttack = true;

            }

        }
    }
}
