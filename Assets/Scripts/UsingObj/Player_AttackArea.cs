using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Player_AttackArea : MonoBehaviour
{
    [Header("基础数值")]
    [SerializeField] private bool isAttack = false;  // 避免重复计算
    [SerializeField] private float attack;
    [SerializeField] public Animator animator;

    [SerializeField] private int playerfacing; // 朝向

    public float attackRange = 5f;
    private Collider2D[] enemiesInRange;

    [SerializeField] private bool wasInTransition = true; // 用于记录上一帧是否在状态转换中
    
    public PlayerBase playerSC;


    public float maxAttackTime;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
/*        // 检查当前动画状态是否处于结束状态或切换状态
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        bool isInTransition = animator.IsInTransition(0);

        // 如果当前不在状态转换中，并且上一帧在转换中，则表示动画刚刚结束或者切换
        if (stateInfo.normalizedTime >= 1f ||(!isInTransition && wasInTransition))
        {
            StopAttack(); // 调用函数A
        }

        // 更新wasInTransition的值，用于下一帧的判断
        wasInTransition = isInTransition;*/
    }

    // PlayerBase发送攻击->PlayerBase修改isAttack = false ->
    // StartAttack执行攻击 , 执行之后修改isAttack = true-> 一定条件时执行StopAttack
    // -> StopAttack执行对应操作

    private void OnEnable()
    {
        StartAttack();
    }

    private void StartAttack()
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


    void OnDrawGizmosSelected()
    {
        // 在编辑器中绘制攻击范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
