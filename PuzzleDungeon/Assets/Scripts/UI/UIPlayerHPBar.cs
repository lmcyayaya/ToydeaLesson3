﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHPBar : UIBar
{
    Text text;
    void Start()
    {
        text = transform.GetComponentInChildren<Text>();
    }

    
    void Update()
    {
        //ShowUI();
        maxAmount = PlayerData.Instance.maxHP;
        CurrentAmount = PlayerData.Instance.currentHP;
        if(effect.activeSelf)
            if(!effect.GetComponent<ParticleSystem>().IsAlive())
                effect.SetActive(false);
        text.text = currentAmount+" / "+maxAmount;

    }
}
