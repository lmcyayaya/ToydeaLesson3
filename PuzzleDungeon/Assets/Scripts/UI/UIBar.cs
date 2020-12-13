using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIBar : MonoBehaviour
{
    public Image backGround;
    public Image back;
    public Image front;
    public GameObject effect;
    [HideInInspector]
    public float CurrentAmount
    {
        get
        {
            return currentAmount;
        }
        set
        {
            if(value!=currentAmount)
            {
                currentAmount = value;
                HPDoFillAmount();
                if(value <currentAmount)
                    if(effect!=null)
                        effect.SetActive(true);
            }
        }
    }
    protected float currentAmount;
    protected float maxAmount;
    
    void HPDoFillAmount()
    {
        front.DOFillAmount(CurrentAmount/maxAmount,0.4f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            back.DOFillAmount(CurrentAmount/maxAmount,0.3f).SetEase(Ease.OutQuart);
        });
    }
    protected void ShowUI()
    {
        if(StateManager.Instance.state != StateManager.State.action)
        {
            backGround.DOFade(0,0);
            back.DOFade(0,0);
            front.DOFade(0,0);
        }
        else
        {
            backGround.DOFade(1,0);
            back.DOFade(1,0);
            front.DOFade(1,0);
        }
    }
}
