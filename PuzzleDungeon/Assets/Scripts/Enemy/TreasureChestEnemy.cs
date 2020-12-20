using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TreasureChestEnemy : Enemy
{
    public int detectDis;
    public int moveTimes;
    public int exp;
    public Sprite changeSprite;
    int hasMove;
    int pauseTimes;
    bool dead;
    bool hasAround;
    bool hasBeenDetected;
    Sprite originSprite;
    MapNodePiece piece;
    Color oriColor;
    SpriteRenderer sprite;
    MapNodePiece attackNode;
    Point oriPoint;
    List<Point> moveList;
    List<List<Point>> moveListList;
    List<Point> hasMoveList;
    void Start()
    {
        piece = GetComponentInChildren<MapNodePiece>();
        sprite = GetComponent<SpriteRenderer>();
        originSprite = sprite.sprite;
        moveList = new List<Point>();
        moveListList = new List<List<Point>>();
        hasMoveList = new List<Point>();
        oriColor = sprite.color;
        oriPoint = piece.index;
        currentHP = maxHP;
    }
    public override void Dead()
    {
        if(currentHP > 0)
            return;
        currentHP = 0;
        dead = true;
        var expText = ObjectPool.TakeFromPool("EXP");
        expText.position = transform.position;
        var ui = expText.GetComponent<UINumberPopUp>();
        ui.amount = exp;
        ui.ShowExp(transform.position);
        PlayerData.Instance.currentExp += exp;
        piece.value = 0;
        Map.Instance.getNodeAtPoint(piece.index).value = 0;
        sprite.color -= new Color(0,0,0,1);
        Player.Instance.DetectMap(Player.Instance.detectedMapDis,Player.Instance.index);
        CloseAttackPiece();
    }
    public override void EnemyNeedToDO()
    {
        if(dead)
            return;

        CloseAttackPiece();
        
        if(sprite.color.a == 1 && !hasBeenDetected)
        {
            hasBeenDetected = true;
            oriColor = new Color(oriColor.r,oriColor.g,oriColor.b,1);
        }
        hasAround = false;
        if(!DetectDistance())
            return;

        Point dir = CheckBeside();
        if(dir != null)
        {
            AttackPlayer();
            attackNode = Map.Instance.getNodeAtPoint(Player.Instance.index).getPiece();
            attackNode.transform.localPosition -= Vector3.forward * 1.1f;
            attackNode.SetColor(Color.red);


            return;
        }

        DetectPlayer(detectDis,piece.index);
        if(hasAround)
        {
            moveList.Clear();
            FindTheWay(detectDis,piece.index,Player.Instance.index);
            FindTheLowestPath();

            if(moveList.Count != 0)
                Move(moveList[0]);

            moveListList.Clear();
            sprite.sprite = changeSprite;
            hasMove +=1;
            if(hasMove < moveTimes)
            {
                StartCoroutine(WaitToDo(0.2f,()=> EnemyNeedToDO()));
            }
            else
            {
                hasMove = 0;
            }
                
        } 
        else
        {
            sprite.sprite = originSprite;
        }
        
        
    }
    void AttackPlayer()
    {
        var damageText = ObjectPool.TakeFromPool("Damage");
        var uiDamage = damageText.GetComponent<UINumberPopUp>();
        damageText.position = Player.Instance.transform.position;
        float damage = Mathf.Clamp((atk-ProcessedData.Instance.def),0,float.MaxValue);
        uiDamage.amount = damage;
        uiDamage.ShowDamage();

        PlayerData.Instance.currentHP -= damage;
    }
    void CloseAttackPiece()
    {
        if(attackNode != null)
        {
            attackNode.SetColor(Color.white);
            attackNode.transform.localPosition += Vector3.forward * 1.1f;
            attackNode = null;
        }
    }
    void DetectPlayer(int moveAmount,Point p)
    {
        if(p.x == Player.Instance.index.x && p.y == Player.Instance.index.y)
        {
            hasAround = true;
            return;
        }
        if(moveAmount == 0 || hasAround)
            return;

        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };
        foreach(Point dir in directions)
        {
            Point next = Point.add(p,dir);
            if(next.x >= Map.Instance.width || next.y >= Map.Instance.height || next.x < 0 || next.y < 0) 
                continue;
            
            if(Map.Instance.getNodeAtPoint(next).value == 1 )
            {
                
                continue;
            }
            else
            {
                DetectPlayer(moveAmount-1,next); 
            }   
        }
    }
    void Move(Point p)
    {
        MapNodePiece target = Map.Instance.getNodeAtPoint(p).getPiece();
        if(target.value == 2)
            return;




        piece.transform.parent = null;
        Map.Instance.SwitchNodePieceAndValue(piece.index,p);
        var tmp = target.transform.position;
        
        transform.DOMove(target.transform.position,0.2f).OnComplete(()=>
        {
            piece.transform.parent = transform;
            piece.transform.localPosition = Vector3.zero;

            if(CheckBeside() != null)
            {
                attackNode = Map.Instance.getNodeAtPoint(Player.Instance.index).getPiece();
                attackNode.transform.localPosition -= Vector3.forward*1.1f;
                attackNode.SetColor(Color.red);
            }
            
        });
        target.transform.position = piece.transform.position;
        piece.transform.position = tmp;
        
    }
    void MovePath(List<Point> pathList,int index)
    {
        MapNodePiece target = Map.Instance.getNodeAtPoint(pathList[index]).getPiece();
        if(target.value == 2)
        {
            pauseTimes += index;
            pauseTimes += 1;
            if(pauseTimes >= moveTimes)
            {
                pauseTimes = 0;
                return;
            }
            else
            {
                FindTheWay(detectDis,piece.index,Player.Instance.index);
                FindTheLowestPath();
                if(moveList.Count != 0)
                    MovePath(moveList,0);
            }
                
        }
        if(index >= moveTimes || pathList.IndexOf(pathList[index]) == pathList.Count-1)
        {
            if( CheckBeside() != null)
            {
                attackNode = Map.Instance.getNodeAtPoint(Player.Instance.index).getPiece();
                attackNode.transform.localPosition -= Vector3.forward;
                attackNode.SetColor(Color.red);
            }
            return;
        }
            

        
        piece.transform.parent = null;
        Map.Instance.SwitchNodePieceAndValue(piece.index,pathList[index]);
        var tmp = target.transform.position;
        
        transform.DOMove(target.transform.position,0.2f).OnComplete(()=>
        {
            piece.transform.parent = transform;
            piece.transform.localPosition = Vector3.zero;
            
            MovePath(pathList,index+1);
        });
        target.transform.position = piece.transform.position;
        piece.transform.position = tmp;
        
    }
    void FindTheWay(int detectDis,Point startPoint,Point endPoint)
    {
        if(detectDis == 0)
            return;
        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };
        foreach(Point dir in directions)
        {
            Point next = Point.add(startPoint,dir);
            if(next.x >= Map.Instance.width || next.y >= Map.Instance.height || next.x < 0 || next.y < 0) 
                continue;
            if(Map.Instance.getNodeAtPoint(next).value == 1)
            {
                continue;
            }
            else
            {
                moveList.Add(next);
                if(next.x == endPoint.x && next.y == endPoint.y)
                {
                    //moveList.Remove(next);
                    moveListList.Add(new List<Point>());
                    CopyList(moveList,moveListList[moveListList.Count-1]);
                }
                
                FindTheWay(detectDis - 1,next,endPoint);
                moveList.Remove(next);
            }
        }
    }
    Point CheckBeside()
    {
        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };
        foreach(Point dir in directions)
        {
            
            Point next = Point.add(piece.index,dir);
            if(next.x == Player.Instance.index.x && next.y == Player.Instance.index.y)
            {
                return dir;
            }
        }
        return null;
    }
    void FindTheLowestPath()    
    { 
        var i = int.MaxValue;
        var j = int.MaxValue;
        List<Point> tmp = null;
        foreach(var list in moveListList)
        {
            if(!ListContainsEnemy(list) && list.Count < j)
            {
                j = list.Count;
                tmp = list;
            }
            if(list.Count < i )
            {
                i =list.Count;
                moveList = list;
            }
        }
        if(tmp != null)
            moveList = tmp;
    }
    bool DetectDistance()
    {
        if(Mathf.Abs(Player.Instance.index.x - piece.index.x) + Mathf.Abs(Player.Instance.index.y - piece.index.y) > detectDis )
            return false;
        else
            return true;
    }
    void CopyList(List<Point> copy,List<Point> to)
    {
        foreach(Point p in copy)
        {
            to.Add(p);
        }
    }
    bool ListContainsEnemy(List<Point> points)
    {
        foreach(Point p in points)
        {
            if(Map.Instance.getNodeAtPoint(p).value == 2)
                return true;
        }
        return false;
    }
    IEnumerator WaitToDo(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
