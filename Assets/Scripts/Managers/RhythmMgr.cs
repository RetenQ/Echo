using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 管理BGM的东西
public class RhythmMgr : SingletonMono<RhythmMgr>
{
    [Header("BGM设置区")]
    public float RhyTolerance;
    public float RhyToleranceTimer;
    public float RhyBgmName;
    public float RhyInterval; // 鼓点间隔，后期使用插件这块可能会弃用 
    public float RhyIntervalTimer; 
    [SerializeField] private bool isAvive = false;
    [SerializeField] private bool playerRhy = false;

    [Header("注册区")]
    [SerializeField]private GameObject Player; 
    [SerializeField]private PlayerBase PlayerSc; 
    public List<BaseObj> Objs;

    protected override void Awake()
    {
        base.Awake();
        Objs = new List<BaseObj>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player"); 
        PlayerSc = Player.GetComponent<PlayerBase>();
        RhyToleranceTimer = RhyTolerance;
        RhyIntervalTimer = RhyInterval;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isAvive)
        {
            if (RhyToleranceTimer <= 0.0f)
            {
                //开始
                RhyMgrStart();
                RhyToleranceTimer = RhyTolerance;


            }
            else
            {
                RhyToleranceTimer -= Time.fixedDeltaTime;
            }
        }
        else
        {
            if(RhyIntervalTimer > 0.0f)
            {
                RhyIntervalTimer -= Time.fixedDeltaTime;
                
/*                if((RhyIntervalTimer <= RhyTolerance) && (playerRhy == false))
                {
                    PlayerRhyOn();//激活
                }else if((RhyIntervalTimer <= RhyInterval-RhyTolerance) &&(playerRhy == true) )
                {
                    PlayerRhyOff(); 

                   //关闭
                }*/


                if((RhyIntervalTimer <= RhyInterval - RhyTolerance)&& (RhyIntervalTimer > RhyTolerance) && (playerRhy == true)){
                    PlayerRhyOff();
                }else if((RhyIntervalTimer <= RhyTolerance) && (playerRhy == false))
                {
                    PlayerRhyOn();//激活

                }
            }
            else
            {
                // RhyIntervalTimer到0后，通知并归位
                NotifyObjs();
                RhyIntervalTimer = RhyInterval;

            }

        }



    }

    private void RhyMgrStart()
    {
        isAvive = true; 
        // 播放
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
        playerRhy = true;
        PlayerSc.PlayerRhyOn();
    }

    public void PlayerRhyOff()
    {
        playerRhy = false;
        PlayerSc.PlayerRhyOff();
    }

    /// <summary>
    /// 通知所有的，由AI控制的，需要在节奏点上进行操作的Object进行响应
    /// </summary>
    public void SystemRhy()
    {
        NotifyObjs(); //通知
    }
}
