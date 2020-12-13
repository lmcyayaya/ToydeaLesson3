using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHPBar : UIBar
{
    Enemy enemy;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();   
    }
    void Update()
    {
        //ShowUI();
        maxAmount = enemy.maxHP;
        CurrentAmount = enemy.currentHP;
        if(effect.activeSelf)
            if(!effect.GetComponent<ParticleSystem>().IsAlive())
                effect.SetActive(false);
    }
}
