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

    private void Awake()
    {
        nowHp = maxHp;  // 初始生命即为最大值
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nowHp <= 0)
        {
            Death();
        }

        ObjUpdate();
    }

    public virtual void ObjUpdate()
    {
        // 每一个Obj自己的Update

    }

    public virtual void Death()
    {

    }

    public virtual void Hurt(float _damage)
    {
        nowHp = _damage;
    }
    
}
