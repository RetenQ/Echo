using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player_AttackArea : MonoBehaviour
{
    [Header("基础数值")]
    [SerializeField] private bool isAttack = false;  // 避免重复计算
    [SerializeField] private float attack;

    [SerializeField] private int playerfacing; // 朝向

    public float maxAttackTime;
    [SerializeField] private float maxAttackTimer; 

    public float attackRange = 5f;
    private Collider2D[] enemiesInRange;

    
    public PlayerBase playerSC;

    private void Awake()
    {
        playerSC = GameObject.FindWithTag("Player").GetComponent<PlayerBase>();
    }

    void FixedUpdate()
    {
        if(maxAttackTimer <= 0.0f)
        {
            // 恢复
            maxAttackTimer = maxAttackTime;
            StopAttack();
        }
        else
        {
            maxAttackTimer -= Time.fixedDeltaTime; 
        }
    }

    // PlayerBase发送攻击->PlayerBase修改isAttack = false ->
    // StartAttack执行攻击 , 执行之后修改isAttack = true-> 一定条件时执行StopAttack
    // -> StopAttack执行对应操作

    private void OnEnable()
    {
        //  StartAttack();
        // 改为在Player的动画的关键帧检测
    }

    public void StartAttack()
    {
        if(isAttack == false)
        {
            // 动画在PlayerBase播放！！

            // 执行操作 
            // 用手动检测代替Trigger范围检测
            // 检测范围内的所有敌人
            enemiesInRange = Physics2D.OverlapCircleAll(transform.position, attackRange);

            foreach (Collider2D enemyCollider in enemiesInRange)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    // 记录在攻击范围内的敌人
                    //Debug.Log("Enemy detected: " + enemyCollider.gameObject.name);
                    enemyCollider.GetComponent<Enemy>().Hurt(attack, playerSC);

                }
            }
            
            isAttack = true;
        }

    }

    private void StopAttack()
    {
        gameObject.SetActive(false); // 关掉
        playerSC.isAttack = false; 
    }

    public void setAttackArea(float _attack , int _facing , PlayerBase _player)
    {
        this.playerfacing = _facing;
        this.attack = _attack; // 为PlayerBase用的
        this.isAttack = false; // 自动设置还没攻击

        this.playerSC = _player;       
    }

    void OnDrawGizmos()
    {
        OnDrawGizmosSelected();
    }

    void OnDrawGizmosSelected()
    {
        // 在编辑器中绘制攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
