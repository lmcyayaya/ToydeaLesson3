using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UISkip : MonoBehaviour
{
    Image image;
    Button button;
    Text text;
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        text = transform.GetComponentInChildren<Text>();
    }


    void Update()
    {
        if(StateManager.Instance.state != StateManager.State.action)
        {
            button.interactable = false;
            image.raycastTarget = false;
            image.color = new Color(0,0,0,0);
            text.color = new Color(0,0,0,0);
        }
        else if(StateManager.Instance.state == StateManager.State.action)
        {
            button.interactable = true;
            image.raycastTarget = true;
            image.color = Color.white;
            text.color = Color.black;
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
