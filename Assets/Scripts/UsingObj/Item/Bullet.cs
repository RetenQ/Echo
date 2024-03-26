using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("基础组件")]
    [SerializeField] private Rigidbody2D rb;

    // 子弹
    public BaseObj shooter; //射出子弹的对象
    public float damage;
    public float damageMul = 1.0f;
    public float maxLifeTime;
    public string targetStr;
    public string ignoreStr;

    [SerializeField] private bool isActive = true; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (maxLifeTime >= 0.001f)
        {
            maxLifeTime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetBullet(  float _damage , BaseObj _shooter)
    {
        this.shooter = _shooter;
        this.damage = _damage;
    }

    public void SetBullet( float _damage , float _mul , BaseObj _shooter)
    {
        this.shooter = _shooter;

        this.damage = _damage;
        this.damageMul = _mul;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        //注：子弹碰到后不会摧毁
        if(isActive)
        {
            if(collision.CompareTag("Wall"))
            {
                //碰到墙就只停下了
                rb.velocity = Vector3.zero;
                Destroy(gameObject);

            }

            if (collision.CompareTag(targetStr))
            {
                 // Debug.Log("!!!!!!");
                collision.gameObject.GetComponent<BaseObj>().Hurt(damage * damageMul , shooter);  //造成伤害

                rb.velocity = Vector3.zero;

                Destroy(gameObject);
            }
            else if (!collision.CompareTag(ignoreStr))
            {
                //Destroy(gameObject);
            }



            // 保证子弹只会被触发一次
            isActive = false;
        }

    }
}
