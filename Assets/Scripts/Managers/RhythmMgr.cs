using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 管理BGM的东西
public class RhythmMgr : SingletonMono<RhythmMgr>
{
    [Header("BGM设置区")]
    [SerializeField]private bool isActive = false;
    public string eventID;
    public float RhyTolerance; // isRhy为True多久之后改回false
    public float RhyToleranceTimer ;
    public float delayPlayer ; // 一开始延迟播放

    public bool isRhy = false; 



    [Header("注册区")]
    [SerializeField]private GameObject Player; 
    [SerializeField]private PlayerBase PlayerSc; 
    public List<BaseObj> Objs;

    [Header("组件")]
    public AudioSource AudioSource;

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
        AudioSource = GetComponent<AudioSource>();

/*        RhyInterval = (60 / RhyBpm) * RhyMul;
        Debug.Log(("!!! : || " + RhyInterval));

        RhyToleranceTimer = RhyTolerance;
        RhyIntervalTimer = RhyInterval;*/

        // kore
        Koreographer.Instance.RegisterForEvents(eventID, DrumBeat); // 注册

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isActive)
        {
            if(delayPlayer <=0)
            {
                RhyMgrStart();

            }
            else
            {
                delayPlayer -= Time.fixedDeltaTime;
            }
        }

        if (isRhy)
        {
            if(RhyToleranceTimer >= 0)
            {
                RhyToleranceTimer -= Time.fixedDeltaTime;
            }
            else
            {
                PlayerRhyOff();

            }
        }

    }

    private void DrumBeat(KoreographyEvent koreographyEvent)
    {
        // 到鼓点了干什么    
        NotifyObjs();
        PlayerRhyOn(); 
    }



    private void RhyMgrStart()
    {
        isActive = true;
        AudioSource.Play();
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
        Debug.Log("ON RHY");
        isRhy = true; //设置为True
        // playerRhy = true;
        PlayerSc.PlayerRhyOn();
    }

    public void PlayerRhyOff()
    {
        Debug.Log("OFF RHY");

        isRhy = false;
        RhyToleranceTimer = RhyTolerance; 
        // playerRhy = false;
        PlayerSc.PlayerRhyOff();
    }

    /// <summary>
    /// 通知所有的，由AI控制的，需要在节奏点上进行操作的Object进行响应
    /// </summary>
    public void SystemRhy()
    {
        NotifyObjs(); //通知
    }



    /*
     * 
     *     [Header("BGM设置区_旧版")]
    // public float RhyTolerance;
    public float RhyBgmName;

    public float RhyBpm;
    public float RhyMul; 

    public float RhyInterval; // 鼓点间隔，后期使用插件这块可能会弃用 
    public float RhyIntervalTimer; 
    [SerializeField] private bool isAvive = false;
    [SerializeField] private bool playerRhy = false;
     * 
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
                }


                if((RhyIntervalTimer <= RhyInterval - RhyTolerance)&& (RhyIntervalTimer > RhyTolerance) && (playerRhy == true)){
                    PlayerRhyOff();
                        }else if ((RhyIntervalTimer <= RhyTolerance) && (playerRhy == false))
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
     
     */
}
