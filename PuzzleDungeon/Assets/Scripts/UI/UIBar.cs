using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIBar : MonoBehaviour
{
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
}
