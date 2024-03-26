using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Fire : Enemy
{
    [Header("远程敌人")]
    public GameObject bullet;
    public float bulletSpeed;

    public override void Attack()
    {
        Audio_attack.Play();
        GameObject bullet_temp = Instantiate(bullet, transform.position, Quaternion.identity);
        bullet_temp.GetComponent<Bullet>().SetBullet( attack , this); //将子弹伤害设置为角色攻击力

        // 计算到玩家的方向
        Vector2 direction = (Player.transform.position - transform.position).normalized;
       // Debug.Log("Dir" + direction);
        bullet_temp.GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed, ForceMode2D.Impulse);
    }


}
