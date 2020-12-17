using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIGameBoardMask : MonoBehaviour
{
    public Image mask;
    bool changeState = true;
    
    void Start()
    {
        
    }


    void Update()
    {
        if(StateManager.Instance.state == StateManager.State.action && !changeState || StateManager.Instance.state == StateManager.State.enemyTurn && !changeState)
        {
            changeState = true;
            mask.DOFade(0.7f,0.2f).SetEase(Ease.OutQuart);
        } 
        else if(changeState && StateManager.Instance.state == StateManager.State.myTurn)
        {
            changeState = false;
            mask.DOFade(0,0.2f).SetEase(Ease.OutQuart);
        }
    }
}
