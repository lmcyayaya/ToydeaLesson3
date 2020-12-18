using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILevelUp : MonoBehaviour
{
    Text text;
    RectTransform rect;
    Vector3 originPos;
    Tween tween;
    void Start()
    {
        text = GetComponent<Text>();
        rect = GetComponent<RectTransform>();
        originPos = rect.anchoredPosition;
    }

    public void ShowUp()
    {
        Debug.Log("123");
        rect.anchoredPosition = originPos;
        rect.DOAnchorPos(originPos + Vector3.up * 100,1);
        text.DOFade(1,0.5f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            text.DOFade(0,0.5f).SetEase(Ease.OutQuart);
        });

    }
}
