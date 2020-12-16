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
                
                if(value < currentAmount)
                    HurtEffect();

                currentAmount = value;
                currentAmount = Mathf.Clamp(currentAmount,0,maxAmount);
                HPDoFillAmount();
                
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
    protected virtual void HurtEffect()
    {
        if(effect!=null)
            effect.SetActive(true);
    }
}
