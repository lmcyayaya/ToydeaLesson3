using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
 

public class UISkillMove : MonoBehaviour
{
    public Vector2 targetPosition;
    RectTransform rect;

    void OnEnable()
    {
        rect.DOScale(Vector2.one*1.5f,1f).SetEase(Ease.OutQuart);
        rect.DOAnchorPos(new Vector2(0,400),1f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            rect.DOScale(Vector2.one,0.5f).SetEase(Ease.OutQuart).SetDelay(1);
            rect.DOAnchorPos(targetPosition,0.5f).SetEase(Ease.OutQuart).SetDelay(1);
        });
    }
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
}
