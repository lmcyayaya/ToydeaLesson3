using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public static Player Instance
    {
        get
        {
            return instance;
        }
    }
    static Player instance;
    public enum PlayerState
    {
        move,attack
    }
    public PlayerState playerState;
    public Point index;
    public UIState uiState;
    bool moving;
    bool attackReady;
    public bool hasAttacked;
    int actionState;
    List<Point> canMoveList;
    List<Point> moveList;
    List<List<Point>> moveListList; 
    List<Point> canAttackList;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        moveList = new List<Point>();
        canMoveList = new List<Point>();
        moveListList = new List<List<Point>>();
        canAttackList = new List<Point>();
    }

    public void ActionUpdate()
    {
        if(moving)
            return;
        //actionState : 0 is first in ,1 is first action,2  is second action ,this action complete will rest state and turn the state of statemanager to enemyTurn
        switch(actionState) 
        {
            case 0 :
            {

                if(ProcessedData.Instance.atkDropsCount > ProcessedData.Instance.moveDropsCount)
                
                    playerState = PlayerState.attack;
                
                    
                else if(ProcessedData.Instance.atkDropsCount < ProcessedData.Instance.moveDropsCount)
                
                    playerState = PlayerState.move;
           
                else//if movedrops equal attackdrops, Attack priority 
                
                    playerState = PlayerState.attack;
     
                    

                actionState +=1;
                break;
            }
            case 1 :
            {
                if(playerState == PlayerState.move)
                {
                    if(ProcessedData.Instance.move <= 0)
                    {
                        actionState += 1;
                        ChangeGridColor(canMoveList,Color.white);
                        moveListList.Clear();
                        canMoveList.Clear();
                        playerState = PlayerState.attack;
                        return;
                    }
                    StateMoveNeedToDo();
                }
                else
                {
                    if(ProcessedData.Instance.atkDropsCount <= 0)
                    {
                        playerState = PlayerState.move;
                        return;
                    }
                        
                    if(hasAttacked)
                    {
                        actionState += 1;
                        playerState = PlayerState.move;
                        ChangeGridColor(canAttackList,Color.white);
                        canAttackList.Clear();
                        attackReady = false;
                        hasAttacked = false;
                        return;
                    }

                    StateAttackNeedToDo();
                }
                break;
            }
            case 2 :
            {
                if(playerState == PlayerState.move)
                {
                    if(ProcessedData.Instance.move <= 0)
                    {
                        actionState = 0;
                        ChangeGridColor(canMoveList,Color.white);
                        moveListList.Clear();
                        canMoveList.Clear();
                        StateManager.Instance.state = StateManager.State.enemyTurn;
                        return;
                    }
                    StateMoveNeedToDo();
                }
                else
                {
                    if(ProcessedData.Instance.atkDropsCount <= 0)
                    {
                        actionState = 0;
                        StateManager.Instance.state = StateManager.State.enemyTurn;
                        return;
                    }
                        
                    if(hasAttacked)
                    {
                        actionState = 0;
                        StateManager.Instance.state = StateManager.State.enemyTurn;
                        ChangeGridColor(canAttackList,Color.white);
                        canAttackList.Clear();
                        attackReady = false;
                        hasAttacked = false;
                        return;
                    }
                    StateAttackNeedToDo();
                }
                break;
            }
        }
    }
    void StateMoveNeedToDo()
    {
        if(canMoveList.Count == 0)
        {
            UpdateIndex();
            canMoveList.Add(index);
            FindCanMovePoints(Mathf.Clamp(ProcessedData.Instance.move,0,5),index);
            canMoveList.Remove(index);
            ChangeGridColor(canMoveList,Color.red);
            uiState.ChangeState("移動選択");
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
                MapNodePiece piece =  hit.transform.GetComponent<MapNodePiece>();
                if(!Map.Instance.isContainPointInPointList(canMoveList,piece.index) || piece == null)
                    return;
                moveList.Clear();
                FindTheWay(Mathf.Clamp(ProcessedData.Instance.move,0,5),index,piece .index);
                FindTheLowestPath();
                Move(moveList,0);
                ProcessedData.Instance.move -= moveList.Count;
                ChangeGridColor(canMoveList,Color.white);
                moveListList.Clear();
                canMoveList.Clear();

            }
        }
    }
    void StateAttackNeedToDo()
    {
        if(!attackReady)
        {
            uiState.ChangeState("攻擊選択");
            UpdateIndex();
            Point[] directions =
            {
                Point.up,
                Point.right,
                Point.down,
                Point.left
            };
            foreach(Point dir in directions)
            {
                Point next = Point.add(index,dir);
                if(next.x >= Map.Instance.width || next.y >= Map.Instance.height || next.x < 0 || next.y < 0) 
                    continue;
                if(Map.Instance.getNodeAtPoint(next).value == 0 ||Map.Instance.getNodeAtPoint(next).value == 2 )
                    canAttackList.Add(next);
            }
            ChangeGridColor(canAttackList,Color.red);
            attackReady = true;
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                var hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
                MapNodePiece piece =  hit.transform.GetComponent<MapNodePiece>();
                if(piece == null)
                    return;
                if(!Map.Instance.isContainPointInPointList(canAttackList,piece.index))
                    return;


                piece.GetComponentInParent<Enemy>().currentHP -= ProcessedData.Instance.atk;

                hasAttacked = true;
            }

        }
        

        
    }
    void UpdateIndex()
    {
        RaycastHit2D hit =  Physics2D.Raycast(transform.position + Vector3.forward*0.01f,Vector3.forward);
        MapNodePiece piece = hit.transform.GetComponent<MapNodePiece>();
        index.x = piece.index.x;
        index.y = piece.index.y;
    }
    void FindCanMovePoints(int moveAmount,Point p)
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
            Point next = Point.add(p,dir);
            if(next.x >= Map.Instance.width || next.y >= Map.Instance.height || next.x < 0 || next.y < 0) 
                continue;
            
            if(Map.Instance.getNodeAtPoint(next).value != 0)
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
        var i = int.MaxValue;
        foreach(var list in moveListList)
        {
            if(list.Count < i)
            {
                i =list.Count;
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
            if(next.x >= Map.Instance.width || next.y >= Map.Instance.height || next.x < 0 || next.y < 0) 
                continue;
            if(Map.Instance.getNodeAtPoint(next).value != 0)
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
                
                FindTheWay(moveAmount - 1,next,endPoint);
                moveList.Remove(next);
            }
        }
    }
    void Move(List<Point> path,int i)
    {
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
        foreach(Point p in copy)
        {
            to.Add(p);
        }
    }
    void ChangeGridColor(List<Point> list,Color color)
    {
        foreach(Point p in list)
        {
            Map.Instance.getNodeAtPoint(p).getPiece().transform.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
