using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIAtkDef : MonoBehaviour
{
    public bool atk;
    public float  Amount
    {
        get
        {
            return amount;
        }
        set
        {
            if(value != amount)
            {
                if(value > amount)
                {
                    
                    ChangeAmount();
                }
                amount = value;
                text.text = amount.ToString();
            }


        }

    }
    private float amount;
    Text text;
    Tween tween;

    void Start()
    {
        text = GetComponentInChildren<Text>();
    }

    void Update()
    {
        if(atk)
            Amount = ProcessedData.Instance.atk;
        else
            Amount = ProcessedData.Instance.def;
    }
    void ChangeAmount()
    {
        if(tween != null)
        {
            tween.Kill();
            text.rectTransform.localScale = Vector2.one;
            tween = text.rectTransform.DOScale(Vector2.one * 1.2f ,0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                tween = text.rectTransform.DOScale(Vector2.one,0.2f).SetEase(Ease.OutQuart);
            });
        }
        else
        {
            tween = text.rectTransform.DOScale(Vector2.one * 1.2f ,0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                tween = text.rectTransform.DOScale(Vector2.one,0.2f).SetEase(Ease.OutQuart);
            });
        }
        
    }
}
