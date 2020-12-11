using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISkip : MonoBehaviour
{
    Button button;
    void Start()
    {
        button = GetComponent<Button>();
    }


    void Update()
    {
        if(StateManager.Instance.state != StateManager.State.action)
        {
           transform.gameObject.SetActive(false);
        }
        
    }
    public void SkipButton()
    {
        if(Player.Instance.playerState == Player.PlayerState.attack)
        {
            Player.Instance.hasAttacked = true;
        }
        else if(Player.Instance.playerState == Player.PlayerState.move)
        {
            ProcessedData.Instance.move = 0;
        }
    }
}
