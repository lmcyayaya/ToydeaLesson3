using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss : Enemy
{
    public Transform bossImage;
    public Transform sword;
    public Transform[] lasers;
    public bool open;
    MapNodePiece[] pieces;
    AttackTrigger[] allAttackTriggers;
    List<AttackTrigger> needToAttack;
    void Start()
    {
        currentHP = maxHP;
        pieces = GetComponentsInChildren<MapNodePiece>();
        allAttackTriggers = GetComponentsInChildren<AttackTrigger>();
        needToAttack = new List<AttackTrigger>();
    }
    public override void EnemyNeedToDO()
    {
        if(!open)
            return;
        if(needToAttack.Count != 0)
        {
            foreach(AttackTrigger attack in needToAttack)
            {
                
                if(attack.playerInhere)
                {
                    float atk =  Mathf.Clamp(attack.atk-ProcessedData.Instance.def,0,int.MaxValue);
                    PlayerData.Instance.currentHP -= atk;
                }
                Map.Instance.getNodeAtPoint(attack.index).getPiece().GetComponent<SpriteRenderer>().color = Color.white;
            }
            switch(needToAttack[0].attackMode)
            {
                case AttackTrigger.AttackMode.ShortDistanceAttack:
                {
                    CutAttack();
                    break;
                }
                case AttackTrigger.AttackMode.LaserAttack:
                {
                    LaserAttack();
                    break;
                }
                case AttackTrigger.AttackMode.MinesAttack:
                {
                    ShowAttackRange();
                    StateManager.Instance.state = StateManager.State.myTurn;
                    break;
                }
            }
            
        }
        else
        {
            ShowAttackRange();
            StateManager.Instance.state = StateManager.State.myTurn;
        }
        
    }
    void CutAttack()
    {
        sword.DOScale(new Vector3(0.3f,1,1),0.5f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            bossImage.DORotate(Vector3.forward * 360,0.6f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                sword.DOScale(new Vector3(0.3f,0,1),0.5f).OnComplete(()=>
                {
                    ShowAttackRange();
                    StateManager.Instance.state = StateManager.State.myTurn;
                });
            });
        });
    }
    void LaserAttack()
    {
        bool onceBool = false;
        foreach(Transform obj in lasers)
        {
            obj.DOScale(new Vector3(1,0.5f,1),0.5f).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                obj.DOScale(new Vector3(3,4,1),0.5f).SetEase(Ease.InExpo).OnComplete(()=>
                {
                    
                    obj.DOScale(new Vector3(0,4,1),0.5f).SetDelay(1f).SetEase(Ease.OutQuart).OnComplete(()=>
                    {
                        StateManager.Instance.state = StateManager.State.myTurn;
                        obj.transform.localScale = new Vector3(0,0,1);
                        if(!onceBool)
                        {
                            onceBool = true;
                            ShowAttackRange();
                        }
                    });
                });
            });
        }
    }
    void MinesAttack()
    {

    }
    void ShowAttackRange()
    {
        needToAttack = SelectAttack();
        foreach(AttackTrigger attack in needToAttack)
        {
            Map.Instance.getNodeAtPoint(attack.index).getPiece().GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    List<AttackTrigger> SelectAttack()
    {
        List<AttackTrigger> attackList = new List<AttackTrigger>();
        foreach(AttackTrigger attackTrigger in allAttackTriggers)
        {
            if(attackTrigger.playerInhere)
                attackList.Add(attackTrigger);
        }
        AttackTrigger selectedAttack = null;
        if(attackList.Count != 0)
        {
            selectedAttack = attackList[UnityEngine.Random.Range(0,attackList.Count)];
        }

        List<AttackTrigger> triggers = new List<AttackTrigger>();

        if(selectedAttack!=null)
            foreach(AttackTrigger attackTrigger in allAttackTriggers)
            {
                if(attackTrigger.attackMode == selectedAttack.attackMode)
                    triggers.Add(attackTrigger);
            }
        else
        {
            AttackTrigger.AttackMode tmp = (AttackTrigger.AttackMode)UnityEngine.Random.Range(0,3);
            foreach(AttackTrigger attackTrigger in allAttackTriggers)
            {
                if(attackTrigger.attackMode == tmp)
                    triggers.Add(attackTrigger);
            }
        }
        return triggers;

    }
    int DetectPlayerDistance()
    {
        int i = int.MaxValue;
        foreach(MapNodePiece piece in pieces)
        {
            int dis = PointToPointDistance(piece.index,Player.Instance.index);
            if(dis < i)
            {
                i = dis;
            }
        }
        return i;
    }
    int PointToPointDistance(Point a,Point b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
    IEnumerator WaitToDo(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
