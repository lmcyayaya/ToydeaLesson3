using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBreathScale : MonoBehaviour
{
    public float duration;
    public float size;
    public Ease ease;
    RectTransform rect;
    void Start()
    {
        rect = transform.GetComponent<RectTransform>();
        ScaleUP();
    }
    void ScaleUP()
    {
        rect.DOScale(Vector3.one * size,duration).SetEase(ease).OnComplete(()=>
        {
            ScaleDown();
        });
    }
    void ScaleDown()
    {
        rect.DOScale(Vector3.one,duration).SetEase(ease).OnComplete(()=>
        {
            ScaleUP();
        });
    }
}
