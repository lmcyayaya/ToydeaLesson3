using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIMoveCount : MonoBehaviour
{
    Image background;
    Text moveCount;
    void Start()
    {
        background = transform.parent.GetComponent<Image>();
        moveCount = GetComponent<Text>();
    }


    void Update()
    {
        if(Player.Instance.playerState == Player.PlayerState.move && StateManager.Instance.state == StateManager.State.action && ProcessedData.Instance.move > 0)
        {
            background.color = Color.white;
            moveCount.color = Color.black;
            moveCount.text = "あと"+ ProcessedData.Instance.move.ToString() + "歩";
        }
        else
        {
            background.color = new Color(0,0,0,0);
            moveCount.color = new Color(0,0,0,0);
        }
            
        
    }
}
