using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Step5 : Step
{
    public Image backGround;
    public Sprite sprite;
    public Image arrow;
    public Image mask;
    public Image mask2;
    public Image askWindow;
    Vector2 originPos;
    Tween tween;
    Tween tween1;
    public override void ShowUp()
    {
        mask.DOFade(0.78f,0.15f);
        mask2.DOFade(0.78f,0.15f);
        arrow.DOFade(1,0.15f);
        backGround.sprite = sprite;
        askWindow.rectTransform.DOScale(Vector2.one,0.3f);
        originPos = arrow.rectTransform.anchoredPosition;
        ArrowMove();
    }
    public override void Close()
    {   
        askWindow.rectTransform.DOScale(Vector2.zero,0.15f).OnComplete(()=>
        {
            arrow.DOFade(0,0.15f);
            tween.Pause();
            tween.Kill();
            tween1.Pause();
            tween1.Kill();
            mask.DOFade(0,0.15f);
            mask2.DOFade(0,0.15f);
            arrow.rectTransform.anchoredPosition = originPos;
        });
    }
    void ArrowMove()
    {
        tween1 = arrow.DOFade(1,0.5f);
        tween = arrow.rectTransform.DOAnchorPos(originPos + Vector2.down * 35, 0.5f).OnComplete(()=>
        {
            arrow.rectTransform.anchoredPosition = originPos;
            arrow.DOFade(0,0);
            ArrowMove();
        });

    }
    
}
