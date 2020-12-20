using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class UIVictory : MonoBehaviour
{
    public Boss boss;
    public Image mask;
    public Text victory;
    public Text thank;
    public GameObject parent;
    public Image[] buttons;
    public Text[] buttonTexts;
    bool doOnce;
    
    void Update()
    {
        if(boss.currentHP <= 0)
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
            victory.rectTransform.DOScale(Vector3.one,0.4f).SetEase(Ease.OutBack).OnComplete(()=>
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
            thank.rectTransform.DOScale(Vector3.one,0.4f).SetDelay(0.2f).SetEase(Ease.OutBack);
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
