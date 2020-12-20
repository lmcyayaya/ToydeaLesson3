using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Step18 : Step
{
    public Image backGround;
    public Sprite sprite;
    public Image arrow;
    public Image arrow2;
    public Image[] masks;
    public Image askWindow;
    Vector3 originPos;
    Vector3 originPos2;
    Tween tween;
    Tween tween1;
    Tween tween2;
    Tween tween3;
    public override void ShowUp()
    {
        foreach(Image mask in masks)
        {
            mask.DOFade(0.78f,0.15f);
        }
        arrow.DOFade(1,0.15f);
        arrow2.DOFade(1,0.15f);
        backGround.sprite = sprite;
        askWindow.rectTransform.DOScale(Vector2.one,0.3f);
        originPos = arrow.rectTransform.anchoredPosition;
        originPos2 = arrow2.rectTransform.anchoredPosition;
        ArrowMove();
    }
    public override void Close()
    {   
        askWindow.rectTransform.DOScale(Vector2.zero,0.15f).OnComplete(()=>
        {
            
            tween.Pause();
            tween.Kill();
            tween1.Pause();
            tween1.Kill();
            tween2.Pause();
            tween2.Kill();
            tween3.Pause();
            tween3.Kill();
            arrow2.DOFade(0,0.15f);
            arrow.DOFade(0,0.15f);
            foreach(Image mask in masks)
            {
                mask.DOFade(0,0.15f);
            }

            arrow.rectTransform.anchoredPosition = originPos;
            arrow2.rectTransform.anchoredPosition = originPos2;
        });
    }
    void ArrowMove()
    {
        tween1 = arrow.DOFade(1,0.5f);
        tween  = arrow.rectTransform.DOAnchorPos(originPos - arrow.rectTransform.right * 30, 0.5f).OnComplete(()=>
        {
            arrow.rectTransform.anchoredPosition = originPos;
            arrow.DOFade(0,0);
            ArrowMove();
        });
        tween2 = arrow2.DOFade(1,0.5f);
        tween3  = arrow2.rectTransform.DOAnchorPos(originPos2 - arrow2.rectTransform.right * 30, 0.5f).OnComplete(()=>
        {
            arrow2.rectTransform.anchoredPosition = originPos2;
            arrow2.DOFade(0,0);
        });

    }
}
