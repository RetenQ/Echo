using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isRhyObj)
        {
            // 如果是在节奏系统中的物体，需要注册
            RhythmMgr.GetInstance().RegistertObj(this);
        }
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

    public virtual void ObjUpdate()
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
        nowHp = _damage;
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
