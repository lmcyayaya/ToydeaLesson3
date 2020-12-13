using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHPBar : UIBar
{
    public PlayerData playerData;
    Text text;
    void Start()
    {
        text = transform.GetComponentInChildren<Text>();
    }

    
    void Update()
    {
        //ShowUI();
        maxAmount = playerData.maxHP;
        CurrentAmount = playerData.currentHP;
        if(effect.activeSelf)
            if(!effect.GetComponent<ParticleSystem>().IsAlive())
                effect.SetActive(false);
        text.text = currentAmount+" / "+maxAmount;

    }
}
