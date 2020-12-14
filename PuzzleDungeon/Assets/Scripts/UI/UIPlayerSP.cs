using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPlayerSP : UIBar
{
    Text text;
    void Start()
    {
        text = transform.GetComponentInChildren<Text>();
        effect = null;
    }

    
    void Update()
    {
        //ShowUI();
        maxAmount = PlayerData.Instance.maxSP;
        CurrentAmount = PlayerData.Instance.currentSP;

        text.text = currentAmount+" / "+maxAmount;

    }
}
