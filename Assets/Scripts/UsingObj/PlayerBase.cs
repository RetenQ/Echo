using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : Chara
{
    [Header("Player属性")]
    [SerializeField] private bool islock = false; //锁定时无法操作
    [SerializeField] private bool isdash = false; 
    private Vector2 movement;


    [Header("组件")]
    public Rigidbody2D rb;
    public Collider2D col;
    public Animator animator;

    protected override void ObjAwake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void ObjStart()
    {

    }

    protected override void ObjUpdate()
    {
        if(!islock)
        {
            if (!isdash)
            {
                //非冲刺状态下进行的操作
                Movement();

                //这里的代码是为了方便之前随时可以冲刺而进行的
                /*            if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
                            {
                                soundMgr.ClipPlay(2);
                                isdash = true;
                                startDashTimer = maxDashTime;

                                dashTimer = dashCD;
                            }*/
            }
            else
            {

                // 冲刺Mode
                /*            startDashTimer -= Time.deltaTime;
                            if (startDashTimer <= 0)
                            {
                                isdash = false;

                            }
                            else
                            {
                                rb.velocity = movement * dashSpeed;  
                            }*/
            }

            if (Input.GetMouseButtonDown(0))
            {
                //左键
            }

            if (Input.GetMouseButtonDown(1))
            {
                //右键
            }
        } 
    }

    public bool Movement()
    {

        // 更新movement的参数
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        //！ 注：GetAxisRaw只返回{-1 ,0 , 1}，不做手柄优化且希望操作干净，故这里使用GetAxisRaw

        Debug.Log(movement.x + " | " +  movement.y);

        //!默认乘50
        rb.MovePosition(rb.position + movement * speed * Time.deltaTime *50);
        //这里直接使用MovePosition
        //因为是放在Update里面且需要每帧控制，所以使用deltaTime


        // anim.SetBool("isRun",true);
        if (movement.x != 0 || movement.y != 0)
        {
            //调整朝向
            if (movement.x < 0)
            {
                transform.localScale = new Vector3(-(movement.x), transform.localScale.y, transform.localScale.z);
                
            }

            if (movement.x > 0)
            {
                transform.localScale = new Vector3(-(movement.x), transform.localScale.y, transform.localScale.z);
            }

            return true;
        }
        else
        {
            return false;
            //表示玩家没有移动
        }
    }
}
