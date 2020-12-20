using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    public Image mask;
    public Text gameOver;
    public GameObject parent;
    public Image[] buttons;
    public Text[] buttonTexts;
    bool doOnce;
    Vector3 originPos;
    void Start()
    {
        originPos = gameOver.rectTransform.anchoredPosition;
    }   

    void Update()
    {
        if(PlayerData.Instance.currentHP <= 0)
        {
            if(!doOnce)
            {
                doOnce = true;
                ShowUp();
            }
        }
    }
    void ShowUp()
    {
        mask.raycastTarget = true;
        mask.DOFillAmount(1,0.3f).OnComplete(()=>
        {
            gameOver.DOFade(1,0.2f);
            gameOver.rectTransform.DOAnchorPos(new Vector2(0,138f),0.3f).SetEase(Ease.OutBounce).OnComplete(()=>
            {
                parent.SetActive(true);
                foreach(Image i in buttons)
                {
                    i.DOFade(1,0.3f);
                }
                foreach(Text t in buttonTexts)
                {
                    t.DOFade(1,0.3f);
                }
            });

        });
    }
    public void RePlay()
    {
        UIFade.Instance.FadeIn(0.5f,Ease.Linear,()=>
        {
            SceneManager.LoadScene("20MinVer");
        });
    }
    public void Quit()
    {
        Application.Quit();
    }
}
