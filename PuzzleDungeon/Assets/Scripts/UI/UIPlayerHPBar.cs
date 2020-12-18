using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHPBar : UIBar
{
    public int attackType;
    public GameObject[] effects;
    Text text;
    void Start()
    {
        text = transform.GetComponentInChildren<Text>();
    }

    
    void Update()
    {
        MaxAmount = PlayerData.Instance.maxHP;
        CurrentAmount = PlayerData.Instance.currentHP;
        if(effect.activeSelf)
            if(!effect.GetComponent<ParticleSystem>().IsAlive())
                effect.SetActive(false);
        text.text = currentAmount + " / " + MaxAmount;

    }
    protected override void HurtEffect()
    {
        if(effects.Length != 0 && attackType < effects.Length)
            effects[attackType].SetActive(true);
    }
}
