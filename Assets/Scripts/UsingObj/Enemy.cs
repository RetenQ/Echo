using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : Chara
{

    // 后期将追踪改为Nav
    //public EnemyRoom room;

    public NavMeshAgent2D Nav2dAgent;

    [Header("敌人参数")]
    [SerializeField] private bool isActive; //敌人是否启动
    [SerializeField] protected GameObject Player;
    [SerializeField] protected PlayerBase PlayerSc;

    public GameObject target;// 攻击等的目标，一般是Player
    public float AttackCD; // CD到了执行Attack参数
    public float activeArrange; //启动范围 - 目前先不用
    public float attackArrange; // 攻击范围 
    [SerializeField] protected float attackCDTimer;

    [Header("组件")]
    public Animator animator;
    public AudioSource audio;  //
    public Rigidbody2D rb;
    public Image hpBar;

    [Header("音效")]
    public AudioSource Audio_attack;
    public AudioSource Audio_hurt;

    protected override void ObjAwake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void ObjStart()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();
        nowHp = maxHp;

        target = Player;  // 默认设置为player

        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();

        hpBar = transform.Find("Char_State_UI").Find("HP").gameObject.GetComponent<Image>();

        Nav2dAgent = GetComponent<NavMeshAgent2D>();
        //room.registerEnemy(this);

        attackCDTimer = AttackCD;

    }

    protected override void ObjUpdate()
    {
        DataUpdater();

        FindPlayer();

        if (Vector2.Distance(transform.position, target.transform.position) <= attackArrange)
        {
            // 玩家进入攻击范围
            if (attackCDTimer >= 0.01f)
            {
                attackCDTimer -= Time.deltaTime;
            }
            else
            {
                //Debug.Log("?");
                Attack();
                // Debug.Log("!");

                attackCDTimer = AttackCD;
            }
        }

    }

    private void DataUpdater()
    {
        hpBar.fillAmount = nowHp * 1.0f / maxHp;

    }

    public override void Hurt(float _damage, BaseObj _hurtby)
    {
        // Debug.Log("?"); 
        nowHp -= _damage;
        Audio_hurt.Play();
        animator.Play("Hurt");

        _hurtby.UpdateLastAttack(this);
        lastHurtby = _hurtby;
    }

    public virtual void FindPlayer()
    {
        // 寻找player
        Nav2dAgent.destination = PlayerSc.transform.position;
    }
    public virtual void Attack()
    {
        Debug.Log("敌人攻击");
        // 
    }




    public float radius = 5f;

    // 圆的分段数，越大越平滑，可以在编辑器中调整
    public int segments = 50;

    // Gizmos的颜色，可以在编辑器中调整
    public Color color = Color.white;

    // 在场景视图中绘制Gizmos
    private void OnDrawGizmos()
    {
        radius = attackArrange;
        color = Color.yellow;
        // 设置Gizmos的颜色
        Gizmos.color = color;

        // 获取物体的位置
        Vector3 position = transform.position;

        // 计算每个分段的角度，单位为弧度
        float angle = 2 * Mathf.PI / segments;

        // 遍历每个分段
        for (int i = 0; i < segments; i++)
        {
            // 计算当前分段的起点坐标，相对于物体位置
            float x1 = radius * Mathf.Cos(i * angle);
            float y1 = radius * Mathf.Sin(i * angle);
            Vector3 point1 = new Vector3(x1, y1, position.z) + position;

            // 计算下一个分段的起点坐标，相对于物体位置
            float x2 = radius * Mathf.Cos((i + 1) * angle);
            float y2 = radius * Mathf.Sin((i + 1) * angle);
            Vector3 point2 = new Vector3(x2, y2, position.z) + position;

            // 绘制两个点之间的线段，形成圆弧
            Gizmos.DrawLine(point1, point2);
        }
    }
}
