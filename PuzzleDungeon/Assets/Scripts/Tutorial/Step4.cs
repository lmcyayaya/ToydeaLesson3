using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class Step4 : Step
{
    public Image backGround;
    public Sprite sprite1;
    public Sprite sprite2;
    public Image arrow;
    public Image mask;
    public Image askWindow;
    Vector2 originPos;
    Coroutine coroutine;
    Tween tween;
    Tween tween1;

    public override void ShowUp()
    {
        mask.DOFade(0.78f,0.15f);
        arrow.DOFade(1,0.15f);
        askWindow.rectTransform.DOScale(Vector2.one,0.3f);
        originPos = arrow.rectTransform.anchoredPosition;
        ArrowMove();
        coroutine = StartCoroutine(BackGroundAutoChange());
    }
    public override void Close()
    {   
        askWindow.rectTransform.DOScale(Vector2.zero,0.15f).OnComplete(()=>
        {
            StopCoroutine(coroutine);
            arrow.DOFade(0,0.15f);
            tween.Pause();
            tween.Kill();
            tween1.Pause();
            tween1.Kill();
            arrow.DOFade(0,0);
            mask.DOFade(0,0.15f);
            arrow.rectTransform.anchoredPosition = originPos;
        });
    }
    void ArrowMove()
    {
        tween1 = arrow.DOFade(1,0.5f);
        tween = arrow.rectTransform.DOAnchorPos(originPos + Vector2.left * 50, 0.5f).OnComplete(()=>
        {
            arrow.rectTransform.anchoredPosition = originPos;
            arrow.DOFade(0,0);
            ArrowMove();
        });

    }
    IEnumerator BackGroundAutoChange()
    {
        backGround.sprite = sprite1;
        yield return new WaitForSeconds(2);
        backGround.sprite = sprite2;
        yield return new WaitForSeconds(2);
        coroutine = StartCoroutine(BackGroundAutoChange());
    }

}
