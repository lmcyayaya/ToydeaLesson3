using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Step0 : Step
{
    public GameObject backGround;
    public GameObject canvas;
    public Image mask;
    public Image askWindow;

    public override void ShowUp()
    {
        mask.DOFade(0.78f,0.15f);
        askWindow.rectTransform.DOScale(Vector2.one,0.3f);
    }
    public void SkipTutorial()
    {
        //TutorialManager.Instance.step = TutorialManager.Instance.steps.Length-1;

        mask.DOFade(1,0.15f);
        askWindow.rectTransform.DOScale(Vector2.zero,0.15f).OnComplete(()=>
        {
            backGround.SetActive(false);
            mask.DOFade(0,0.15f).OnComplete(()=>
            {
                canvas.SetActive(false);
            });
        });
        
    }
    public override void Close()
    {   
        askWindow.rectTransform.DOScale(Vector2.zero,0.15f).OnComplete(()=>
        {
            mask.DOFade(0,0.15f);
        });
        
    }
}

