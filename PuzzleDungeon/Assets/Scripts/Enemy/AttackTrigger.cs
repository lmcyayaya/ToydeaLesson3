using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public enum AttackMode
    {
        ShortDistanceAttack,LaserAttack,MinesAttack
    }
    public AttackMode attackMode;
    public float atk;
    public Point index;
    public bool playerInhere;
    public bool minesOn;
    void Update()
    {
        if(Player.Instance.transform.position == transform.position)
        {
            if(minesOn)
                PlayerData.Instance.currentHP -= atk;
            playerInhere = true;
        }
            
        else
            playerInhere = false;
        if(index.x == 0 && index.y == 0)
        {
            RaycastHit2D hit =  Physics2D.Raycast(transform.position + Vector3.forward*0.01f,Vector3.forward);
            MapNodePiece piece = hit.transform.GetComponent<MapNodePiece>();
            index.x = piece.index.x;
            index.y = piece.index.y;
        }
            
    }
}
