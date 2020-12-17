﻿using System;
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
        targetExp.text = (PlayerData.Instance.CurrentLevel * 80).ToString();
        float a = PlayerData.Instance.currentExp - (PlayerData.Instance.CurrentLevel *80);
        float i = a / 80;
        currentExpBar.DOFillAmount( i,0.5f).SetEase(Ease.OutQuart);
        atk.text = PlayerData.Instance.ATK.ToString();
        def.text = PlayerData.Instance.DEF.ToString();
        move.text = PlayerData.Instance.MOVE.ToString();
        hp.text = PlayerData.Instance.HP.ToString();
        sp.text = PlayerData.Instance.SP.ToString();
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        ori_remainPoint = PlayerData.Instance.remainPoint;
        atkTotalPlus.text = "+0";
        defTotalPlus.text = "+0";
        moveTotalPlus.text = "+0";
        hpTotalPlus.text = "+0";
        spTotalPlus.text = "+0";
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
        if(PlayerData.Instance.remainPoint < 1)
            return;
        PlayerData.Instance.remainPoint -=1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        int i = Convert.ToInt32(atk.text);
        i += 1;
        atk.text = i.ToString();
        atkTotalPlus.text = "+"+(i - PlayerData.Instance.ATK).ToString();
        
    }
    public void ATKMinusButton()
    {   
        int i = Convert.ToInt32(atk.text);
        i -= 1;
        if(i < PlayerData.Instance.ATK)
            return;
        PlayerData.Instance.remainPoint += 1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        atk.text = i.ToString();
        atkTotalPlus.text = "+"+(i - PlayerData.Instance.ATK).ToString();
    }
    public void DEFPlusButton()
    {   
        if(PlayerData.Instance.remainPoint < 1)
            return;
        PlayerData.Instance.remainPoint -=1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        int i = Convert.ToInt32(def.text);
        i += 1;
        def.text = i.ToString();
        defTotalPlus.text = "+"+(i - PlayerData.Instance.DEF).ToString();
        
    }
    public void DEFMinusButton()
    {   
        int i = Convert.ToInt32(def.text);
        i -= 1;
        if(i < PlayerData.Instance.DEF)
            return;
        PlayerData.Instance.remainPoint += 1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        def.text = i.ToString();
        defTotalPlus.text = "+"+(i - PlayerData.Instance.DEF).ToString();
    }
    public void MOVEPlusButton()
    {   
        if(PlayerData.Instance.remainPoint < 5 || Convert.ToInt32(move.text) >= 10)
            return;
        PlayerData.Instance.remainPoint -=5;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
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
        PlayerData.Instance.remainPoint += 5;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        move.text = i.ToString();
        moveTotalPlus.text = "+"+(i - PlayerData.Instance.MOVE).ToString();
    }
    public void HPPlusButton()
    {   
        if(PlayerData.Instance.remainPoint < 1)
            return;
        PlayerData.Instance.remainPoint -=1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
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
        PlayerData.Instance.remainPoint += 1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        hp.text = i.ToString();
        hpTotalPlus.text = "+"+(i - PlayerData.Instance.HP).ToString();
    }
    public void SPPlusButton()
    {   
        if(PlayerData.Instance.remainPoint < 1)
            return;
        PlayerData.Instance.remainPoint -=1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
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
        PlayerData.Instance.remainPoint += 1;
        remainPoint.text = PlayerData.Instance.remainPoint.ToString();
        sp.text = i.ToString();
        spTotalPlus.text = "+"+(i - PlayerData.Instance.SP).ToString();
    }
    public void ResetPoints()
    {
        PlayerData.Instance.remainPoint = ori_remainPoint;
        CopyCurrentData();
        atkTotalPlus.text = "+0";
        defTotalPlus.text = "+0";
        moveTotalPlus.text = "+0";
        hpTotalPlus.text = "+0";
        spTotalPlus.text = "+0";
    }
    public void Confirm()
    {
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
