using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIFade : MonoBehaviour
{
    public static UIFade Instance
    {
        get
        {
            return instance;
        }
    }
    static UIFade instance;
    Image fade;
    void Start()
    {
        instance = this;
        fade = GetComponent<Image>();
        FadeOut(0.3f);
    }
    public void FadeIn(float time)
    {
        fade.raycastTarget = true;
        fade.DOFade(1,time);
    }
    public void FadeIn(float time,Ease ease)
    {
        fade.raycastTarget = true;
        fade.DOFade(1,time).SetEase(ease);
    }
    public void FadeIn(float time,Ease ease,Action action)
    {
        fade.raycastTarget = true;
        fade.DOFade(1,time).SetEase(ease).OnComplete(()=>
        {
            action();
        });
    }
    public void FadeOut(float time)
    {
        fade.raycastTarget = false;
        fade.DOFade(0,time);
    }
    public void FadeOut(float time,Ease ease)
    {
        fade.raycastTarget = false;
        fade.DOFade(0,time).SetEase(ease);
    }
    public void FadeOut(float time,Ease ease,Action action)
    {
        fade.raycastTarget = false;
        fade.DOFade(0,time).SetEase(ease).OnComplete(()=>
        {
            action();
        });
    }
}
