﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 蓝图界面
/// </summary>
public class BluePrintPanel : MonoBehaviour
{
    //单例模式
    public static BluePrintPanel instance;

    public Scrollbar scrollbar;//垂直滑动条
    public Transform bluePrintList;//蓝图列表
    public GameObject bluePrintPrefab;//蓝图预制体
    public int[] towerIdArray;//玩家能解锁的塔的编号数组 

    private Animation anim;
    private int curIndex = 0;//当前即将解锁的塔编号
    private bool isOpen = false;

    private void Awake()
    {
        instance = this;
        anim = gameObject.GetComponent<Animation>();        
    }

    private void Start()
    {
        //默认一开始就有基础炮塔图纸
        InventNewTower();
    }

    /// <summary>
    /// 面板进入游戏界面
    /// </summary>
    public void OnPanelEnter()
    {      
        if(!isOpen && !anim.isPlaying)
        {
            scrollbar.value = 1;
            isOpen = true;
            anim.Play("BluePrintPanelEnter");
        }
    }

    /// <summary>
    /// 面板离开游戏界面
    /// </summary>
    public void OnPanelExit()
    {       
        if (isOpen && !anim.isPlaying)
        {
            isOpen = false;
            anim.Play("BluePrintPanelExit");
        }
    }

    /// <summary>
    /// 发明新的塔图纸，供玩家升级时调用
    /// </summary>
    public void InventNewTower()
    {
        TowerInfo newTowerInfo = TowerInfos.instance.GetTowerInfo(towerIdArray[curIndex]);
        curIndex++;
        //安全校验
        if (newTowerInfo != null)
        {
            //实例化新的图纸，加入到图纸列表中
            GameObject go = Instantiate(bluePrintPrefab, bluePrintList);
            BluePrint bluePrint = go.GetComponent<BluePrint>();
            bluePrint.InitBluePrint(newTowerInfo);
        }
    }
}
