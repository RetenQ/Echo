using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("基础组件")]
    [SerializeField] private Rigidbody2D rb;

    // 子弹

    public float damage;
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
    public void SetBullet(float _damage)
    {
        this.damage = _damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
        //注：子弹碰到后不会摧毁
        if(isActive)
        {
            if (collision.CompareTag(targetStr))
            {
                collision.gameObject.GetComponent<BaseObj>().Hurt(damage);  //造成伤害

                rb.velocity = Vector3.zero;

                //Destroy(gameObject);
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
