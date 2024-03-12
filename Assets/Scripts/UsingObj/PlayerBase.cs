using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Chara
{

    [Header("Player属性")]
    [SerializeField] private bool inRhy; // 是否在节奏区间
    [SerializeField] private bool islock = false; //锁定时无法操作
    [SerializeField] private bool isdash = false; 
    private Vector2 movement;
    [SerializeField] private Vector2 lastMovement;//最后一次非0方向

    public Vector2 mouseLocation;
    private Vector2 ToMouseDirection;

    [SerializeField] private int facing;

    [Header("冲刺数据")]
    public float dashCD = 2;
    public float dashMul;  // 此处是dash的加速倍数
    [SerializeField] private float dashTimer = -99f;
    public float maxDashTime = 1.5f;
    public float stopDashTime = 0.1f; //多久可以手动停止
    [SerializeField] private float startDashTimer;
    public GameObject trailEffect; 
    public GameObject trailEffect_ex; 
    public GameObject trailEffect_last; 

    [Header("节奏区域")]
    public float nowBeatValue; // 目前压点的得分 ， 最高100

    // public float nowRhyPoint; //得分

    [Header("子弹区")]
    public GameObject bullet;
    public GameObject bullet_ex;
    public Transform firePosition;
    public float bulletSpeed; 
    public float bulletSpeed_ex; 

    [Header("组件")]
    public Rigidbody2D rb;
    public Collider2D col;
    public Animator animator;
    public SpriteRenderer sr;

    [Header("测试数据")]
    public int RightD; 
    public int ErrorD; 

    protected override void ObjAwake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    protected override void ObjStart()
    {
/*        sr.color = Color.blue;
*/
    }

    protected override void ObjUpdate()
    {
        // 测试用
        if(Input.GetKeyDown(KeyCode.K)) {
            if (inRhy)
            {
                RightD++;
            }
            else
            {
                ErrorD++;
            }
        }

        // Debug.Log("R:" + RightD + " || E:" + ErrorD);


        DataUpdater();

        if (!islock)
        {
            if (!isdash)
            {
                //非冲刺状态下进行的操作

                Movement();

                if (lastMovement.x == -1)       facing = 1; //左
                else if (lastMovement.x == 1)   facing = 2; //右
                else if (lastMovement.y == 1)   facing = 3; //上
                else                            facing = 4; //下
                
                if(facing!=0  && (movement.x != 0 || movement.y != 0))
                {
                    PlayAnim("run");//运动
                }
                else
                {
                    PlayAnim("idle"); 
                }

                if (Input.GetKeyDown(KeyCode.LeftControl) && dashTimer <= 0)
                {
                    DashOn();
                }
            }
            else
            {
                PlayAnim("dash");
                if(Input.GetKeyDown(KeyCode.LeftControl) && startDashTimer <= maxDashTime - stopDashTime)
                {
                    // 如果启动后再次按下冲刺
                    startDashTimer = 0;
                        DashOff();

                }

                // 冲刺Mode
                startDashTimer -= Time.deltaTime;
                    if (startDashTimer <= 0)
                    {
                    // 时间到了结束状态
                        DashOff();

                    }
                    else
                    {

                        // 处于冲刺状态下进行冲刺
                        // 这里位置使用的是最后移动的方向
                        rb.velocity = lastMovement * speed * dashMul;  
                    }
            }

            if (Input.GetMouseButtonDown(0))
            {
                //左键
                Fire();
            }

            if (Input.GetMouseButtonDown(1))
            {
                //右键
            }
        } 
    }

    private void FixedUpdate()
    {
        FiexdDataUpdater();
    }

    private void DashOn()
    {
        if (inRhy)
        {
            trailEffect_ex.SetActive(true);
            trailEffect_last = trailEffect_ex;

        }
        else
        {
            trailEffect.SetActive(true);
            trailEffect_last = trailEffect;
        }

        isdash = true;
        startDashTimer = maxDashTime; // Timer设置为最大冲刺时间倒计时

        dashTimer = dashCD;
    }

    private void DashOff()
    {
        trailEffect_last.SetActive(false);
        isdash = false;

    }

    public void Movement()
    {

        // 更新movement的参数
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x !=0 || movement.y !=0)
        {
            lastMovement = movement; //记录最后方向
        }

        //！ 注：GetAxisRaw只返回{-1 ,0 , 1}，不做手柄优化且希望操作干净，故这里使用GetAxisRaw

        // 直接使用velocity更好用，所以这里改了
        //!默认乘50
       // rb.MovePosition(rb.position + movement * speed * Time.deltaTime *50);
        //这里直接使用MovePosition
        //因为是放在Update里面且需要每帧控制，所以使用deltaTime

        rb.velocity = new Vector2(movement.x * speed  , movement.y *speed);


        // anim.SetBool("isRun",true);
        if (movement.x != 0 || movement.y != 0)
        {
            //调整朝向
            //此处默认角色朝向是右侧
            if (movement.x > 0)
            {
                transform.localScale = new Vector3(movement.x, transform.localScale.y, transform.localScale.z);
                
            }

            if (movement.x < 0)
            {
                transform.localScale = new Vector3(movement.x, transform.localScale.y, transform.localScale.z);
            }


            // 左右上下 1234
/*            if (lastMovement.x == -1) return 1; //左
            else if (lastMovement.x == 1) return 2; //右
            else if (lastMovement.y == 1) return 3; //上
            else if (lastMovement.y == -1) return 4; //下
            else return 4;*/

                //return 0;

            }
            else
        {


            // return 0;
        }
    }

    private void DataUpdater()
    {
        mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ToMouseDirection = (mouseLocation - new Vector2(transform.position.x, transform.position.y)).normalized;
    }

    private void FiexdDataUpdater()
    {
        // 需要准确计时的数据放在这里
        if (dashTimer >= 0)
        {
            // isdash = false;
            dashTimer -= Time.deltaTime;
        }
    }


    private void Fire()
    {
        if (inRhy)
        {
            GameObject bullet_temp = Instantiate(bullet_ex, firePosition.position, Quaternion.identity);
            bullet_temp.GetComponent<Bullet>().SetBullet(attack , 2.0f);
            bullet_temp.GetComponent<Rigidbody2D>().AddForce(ToMouseDirection * bulletSpeed_ex, ForceMode2D.Impulse);
        }
        else
        {
            GameObject bullet_temp = Instantiate(bullet, firePosition.position, Quaternion.identity);
            bullet_temp.GetComponent<Bullet>().SetBullet(attack);
            bullet_temp.GetComponent<Rigidbody2D>().AddForce(ToMouseDirection * bulletSpeed, ForceMode2D.Impulse);
        }

    }

    public void PlayerRhyOn()
    {
        //Debug.Log("ON");
        inRhy = true;
       // sr.color = Color.red; 
    }

    public void PlayerRhyOff()
    {
        //Debug.Log("OFF!");

        inRhy = false;
        //sr.color = Color.blue;

    }

    // 控制beat的区域
    public void AddBeatPont()
    {
        if(nowBeatValue < 100)
        {
            nowBeatValue += 10; //默认+10
        }
    }

    public void AddBeatPont(float _value)
    {
        if(nowBeatValue + _value <= 100)
        {
            nowBeatValue += _value; 
        }
    }

    public void ClearBeatValue()
    {
        // 触发一些技能

        //
        nowBeatValue = 0;
    }

    public void PlayAnim(string _name)
    {
        string res = _name;

/*        if(_name.Equals("idle"))
        {

        }*/

        // 左右上下 1234
        if (facing == 3)
        {
            res = "p1-b-" + _name;
        }
        else if (facing == 4)
        {
            res = "p1-f-" + _name;
        }
        else
        {
            res = "p1-s-" + _name;

        }

        //Debug.Log(res);
        animator.Play(res);

    }

}
