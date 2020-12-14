using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NormalEnemy : Enemy
{
    public int detectDis;
    bool hasAround;
    MapNodePiece piece;
    Color oriColor;
    SpriteRenderer sprite;
    SpriteRenderer attackNode;
    Point oriPoint;
    List<Point> moveList;
    List<List<Point>> moveListList;
    List<Point> hasMoveList;
    void Start()
    {
        piece = GetComponentInChildren<MapNodePiece>();
        sprite = GetComponent<SpriteRenderer>();
        moveList = new List<Point>();
        moveListList = new List<List<Point>>();
        hasMoveList = new List<Point>();
        oriColor = sprite.color;
        oriPoint = piece.index;
        currentHP = maxHP;
    }
    public override void EnemyNeedToDO()
    {
        if(attackNode != null)
        {
            attackNode.color = Color.white;
            attackNode = null;
        }
        hasAround = false;
        Point dir = CheckBeside();
        if(dir != null)
        {
            AttackPlayer();
            attackNode = Map.Instance.getNodeAtPoint(Player.Instance.index).getPiece().GetComponent<SpriteRenderer>();
            attackNode.color = Color.red;
            if(dir.Equals(Point.up))
                transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            else if(dir.Equals(Point.down))
                transform.rotation = Quaternion.Euler(Vector3.zero);
            else if(dir.Equals(Point.right))
                transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            else if(dir.Equals(Point.left))
                transform.rotation = Quaternion.Euler(Vector3.forward * 90);

                return;
        }

        DetectPlayer(detectDis,piece.index);
        if(hasAround)
        {
            Debug.Log("Detected");
            moveList.Clear();
            FindTheWay(detectDis,piece.index,Player.Instance.index);
            FindTheLowestPath();
            if(moveList.Count !=0)
                Move(moveList[0]);
            moveListList.Clear();
            sprite.color = Color.red;
            
        } 
        else
        {
            sprite.color = oriColor;
        }
        
        
    }
    void AttackPlayer()
    {
        sprite.color = Color.red;
        PlayerData.Instance.currentHP -= (atk-ProcessedData.Instance.def);
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
        
        if(Mathf.Abs(p.x - piece.index.x) == 0)
        {
            if(p.y - piece.index.y > 0)
                transform.rotation = transform.rotation = Quaternion.Euler(Vector3.forward * 180);
            else
                transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            if(p.x - piece.index.x > 0)
                transform.rotation = Quaternion.Euler(Vector3.forward * -90);
            else
                transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        }

        
        piece.transform.parent = null;
        Map.Instance.SwitchNodePieceAndValue(piece.index,p);
        transform.DOMove(target.transform.position,0.2f).OnComplete(()=>
        {
            
            piece.transform.localPosition = Vector3.zero;
            //hasMoveList.Add(p);

            if(CheckBeside() != null)
            {
                Debug.Log("In");

                attackNode = Map.Instance.getNodeAtPoint(Player.Instance.index).getPiece().GetComponent<SpriteRenderer>();
                attackNode.color = Color.red;
            }
            
        });
        target.transform.position = piece.transform.position;
        piece.transform.parent = transform;
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
}
