﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 关卡完成UI
/// </summary>
public class CompletedTxt : MonoBehaviour
{
    //单例模式
    public static CompletedTxt instance;

    private Animation anim;
    private Image img;

    private void Awake()
    {
        instance = this;
        anim = gameObject.GetComponent<Animation>();
        img = gameObject.GetComponent<Image>();
    }

    public void Show()
    {
        img.enabled = true;
        anim.Play();
        Invoke("Hide", 2.0f);
    }

    private void Hide()
    {
        img.enabled = false;
    }
}
