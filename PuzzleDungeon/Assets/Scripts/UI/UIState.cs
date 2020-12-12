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
                else if(stateManagerState == StateManager.State.enemyTurn)
                    ChangeState("エネミー");
            }
        }
    }
    private StateManager.State stateManagerState;
    public Text[] texts;
    int currentIndex = 0;
    List<Tween> tween0;
    List<Tween> tween1;
    void Start() 
    {
        tween0 = new List<Tween>();
        tween1 = new List<Tween>();   
    }
    void Update()
    {
        StateManagerState = StateManager.Instance.state;
    }
    public void ChangeState(string stateName)
    {
        if(currentIndex == 0)
        {
            if(tween0.Count != 0 && tween1.Count != 0)
            {
                currentIndex = 1;
                texts[1].text = stateName;
                Tween tmp0 = null;
                tmp0 = texts[0].rectTransform.DOAnchorPos(Vector2.down * 60,0.2f).OnComplete(()=>
                {
                    texts[0].rectTransform.anchoredPosition =Vector2.up * 120;
                    if(tween0.Count - 1 > tween0.IndexOf(tmp0))
                    {
                        tween0[tween0.IndexOf(tmp0)+1].Play();
                    }
                    tween0.Remove(tmp0);
                });
                tmp0.Pause();
                tween0.Add(tmp0);
                Tween tmp1 = null;
                tmp1 = texts[1].rectTransform.DOAnchorPos(Vector2.zero,0.2f).OnComplete(()=>
                {
                    if(tween1.Count - 1 > tween1.IndexOf(tmp1))
                    {
                        tween1[tween1.IndexOf(tmp1)+1].Play();
                    }
                   tween1.Remove(tmp1);
                });
                tmp1.Pause();
                tween1.Add(tmp1);
            }
            else
            {
                currentIndex = 1;
                texts[1].text = stateName;
                Tween tmp0 = null;
                tmp0 = texts[0].rectTransform.DOAnchorPos(Vector2.down * 60,0.2f).OnComplete(()=>
                {
                    texts[0].rectTransform.anchoredPosition =Vector2.up * 120;
                    if(tween0.Count - 1 > tween0.IndexOf(tmp0))
                    {
                        tween0[tween0.IndexOf(tmp0)+1].Play();
                    }
                    tween0.Remove(tmp0);
                });
                tween0.Add(tmp0);   
                Tween tmp1 = null;
                tmp1 = texts[1].rectTransform.DOAnchorPos(Vector2.zero,0.2f).OnComplete(()=>
                {
                    if(tween1.Count - 1 > tween1.IndexOf(tmp1))
                    {
                        tween1[tween1.IndexOf(tmp1)+1].Play();
                    }
                    tween1.Remove(tmp1);
                });
                tween1.Add(tmp1);
            }
        }
        else if(currentIndex == 1)
        {
            if(tween0.Count != 0 && tween1.Count != 0)
            {
                currentIndex = 0;
                texts[0].text = stateName;
                Tween tmp0 = null;
                tmp0 = texts[0].rectTransform.DOAnchorPos(Vector2.zero,0.2f).OnComplete(()=>
                {
                    if(tween0.Count - 1 > tween0.IndexOf(tmp0))
                    {
                        tween0[tween0.IndexOf(tmp0)+1].Play();
                    }
                    tween0.Remove(tmp0);
                });
                tmp0.Pause();
                tween0.Add(tmp0);
                Tween tmp1 = null;
                tmp1 = texts[1].rectTransform.DOAnchorPos(Vector2.down * 60,0.2f).OnComplete(()=>
                {
                    texts[1].rectTransform.anchoredPosition =Vector2.up * 120;
                    if(tween1.Count - 1 > tween1.IndexOf(tmp1))
                    {
                        tween1[tween1.IndexOf(tmp1)+1].Play();
                    }
                   tween1.Remove(tmp1);
                });
                tmp1.Pause();
                tween1.Add(tmp1);
            }
            else
            {
                currentIndex = 0;
                texts[0].text = stateName;
                Tween tmp0 = null;
                tmp0 = texts[0].rectTransform.DOAnchorPos(Vector2.zero,0.2f).OnComplete(()=>
                {
                    if(tween0.Count - 1 > tween0.IndexOf(tmp0))
                    {
                        tween0[tween0.IndexOf(tmp0)+1].Play();
                    }
                    tween0.Remove(tmp0);
                });
                tween0.Add(tmp0);   
                Tween tmp1 = null;
                tmp1 = texts[1].rectTransform.DOAnchorPos(Vector2.down * 60,0.2f).OnComplete(()=>
                {
                    texts[1].rectTransform.anchoredPosition =Vector2.up * 120;
                    if(tween1.Count - 1 > tween1.IndexOf(tmp1))
                    {
                        tween1[tween1.IndexOf(tmp1)+1].Play();
                    }
                    tween1.Remove(tmp1);
                });
                tween1.Add(tmp1);
            }
        }
    }
}
