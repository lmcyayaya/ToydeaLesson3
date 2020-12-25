using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIPlayData : MonoBehaviour
{
    public Image mask;
    public Text level;
    public Text currentExp;
    public Text targetExp;
    public Image currentExpBar;
    public Text atk;
    public Text def;
    public Text move;
    public Text hp;
    public Text sp;
    public Text remainPoint;
    public  Text atkTotalPlus;
    public  Text defTotalPlus;
    public  Text moveTotalPlus;
    public  Text hpTotalPlus;
    public  Text spTotalPlus;
    int ori_remainPoint;
    int tmpRemainPoint;
    RectTransform rect;
    Vector2 oriPos;
    Tween tween;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        oriPos = rect.anchoredPosition;
        //CopyCurrentData();
    }

    void Update()
    {

    }
    void CopyCurrentData()
    {
        level.text = PlayerData.Instance.CurrentLevel.ToString();
        currentExp.text = PlayerData.Instance.currentExp.ToString();
        float tmp = 0;
        for(int j = 1; j <= PlayerData.Instance.CurrentLevel; j++)
        {
            tmp += (80 + ((j - 1) *40));
        }
        targetExp.text = tmp.ToString();
        float tmp2 = 0;
        for(int j = 1; j < PlayerData.Instance.CurrentLevel; j++)
        {
            tmp2 += (80 + ((j - 1) *40));
        }
        float a = (80 + ((PlayerData.Instance.CurrentLevel - 1) * 40));
        float i =(PlayerData.Instance.currentExp - tmp2) / a;
        currentExpBar.DOFillAmount(i,0.5f).SetEase(Ease.OutQuart);
        atk.text = PlayerData.Instance.ATK.ToString();
        def.text = PlayerData.Instance.DEF.ToString();
        move.text = PlayerData.Instance.MOVE.ToString();
        hp.text = PlayerData.Instance.HP.ToString();
        sp.text = PlayerData.Instance.SP.ToString();
        tmpRemainPoint = PlayerData.Instance.remainPoint;
        remainPoint.text = tmpRemainPoint.ToString();
        atkTotalPlus.text = "+0";
        defTotalPlus.text = "+0";
        moveTotalPlus.text = "+0";
        hpTotalPlus.text = "+0";
        spTotalPlus.text = "+0";
    }
    public void ResetData()
    {

    }
    public void OpenPage()
    {
        if(StateManager.Instance.state != StateManager.State.myTurn)
            return;
        if(tween!=null)
        {
            tween.Kill();
            mask.gameObject.SetActive(true);
            CopyCurrentData();
            tween = mask.DOFillAmount(1,0.5f).SetEase(Ease.OutQuart).OnComplete(()=>
            {

                tween = null;
            });
        }
        else
        {
            mask.gameObject.SetActive(true);
            CopyCurrentData();
            tween = mask.DOFillAmount(1,0.5f).SetEase(Ease.OutQuart).OnComplete(()=> 
            {
                tween = null;
            });
        }
        
    }
    public void ClosePage()
    {
        if(tween!=null)
        {
            tween.Kill();
            currentExpBar.DOFillAmount(0,0.2f);
            tween = mask.DOFillAmount(0,0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                tween = null;
                mask.gameObject.SetActive(false);
            });
        }
        else
        {
            tween = mask.DOFillAmount(0,0.2f).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                tween = null;
                currentExpBar.DOFillAmount(0,0.2f);
                mask.gameObject.SetActive(false);
            });
        }
        
    }
    public void ATKPlusButton()
    {   
        if(tmpRemainPoint < 1)
            return;
        tmpRemainPoint -= 1;
        remainPoint.text = tmpRemainPoint.ToString();
        int i = Convert.ToInt32(atk.text);
        i += 2;
        atk.text = i.ToString();
        atkTotalPlus.text = "+"+(i - PlayerData.Instance.ATK).ToString();
        
    }
    public void ATKMinusButton()
    {   
        int i = Convert.ToInt32(atk.text);
        i -= 2;
        if(i < PlayerData.Instance.ATK)
            return;
        tmpRemainPoint += 1;
        remainPoint.text = tmpRemainPoint.ToString();
        atk.text = i.ToString();
        atkTotalPlus.text = "+"+(i - PlayerData.Instance.ATK).ToString();
    }
    public void DEFPlusButton()
    {   
        if(tmpRemainPoint < 1)
            return;
        tmpRemainPoint -= 1;
        remainPoint.text = tmpRemainPoint.ToString();
        int i = Convert.ToInt32(def.text);
        i += 2;
        def.text = i.ToString();
        defTotalPlus.text = "+"+(i - PlayerData.Instance.DEF).ToString();
        
    }
    public void DEFMinusButton()
    {   
        int i = Convert.ToInt32(def.text);
        i -= 2;
        if(i < PlayerData.Instance.DEF)
            return;
        tmpRemainPoint += 1;
        remainPoint.text = tmpRemainPoint.ToString();
        def.text = i.ToString();
        defTotalPlus.text = "+"+(i - PlayerData.Instance.DEF).ToString();
    }
    public void MOVEPlusButton()
    {   
        if(tmpRemainPoint < 5 || Convert.ToInt32(move.text) >= 10)
            return;
        tmpRemainPoint -=5;
        remainPoint.text = tmpRemainPoint.ToString();
        int i = Convert.ToInt32(move.text);
        i += 1;
        move.text = i.ToString();
        moveTotalPlus.text = "+"+(i - PlayerData.Instance.MOVE).ToString();
        
    }
    public void MOVEMinusButton()
    {   
        int i = Convert.ToInt32(move.text);
        i -= 1;
        if(i < PlayerData.Instance.MOVE)
            return;
        tmpRemainPoint += 5;
        remainPoint.text = tmpRemainPoint.ToString();
        move.text = i.ToString();
        moveTotalPlus.text = "+"+(i - PlayerData.Instance.MOVE).ToString();
    }
    public void HPPlusButton()
    {   
        if(tmpRemainPoint < 1)
            return;
        tmpRemainPoint -= 1;
        remainPoint.text = tmpRemainPoint.ToString();
        int i = Convert.ToInt32(hp.text);
        i += 10;
        hp.text = i.ToString();
        hpTotalPlus.text = "+"+(i - PlayerData.Instance.HP).ToString();
        
    }
    public void HPMinusButton()
    {   
        int i = Convert.ToInt32(hp.text);
        i -= 10;
        if(i < PlayerData.Instance.HP)
            return;
        tmpRemainPoint += 1;
        remainPoint.text = tmpRemainPoint.ToString();
        hp.text = i.ToString();
        hpTotalPlus.text = "+"+(i - PlayerData.Instance.HP).ToString();
    }
    public void SPPlusButton()
    {   
        if(tmpRemainPoint < 1)
            return;
        tmpRemainPoint -= 1;
        remainPoint.text = tmpRemainPoint.ToString();
        int i = Convert.ToInt32(sp.text);
        i += 5;
        sp.text = i.ToString();
        spTotalPlus.text = "+"+(i - PlayerData.Instance.SP).ToString();
        
    }
    public void SPMinusButton()
    {   
        int i = Convert.ToInt32(sp.text);
        i -= 5;
        if(i < PlayerData.Instance.SP)
            return;
        tmpRemainPoint += 1;
        remainPoint.text = tmpRemainPoint.ToString();
        sp.text = i.ToString();
        spTotalPlus.text = "+"+(i - PlayerData.Instance.SP).ToString();
    }
    public void ResetPoints()
    {
        tmpRemainPoint = PlayerData.Instance.remainPoint;
        CopyCurrentData();
        atkTotalPlus.text = "+0";
        defTotalPlus.text = "+0";
        moveTotalPlus.text = "+0";
        hpTotalPlus.text = "+0";
        spTotalPlus.text = "+0";
    }
    public void Confirm()
    {
        PlayerData.Instance.remainPoint = tmpRemainPoint;
        PlayerData.Instance.ATK = Convert.ToInt32(atk.text);
        PlayerData.Instance.DEF = Convert.ToInt32(def.text);
        PlayerData.Instance.MOVE= Convert.ToInt32(move.text);
        PlayerData.Instance.HP  = Convert.ToInt32(hp.text);
        PlayerData.Instance.SP  = Convert.ToInt32(sp.text);
        atkTotalPlus.text = "+0";
        defTotalPlus.text = "+0";
        moveTotalPlus.text = "+0";
        hpTotalPlus.text = "+0";
        spTotalPlus.text = "+0";
    }
}
