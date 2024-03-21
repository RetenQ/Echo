using SonicBloom.Koreo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 管理BGM的东西
public class RhythmMgr : SingletonMono<RhythmMgr>
{
    [Header("BGM设置区")]
    public GameObject realPlayer;
    public AudioSource realAudio; 

    [SerializeField]private bool isActive = false;
    public string eventID;
    public float RhyTolerance; // isRhy为True多久之后改回false
    public float RhyToleranceTimer ;
    public float startPlay ; // 倒计时多久之后开始节奏系统
    public float delayPlay ; // 延迟播放，实际上是可以提前多少秒踩点

    public bool isRhy = false;
    [SerializeField] private float timeToArrive; // 用于计算UI
    [SerializeField] private float delayPlay_Record;



    [Header("注册区")]
    [SerializeField]private GameObject Player; 
    [SerializeField]private PlayerBase PlayerSc; 
    public List<BaseObj> Objs;

    [Header("组件")]
    public AudioSource audioSource;
    public Image RhyBar; 

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
        audioSource = GetComponent<AudioSource>();

        realPlayer = transform.Find("realPlayer").gameObject;
        realAudio = realPlayer.GetComponent<AudioSource>(); // 真实的播放器
        realAudio.clip = audioSource.clip;

        delayPlay_Record = delayPlay;

        timeToArrive = delayPlay_Record; 

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
            if(startPlay <=0)
            {
                RhyMgrStart();

            }
            else
            {
                startPlay -= Time.fixedDeltaTime;
            }
        }

        // 延迟播放设置
        if (!realAudio.isPlaying)
        {
           if(isActive)
            {
                if(delayPlay <=0)
                {
                    PlayRealAudio();
                }
                else
                {
                    delayPlay -= Time.fixedDeltaTime;   
                }
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


        timeToArrive -= Time.fixedDeltaTime;

        RhyBar.fillAmount = (1.0f - (timeToArrive / delayPlay_Record)); 

    }

    private void DrumBeat(KoreographyEvent koreographyEvent)
    {
        // 到鼓点了干什么    
        NotifyObjs();
        PlayerRhyOn(); 
    }

    private void PlayRealAudio()
    {
        // 播放音乐谱。二者之间的延迟实际上就是delayPlay
        realAudio.Play();
    }

    private void RhyMgrStart()
    {
        // 播放节奏谱，也就是压点的谱
        isActive = true;
        audioSource.Play();
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
        //Debug.Log("ON RHY");

        timeToArrive = delayPlay_Record;


        isRhy = true; //设置为True
        // playerRhy = true;
        PlayerSc.PlayerRhyOn();
    }

    public void PlayerRhyOff()
    {
        //Debug.Log("OFF RHY");

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
