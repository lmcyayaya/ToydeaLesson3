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
    [SerializeField]
    public List<AttackTrigger> playerInNow;
    public List<AttackTrigger> cutAttack;
    public List<AttackTrigger> laserAttack;
    public List<AttackTrigger> minesAttack;
    public List<AttackTrigger> midDistanceAttack;
    public List<AttackTrigger> creatWallAttack;
    public List<MapNodePiece> weakPoints;
    public int minesTimeCounter;
    public int wallTimeCounter;
    public bool open;
    public MapNodePiece currentWeakPoint;
    [SerializeField]
    Color mapPieceColor;
    List<AttackTrigger> needToAttack;
    bool minesExist;
    bool wallExist;
    
    void Start()
    {
        currentHP = maxHP;
        needToAttack = new List<AttackTrigger>();
    }
    public override void EnemyNeedToDO()
    {
        if(!open)
            return;
        minesTimeCounter += 1;
        wallTimeCounter += 1;
        if(needToAttack.Count != 0)
        {
            foreach(AttackTrigger attack in needToAttack)
            {
                Map.Instance.getNodeAtPoint(attack.index).getPiece().SetColor(Color.white);
            }
            
            switch(needToAttack[0].attackMode)
            {
                case AttackTrigger.AttackMode.CutAttack:
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
                    MinesAttack();
                    break;
                }
                case AttackTrigger.AttackMode.MidDistanceAttack:
                {
                    MidDistanceAttack();
                    break;
                }
                case AttackTrigger.AttackMode.CreatWall:
                {
                    CreatWall();
                    break;
                }
            }
            
        }
        else
        {
            ShowAttackRange();
            StateManager.Instance.state = StateManager.State.myTurn;
        }

        WallReset();
        MinesReset();
        currentWeakPoint = ChangeWeakPoint();
        
    }
    public override void Dead()
    {
        if(currentHP <= 0)
        {
            transform.gameObject.SetActive(false);
        }
    }
    void CutAttack()
    {
        bool minusHP = false;
        foreach(AttackTrigger a in playerInNow)
        {
            if(a.attackMode == AttackTrigger.AttackMode.CutAttack)
            {
                minusHP = true;
                atk = a.atk;
            }
        }
        sword.DOScale(new Vector3(0.3f,1,1),0.5f).SetEase(Ease.OutQuart).OnComplete(()=>
        {
            bossImage.DORotate(Vector3.forward * 360,0.6f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                if(minusHP)
                {
                    float tmp =  Mathf.Clamp(atk - ProcessedData.Instance.def,0,int.MaxValue);
                    PlayerData.Instance.currentHP -= tmp;
                }
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
        bool minusHP = false;
        foreach(AttackTrigger a in playerInNow)
        {
            if(a.attackMode == AttackTrigger.AttackMode.LaserAttack)
            {
                minusHP = true;
                atk = a.atk;
            }
        }
        foreach(Transform obj in lasers)
        {
            obj.DOScale(new Vector3(1,0.5f,1),0.5f).SetEase(Ease.OutQuart).OnComplete(()=>
            {
                obj.DOScale(new Vector3(3,4,1),0.5f).SetEase(Ease.InExpo).OnComplete(()=>
                {
                    if(minusHP)
                    {
                        minusHP = false;
                        float tmp =  Mathf.Clamp(atk - ProcessedData.Instance.def,0,int.MaxValue);
                        PlayerData.Instance.currentHP -= tmp;
                    }
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
        minesExist = true;
        minesTimeCounter = 0;
        foreach(AttackTrigger attack in minesAttack)
        {
            var tmp = attack.GetComponentInChildren<Mines>();
            tmp.minesOn = true;
            tmp.transform.DOScale(Vector3.one*0.75f,0.5f).SetEase(Ease.OutBack);
        }
        StartCoroutine(WaitToDo(0.5f,()=>
        {
            ShowAttackRange();
            StateManager.Instance.state = StateManager.State.myTurn;
        }));
    }
    void MidDistanceAttack()
    {
        float time = 0;
        foreach(AttackTrigger attack in midDistanceAttack)
        {
            attack.transform.DOScale(Vector3.one,0.2f).SetDelay(time).SetEase(Ease.OutQuad).OnComplete(()=>
            {
                attack.transform.DOScale(Vector3.zero,0.2f).SetEase(Ease.OutQuad);
                if(attack.playerInHere)
                {
                    float tmp =  Mathf.Clamp(attack.atk - ProcessedData.Instance.def,0,int.MaxValue);
                    PlayerData.Instance.currentHP -= tmp;
                }
                if(midDistanceAttack.IndexOf(attack) == midDistanceAttack.Count-1)
                {
                    ShowAttackRange();
                    StateManager.Instance.state = StateManager.State.myTurn;
                }
            });
            time += 0.02f;
        }
    }
    void CreatWall()
    {
        wallExist = true;
        wallTimeCounter = 0;
        foreach(AttackTrigger attack in creatWallAttack )
        {
            if(attack.playerInHere)
            {
                float tmp =  Mathf.Clamp(attack.atk - ProcessedData.Instance.def,0,int.MaxValue);
                PlayerData.Instance.currentHP -= tmp;
                Point[] directions =
                {
                    Point.up,
                    Point.right,
                    Point.down,
                    Point.left
                };
                foreach(Point dir in directions)
                {
                    Point next = Point.add(attack.index,dir);

                    if(next.x >= Map.Instance.width || next.y >= Map.Instance.height || next.x < 0 || next.y < 0) 
                        continue;
                    if(Map.Instance.getNodeAtPoint(next).value == 2 || Map.Instance.getNodeAtPoint(next).value == 1)
                        continue;
                    
                    Player.Instance.transform.DOMove(Map.Instance.getNodeAtPoint(next).getPiece().transform.position,0.2f);
                    Player.Instance.index = next;
                }
            }

            Map.Instance.SetNewValueToNord(attack.index,1);
            Map.Instance.getNodeAtPoint(attack.index).getPiece().SetColor(new Color(1,1,1,0));
            attack.transform.DOScale(Vector3.one,0.3f).SetEase(Ease.OutBack).OnComplete(()=>
            {
                if(creatWallAttack.IndexOf(attack) == creatWallAttack.Count-1)
                {
                    ShowAttackRange();
                    StateManager.Instance.state = StateManager.State.myTurn;
                }
            });
        }
    }
    void WallReset()
    {
        if(wallTimeCounter < 2 || !wallExist)
            return;
        wallExist= false;
        foreach(AttackTrigger attack in creatWallAttack )
        {
            attack.transform.DOScale(Vector3.zero,0.3f).SetEase(Ease.InBack);
            Map.Instance.SetNewValueToNord(attack.index,0);
            Map.Instance.getNodeAtPoint(attack.index).getPiece().BackToLastColor();
        }
    }
    void MinesReset()
    {
        if(minesTimeCounter < 3 || !minesExist)
            return;
        minesExist= false;
        foreach(AttackTrigger attack in minesAttack )
        {
            attack.GetComponentInChildren<Mines>().DisableMines();
        }
    }
    void ShowAttackRange()
    {
        needToAttack = SelectAttack();
        if(needToAttack.Count!=0)
            foreach(AttackTrigger attack in needToAttack)
            {
                Map.Instance.getNodeAtPoint(attack.index).getPiece().SetColor(Color.red);
            }
    }
    MapNodePiece ChangeWeakPoint()
    {
        if(currentWeakPoint == null)
        {
            weakPoints[0].SetColor(Color.yellow);
            return weakPoints[0];
        }
            
        int index = weakPoints.IndexOf(currentWeakPoint);
        index +=1;
        if(index >= weakPoints.Count)
            index = 0;
        weakPoints[index].SetColor(Color.yellow);
        currentWeakPoint.SetColor(mapPieceColor);
        return weakPoints[index];
    }
    List<AttackTrigger> SelectAttack()
    {

        AttackTrigger.AttackMode tmp = (AttackTrigger.AttackMode)UnityEngine.Random.Range(0,5);
        switch (tmp)
        {
            case AttackTrigger.AttackMode.CutAttack:
            {
                return cutAttack;
            }
            case AttackTrigger.AttackMode.LaserAttack:
            {
                return laserAttack;
            }
            case AttackTrigger.AttackMode.MinesAttack:
            {
                if(minesExist || minesTimeCounter < 4)
                    return SelectAttack();

                return minesAttack;
            }
            case AttackTrigger.AttackMode.MidDistanceAttack:
            {
                return midDistanceAttack;
            }
            case AttackTrigger.AttackMode.CreatWall:
            {
                if(wallExist || wallTimeCounter < 5)
                    return SelectAttack();
                return creatWallAttack;
            }
        
        }
        return null;
    }

    IEnumerator WaitToDo(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
