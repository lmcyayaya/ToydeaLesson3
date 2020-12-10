using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public Point index;
    bool moving;
    bool hasFound;
    bool test;
    int moveX;
    int moveY;
    List<Point> canMoveList;
    List<Point> moveList;
    List<List<Point>> moveListList; 
    void Start()
    {
        moveList = new List<Point>();
        canMoveList = new List<Point>();
        moveListList = new List<List<Point>>();
    }

    void Update()
    {
        if(moving)
            return;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UpdateIndex();
            canMoveList.Add(index);
            FindCanMovePoints(4,index);
            foreach(Point p in canMoveList)
            {
                Map.Instance.getNodeAtPoint(p).getPiece().transform.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            //UpdateIndex();
            moveList.Clear();
            var hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
            hit.transform.GetComponent<MapNodePiece>();
            FindTheWay(4,index,hit.transform.GetComponent<MapNodePiece>().index);
            FindTheLowestPath();
            Move(moveList,0);

            // UpdateIndex();
            // var hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
            // MapNodePiece piece = hit.transform.GetComponent<MapNodePiece>();
            // if(piece!=null)
            // {
            //     StartCoroutine(Move(piece.index.x - index.x,piece.index.y - index.y));
            //     moving = true;
            // }
        }
    }
    void UpdateIndex()
    {
        RaycastHit2D hit =  Physics2D.Raycast(transform.position,Vector3.forward);
        MapNodePiece piece = hit.transform.GetComponent<MapNodePiece>();
        index.x = piece.index.x;
        index.y = piece.index.y;
    }
    void FindCanMovePoints(int moveAmount,Point p)
    {
        test = true;
        if(moveAmount == 0)
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
            if(Map.Instance.getNodeAtPoint(next).value == 1)
            {
                continue;
            }
            else
            {
                if(Map.Instance.isContainPointInPointList(canMoveList,next))
                {
                    FindCanMovePoints(moveAmount-1,next);
                }
                else
                {
                    canMoveList.Add(next);
                    FindCanMovePoints(moveAmount-1,next);
                }
            }
                
        }
        
    }
    void FindTheLowestPath()    
    { 
        int i = moveListList[0].Count;
        moveList = moveListList[0];
        foreach(List<Point> list in moveListList)
        {
            if(list.Count < i)
            {
                moveList = list;
            }
                
        }
    }
    void FindTheWay(int moveAmount,Point startPoint,Point endPoint)
    {
        if(moveAmount == 0)
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
            if(Map.Instance.getNodeAtPoint(next).value == 1)
            {
                continue;
            }
            else
            {
                moveList.Add(next);
                if(next.x == endPoint.x && next.y == endPoint.y)
                {
                    //hasFound = true;
                    moveListList.Add(new List<Point>());
                    CopyList(moveList,moveListList[moveListList.Count-1]);

                    
                }
                
                FindTheWay(moveAmount - 1,next,endPoint);
                moveList.Remove(next);
            }
        }
    }
    void Move(List<Point> path,int i)
    {
        Debug.Log(path.Count);
        moving = true;
        if(i > 0)
        {
            if(Mathf.Abs(path[i].x - path[i-1].x) == 0)
            {
                if(path[i].y - path[i - 1].y > 0)
                    transform.rotation = transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                else
                    transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                if(path[i].x - path[i - 1].x > 0)
                    transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                else
                    transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            }
                
        }
        else
        {
            if(Mathf.Abs(path[i].x - index.x) == 0)
            {
                if(path[i].y - index.y > 0)
                    transform.rotation = transform.rotation = Quaternion.Euler(Vector3.forward * 180);
                else
                    transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                if(path[i].x - index.x > 0)
                    transform.rotation = Quaternion.Euler(Vector3.forward * -90);
                else
                    transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            }
        }
        transform.DOMove(Map.Instance.getNodeAtPoint(path[i]).getPiece().transform.position,0.2f).OnComplete(()=>
        {
            index = path[i];
            
            if( i+1 < path.Count)
               Move(path,i+1);
            else
                moving = false;
        });
    }
    void CopyList(List<Point> copy,List<Point> to)
    {
        Debug.Log(copy.Count);
        foreach(Point p in copy)
        {
            to.Add(p);
        }
    }
}
