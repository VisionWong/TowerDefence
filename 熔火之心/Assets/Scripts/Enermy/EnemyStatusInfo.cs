﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人状态信息类
/// </summary>
public class EnemyStatusInfo : MonoBehaviour
{
    [Header("血条")]
    public SmoothSlider hpBarSlider;
    public bool Isdead;
    ParticleSystem blood;
    /// <summary>
    /// 敌人执行攻击动作的范围与伤害距离的差距
    /// </summary>
    public float atkExecuteRange;
    /// <summary>
    /// 攻击范围
    /// </summary>
    public float atkRange;
    /// <summary>
    /// 当前血量
    /// </summary>
    public float currentHP=100;
    /// <summary>
    /// 最大血量
    /// </summary>
    public float maxHP=100;
    [Header("经验值")]
    public int exp;

    private Player player;

    private void Awake()
    {
        //BOSS一登场就出现状态栏
        if(gameObject.tag == Tags.boss)
        {
            BossStatusUI.instance.ShowStatus();
            hpBarSlider = GameObject.FindGameObjectWithTag(Tags.bossSlider).GetComponent<SmoothSlider>();
            print(hpBarSlider.gameObject.name);
        }     
        blood = GetComponent<ParticleSystem>();
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Player>();
        hpBarSlider.InitValue(currentHP, maxHP);
    }


    public void Damage(float amount)
    {
        //如果敌人已经死亡 则退出(防止虐尸)
        if (currentHP <= 0)
        {
            Isdead = true;
            return;
        }
        currentHP -= amount;
        if (currentHP < 0) currentHP = 0;
        hpBarSlider.ChangeValue(currentHP);
        if (currentHP <= 0)
            Death();
        blood.Play();
        //如果是BOSS，则半血一下进入狂暴模式
        if (gameObject.tag == Tags.boss)
        {
            //咆哮一声
            //音乐变更
            AudioManager.instance.PlayCreazyBossBGM();
        }
    }
    /// <summary>
    /// 死亡延迟时间
    /// </summary>
    public float deathDelay = 10;
    

    private void Start()
    {
        blood = GetComponentInChildren<ParticleSystem>();

    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Death()
    {
        Isdead = true;
        //如果是BOSS则把状态栏隐藏,并游戏胜利
        if (gameObject.tag == Tags.boss)
        {
            BossStatusUI.instance.Hide();
            GameManager.instance.GameWin();
        }
        //播放动画
        var anim = GetComponent<EnemyAnimation>();
        anim.Play(anim.deathName);
        if(GetComponent<EnemyAI>()==null)
        GetComponent<BossAI>().state = BossAI.State.Death;
        else GetComponent<EnemyAI>().state = EnemyAI.State.Death;
        //销毁当前游戏物体
        Destroy(gameObject, deathDelay);

        //修改状态
 
        //给生成器传输当前死亡数加一
        SystemLevelEditor.instance.DeathCnt();
        //给人物加经验
        player.GetExp(exp);
    }

}
