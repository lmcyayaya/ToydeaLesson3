using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HasBeenBossRoom : MonoBehaviour
{
    public GameObject mist;
    public UIBossRoom bossRoom;
    public Point bossRoomStartIndex;
    public Point bossRoomEndIndex;
    public Point[] triggerBossRoom;
    public Sprite wall;
    public Color wallColor;
    bool hasBeenBossRoom = false;
    Boss boss;
    void Start()
    {
        boss = GetComponent<Boss>();
    }
    void Update()
    {
        if(hasBeenBossRoom)
            return;
        foreach(Point p in triggerBossRoom)
        {
            if(p.Equals(Player.Instance.index))
            {
                hasBeenBossRoom = true;
                bossRoom.OpenWindow();
                Player.Instance.stopMoving = true;
            }
        }
    }
    void ChangeToWall()
    {
        foreach(Point p in triggerBossRoom)
        {
            Map.Instance.getNodeAtPoint(p).value = 1;
            var piece = Map.Instance.getNodeAtPoint(p).getPiece();
            piece.value = 1;
            piece.sprite.sprite = wall;
            piece.lastColor = wallColor;
            piece.sprite.color = wallColor;
        }
    }
    public void ResetPlayer(bool check)
    {
        if(check)
            ProcessedData.Instance.move = 0;

        
    }
    public void MovePlayer(bool check)
    {
        Point p = Point.add(Player.Instance.index,Point.up);
        if(!check)
        {
            p = Point.add(Player.Instance.index,Point.up);
            
        }
        else
        {
            p = Point.add(Player.Instance.index,Point.down);
            ChangeToWall();
        }
        Player.Instance.index = p;
        
        var pos = Map.Instance.getNodeAtPoint(p).getPiece().transform.position;
        Player.Instance.transform.DOMove(pos,0.2f).OnComplete(()=>
        {
            hasBeenBossRoom = false;
            Player.Instance.stopMoving = false;
        });
        
    }
    public void ShowBossRoom()
    {
        mist.SetActive(false);
        boss.open = true;
        foreach(MapNodePiece piece in transform.GetComponentsInChildren<MapNodePiece>())
        {
            piece.sprite.color = piece.lastColor; 
        }
        for(int x = bossRoomStartIndex.x ; x < bossRoomEndIndex.x; x++)
        {
            for(int y = bossRoomStartIndex.y; y < bossRoomEndIndex.y; y++)
            {
                MapNodePiece piece = Map.Instance.getNodeAtPoint(new Point(x,y)).getPiece();
                if(piece.value == 0)
                    piece.sprite.color = Color.white;
                else
                    piece.sprite.color = piece.lastColor; 
            }
        }
    }
}
