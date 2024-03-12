using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : SingletonMono<UIMgr>
{
    [Header("数据对象区")]
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerBase PlayerSc;



    [Header("UI组件区")]
    public Scrollbar hpBar;
    public Scrollbar beatBar;
    public Image DashCD; 


    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerSc = Player.GetComponent<PlayerBase>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        // 对于Scrollbar调整的是size
        hpBar.size = PlayerSc.nowHp / PlayerSc.maxHp;
        beatBar.size = PlayerSc.nowBeatValue / (100.0f); // 最大值反正是100
        DashCD.fillAmount = PlayerSc.dashTimer / PlayerSc.dashCD; 

    }
}
