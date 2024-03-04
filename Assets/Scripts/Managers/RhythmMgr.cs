using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 管理BGM的东西
public class RhythmMgr : SingletonMono<RhythmMgr>
{

    [Header("注册区")]
    public List<BaseObj> Objs;

    protected override void Awake()
    {
        base.Awake();
        Objs = new List<BaseObj>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    // 控制Obj
    public void RegistertObj(BaseObj obj)
    {
        Objs.Add(obj);
    }

    public void RemoveObj(BaseObj obj)
    {
        Objs.Remove(obj);
    }
   
    public void NotifyObjs()
    {
        foreach (BaseObj _obj in Objs)
        {
            _obj.RhyActOn(); 
        }
    }

    public void PlayerRhyOn()
    {

    }

    public void PlayerRhyOff()
    {

    }

    /// <summary>
    /// 通知所有的，由AI控制的，需要在节奏点上进行操作的Object进行响应
    /// </summary>
    public void SystemRhy()
    {
        NotifyObjs(); //通知
    }
}
