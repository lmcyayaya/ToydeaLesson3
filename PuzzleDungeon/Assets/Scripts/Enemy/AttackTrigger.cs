using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public enum AttackMode
    {
        CutAttack,LaserAttack,MinesAttack,MidDistanceAttack,CreatWall
    }
    public AttackMode attackMode;
    public float atk;
    public Point index;
    public bool playerInHere;
    Boss boss;
    void Start() 
    {
        boss = GetComponentInParent<Boss>();
    }
    void Update()
    {
        if(Player.Instance.index.Equals(index))
        {
            playerInHere = true;
            if(!boss.playerInNow.Contains(this))
                boss.playerInNow.Add(this);
        }   
        else
        {
            playerInHere = false;
            if(boss.playerInNow.Contains(this))
                boss.playerInNow.Remove(this);
        }
            
        if(index.x == 0 && index.y == 0)
        {
            RaycastHit2D hit =  Physics2D.Raycast(transform.position + Vector3.forward*0.01f,Vector3.forward);
            MapNodePiece piece = hit.transform.GetComponent<MapNodePiece>();
            index.x = piece.index.x;
            index.y = piece.index.y;
        }
            
    }
}
