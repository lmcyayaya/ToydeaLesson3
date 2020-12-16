using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasBeenBossRoom : MonoBehaviour
{
    public GameObject mist;
    public Point[] triggerBossRoom;
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
                ShowBossRoom();
            }
        }
    }
    void ShowBossRoom()
    {
        mist.SetActive(false);
        boss.open = true;
        foreach(MapNodePiece piece in transform.GetComponentsInChildren<MapNodePiece>())
        {
            piece.sprite.color = piece.lastColor; 
        }
        for(int x = 11 ; x < 24; x++)
        {
            for(int y = 0; y < 14; y++)
            {
                MapNodePiece piece = Map.Instance.getNodeAtPoint(new Point(x,y)).getPiece();
                piece.sprite.color = piece.lastColor;
            }
        }
    }
}
