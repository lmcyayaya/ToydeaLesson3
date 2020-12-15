using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHPBar : UIBar
{
    Enemy enemy;
    SpriteRenderer sprite;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();   
        sprite = enemy.transform.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        //ShowUI();
        maxAmount = enemy.maxHP;
        CurrentAmount = enemy.currentHP;
        if(sprite != null)
        {
            if(sprite.color.a == 0)
            {
                backGround.gameObject.SetActive(false);
                back.gameObject.SetActive(false);
                front.gameObject.SetActive(false);
            }
            else
            {
                backGround.gameObject.SetActive(true);
                back.gameObject.SetActive(true);
                front.gameObject.SetActive(true);
            }
        }
        
        if(effect.activeSelf)
            if(!effect.GetComponent<ParticleSystem>().IsAlive())
                effect.SetActive(false);
    }
}
