using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHPBar : UIBar
{
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();   
    }
    void Update()
    {
        maxAmount = enemy.maxHP;
        CurrentAmount = enemy.currentHP;
        if(effect.activeSelf)
            if(!effect.GetComponent<ParticleSystem>().IsAlive())
                effect.SetActive(false);
    }
}
