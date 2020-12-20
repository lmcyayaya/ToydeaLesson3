using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Step1 : Step
{
    public Image mask;
    public Image askWindow;

    public override void ShowUp()
    {
        mask.DOFade(0.78f,0.15f);
        askWindow.rectTransform.DOScale(Vector2.one,0.3f);
    }
    public override void Close()
    {   
        askWindow.rectTransform.DOScale(Vector2.zero,0.15f).OnComplete(()=>
        {
            mask.DOFade(0,0.15f);
        });
    }
}
