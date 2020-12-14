using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICombo : MonoBehaviour
{
    public float Combo
    {
        get
        {
            return combo;
        }
        set
        {
            if(value!=combo)
            {
                if(value > combo)
                    ShowCombo(value+" Combo");

                combo = value;
            }
        }
    }
    protected float combo;
    Text text;
    Tween tween;
    void Start()
    {
        text = GetComponent<Text>();   
    }
    void Update()
    {
        Combo = ProcessedData.Instance.combo;
    }
    void ShowCombo(string comboText)
    {
        if(tween!=null)
        {
            tween.Kill();
            tween = null;
        }
        text.text = comboText;
        text.rectTransform.localScale = Vector3.one * 2;
        text.color = Color.white;  
        tween = text.rectTransform.DOScale(Vector3.one,0.2f).OnComplete(()=>
        {
            tween = null;
            tween = text.DOFade(0,2f);
        });
    }
}
