using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *    
 *    
    protected override void ObjAwake()
    {

    }

    protected override void ObjStart()
    {

    }

    protected override void ObjUpdate()
    {
        
    }
*/

public class BaseObj : MonoBehaviour
{
    [Header("基础数值")]
    public float maxHp;
    public float nowHp;

    public float attack; // 攻击力，这里就是直接表示成功命中会有多少伤害 
    public float speed;


    [Header("节奏响应部分")]
    public bool isRhyObj = false; //是否是可以响应节奏的物体
    public bool isRhyAct = false;

    private void Awake()
    {
        nowHp = maxHp;  // 初始生命即为最大值
        ObjAwake();


    }

    protected virtual void ObjAwake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if(isRhyObj)
        {
            // 如果是在节奏系统中的物体，需要注册
            RhythmMgr.GetInstance().RegistertObj(this);
        }
        ObjStart();
    }

    protected virtual void ObjStart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(nowHp <= 0)
        {
            Death();
        }

        if(isRhyObj)
        {
            if(isRhyAct)
            {
                RhyAction(); 
                isRhyAct=false;
            }
        }

        ObjUpdate();
    }

    protected virtual void ObjUpdate()
    {
        // 每一个Obj自己的Update

    }

    public void Death()
    {
        if (isRhyObj)
        {
            RhythmMgr.GetInstance().RemoveObj(this); // 解除注册
        }

        ObjDeath();
    }

    public virtual void ObjDeath()
    {
        // 各个物体在死亡的时候的额外操作
    }

    public virtual void Hurt(float _damage)
    {
        if(gameObject.CompareTag("Wall"))
        {
            // 目前不做墙体伤害
        }
        else
        {
            
            nowHp -= _damage;
        }
    }

    public virtual void Heal(float _heal)
    {
        if(nowHp + _heal <= maxHp)
        {
            nowHp += _heal;
        }

        if(nowHp > maxHp)
        {
            nowHp = maxHp;
        }
    }

    public void RhyActOn()
    {
        // 将Rhy操作打开，具体释放时机会在Update中进行
        isRhyAct = true;
    }

    public virtual void RhyAction()
    {
        // 响应节奏系统的具体操作
    }
    
}
