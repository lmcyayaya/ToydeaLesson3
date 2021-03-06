﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIBreathFade : MonoBehaviour
{
    public float fadeInAmount;
    public float duration;
    public Ease ease;
    Image image;
    Tween tween;
    void Awake() 
    {
        image = GetComponent<Image>();   
    }
    public void FadeIn()
    {
        if(tween!=null)
        {
            tween.Kill(); 
            tween = image.DOFade(fadeInAmount,duration).SetEase(ease).OnComplete(()=>
            {
                tween = null;
                FadeOut();
            });
        }
        else
        {
            tween = image.DOFade(fadeInAmount,duration).SetEase(ease).OnComplete(()=>
            {
                tween = null;
                FadeOut();
            });
        }
       
    }
    public void FadeOut()
    {
        if(tween!=null)
        {
            tween.Kill();
            tween = image.DOFade(0,duration).SetEase(ease).OnComplete(()=>
            {
                tween = null;
                FadeIn();
            });
        }
        else
        {
            tween = image.DOFade(0,duration).SetEase(ease).OnComplete(()=>
            {
                tween = null;
                FadeIn();
            });
        }
    }
    public void Stop()
    {
        image.DOFade(0,0);
        tween.Pause();
        tween.Kill();
        tween = null;
    }
}
