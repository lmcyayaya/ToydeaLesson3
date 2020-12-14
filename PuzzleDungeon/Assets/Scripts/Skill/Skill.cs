using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int value;
    public int cost;
    public Match3 game;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ForSkillButton()
    {
        if(PlayerData.Instance.currentSP < cost)
            return;
        PlayerData.Instance.currentSP -= cost;
        List<Node> spList =  game.getNodesOfValue(5);
        foreach(Node node in spList)
        {
            node.getPiece().value = value;
            node.SetPiece(node.getPiece());
            game.ForSkillChangePieceSprite(node.index,value);
            game.CopyBoard();
        }
    }
}
