using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIHint : MonoBehaviour
{
    Text text;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(StateManager.Instance.state != StateManager.State.action)
        {
            text.color = new Color(1,1,1,0);
            return;
        }
        else
        {
            if(Player.Instance.actionState == 1)
            {
                if(Player.Instance.playerState == Player.PlayerState.attack)
                {
                    if(ProcessedData.Instance.moveDropsCount >= 3)
                    {
                        text.text = "攻撃後も移動可能";
                        text.color = new Color(1,1,1,1);
                    }
                }
                else
                {
                    if(ProcessedData.Instance.atkDropsCount >= 3)
                    {
                        text.text = "移動後も攻撃可能";
                        text.color = new Color(1,1,1,1);
                    }
                }
            }
            else
            {
                text.color = new Color(1,1,1,0);
            }
            
        }
            
    }
}
