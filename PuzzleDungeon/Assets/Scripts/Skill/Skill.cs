using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        if(PlayerData.Instance.currentSP < cost || !game.isCurrentBoardContainDrops(5) || StateManager.Instance.state !=StateManager.State.myTurn)
            return;
        PlayerData.Instance.currentSP -= cost;
        List<Node> spList =  game.getNodesOfValue(5);
        foreach(Node node in spList)
        {
            NodePiece p =node.getPiece();
            RectTransform rect = p.GetComponent<RectTransform>();
            p.value = value;
            node.SetPiece(node.getPiece());
            game.ForSkillChangePieceSprite(node.index,value);
            game.CopyBoard();
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one,0.3f).SetEase(Ease.OutBack);
        }
    }
}
