using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIState : MonoBehaviour
{
    [HideInInspector]
    public StateManager.State StateManagerState
    {
        get
        {
            return stateManagerState;
        }
        set
        {
            if(value!=stateManagerState)
            {
                stateManagerState = value;
                if(stateManagerState == StateManager.State.myTurn)
                    ChangeState("準備OK");
                else if(stateManagerState == StateManager.State.turning)
                    ChangeState("パズル中");
            }
        }
    }
    private StateManager.State stateManagerState;
    public Text[] texts;
    int currentIndex = 0;
    void Update()
    {
        StateManagerState = StateManager.Instance.state;
    }
    public void ChangeState(string stateName)
    {
        if(currentIndex == 0)
        {
            currentIndex = 1;
            texts[1].text = stateName;
            texts[0].rectTransform.DOAnchorPos(texts[0].rectTransform.anchoredPosition + Vector2.down * 60,0.2f).OnComplete(()=>
            {
                texts[0].rectTransform.anchoredPosition = texts[0].rectTransform.anchoredPosition + Vector2.up * 120;
            });
            texts[1].rectTransform.DOAnchorPos(texts[1].rectTransform.anchoredPosition + Vector2.down * 60,0.2f);
        }
        else if(currentIndex == 1)
        {
            currentIndex = 0;
            texts[0].text = stateName;
            texts[0].rectTransform.DOAnchorPos(texts[0].rectTransform.anchoredPosition + Vector2.down * 60,0.2f);
            texts[1].rectTransform.DOAnchorPos(texts[1].rectTransform.anchoredPosition + Vector2.down * 60,0.2f).OnComplete(()=>
            {
                texts[1].rectTransform.anchoredPosition = texts[1].rectTransform.anchoredPosition + Vector2.up * 120;
            });
        }
    }
}
