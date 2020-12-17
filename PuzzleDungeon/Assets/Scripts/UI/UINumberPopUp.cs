using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UINumberPopUp : MonoBehaviour
{
    public float speed;
    public float amount;
    Text text;
    Canvas canvas;
    Rigidbody2D rb;
    
    void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.worldCamera = Camera.main;
        text = GetComponentInChildren<Text>();
        rb = GetComponent<Rigidbody2D>();
        
    }
    public void ShowDamage()
    {
        int i = Random.Range(0,2);
        if(i == 0)
            i = -1;
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(i,1) * speed,ForceMode2D.Impulse);
        text.text = amount.ToString();
        text.rectTransform.DOScale(Vector2.one,0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            text.DOFade(0,0.4f).SetEase(Ease.InQuart).OnComplete(()=>
            {
                text.DOFade(1,0);
                text.rectTransform.localScale = Vector2.zero;
                ObjectPool.ReturnToPool(transform.gameObject);
            });
        });
    }
    public void ShowHp()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(1,0) * speed,ForceMode2D.Impulse);
        text.text = "+"+amount;
        //transform.DOMove(Player.Instance.transform.position + Vector3.up * 0.32f,0.5f);
        text.rectTransform.DOScale(Vector2.one,0.2f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            text.DOFade(0,0.3f).OnComplete(()=>
            {
                text.DOFade(1,0);
                text.rectTransform.localScale = Vector2.zero;
                ObjectPool.ReturnToPool(transform.gameObject);
            });
        });
    }
    public void ShowSp()
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-1,0) * speed,ForceMode2D.Impulse);
        text.text = "+"+amount;
        //transform.DOMove(Player.Instance.transform.position + Vector3.up * 0.32f,0.5f);
        text.rectTransform.DOScale(Vector2.one,0.2f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            text.DOFade(0,0.3f).OnComplete(()=>
            {
                text.DOFade(1,0);
                text.rectTransform.localScale = Vector2.zero;
                ObjectPool.ReturnToPool(transform.gameObject);
            });
        });
    }
    public void ShowExp(Vector3 pos)
    {
        text.text = "EXP+"+amount;
        transform.DOMove(pos + Vector3.up * 0.32f,0.5f);
        text.rectTransform.DOScale(Vector2.one,0.3f).SetEase(Ease.OutBack).OnComplete(()=>
        {
            text.DOFade(0,1f).OnComplete(()=>
            {
                text.DOFade(1,0);
                text.rectTransform.localScale = Vector2.zero;
                ObjectPool.ReturnToPool(transform.gameObject);
            });
        });
    }
}
