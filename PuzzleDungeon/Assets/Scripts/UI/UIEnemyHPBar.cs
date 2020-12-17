using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnemyHPBar : UIBar
{
    Enemy enemy;
    SpriteRenderer sprite;
    float alpha;
    
    public float Alpha
    {
        get
        {
            return alpha;
        }
        set
        {
            if(value!=alpha)
            {
                ShowUI();
                alpha = value;
            }
        }
    }
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();   
        sprite = enemy.transform.GetComponent<SpriteRenderer>();
        if(sprite!=null)
        {
            backGround.color = backGround.color - new Color(0,0,0,1);
            back.color = back.color - new Color(0,0,0,1) ;
            front.color = front.color - new Color(0,0,0,1);
        }
        
    }
    void Update()
    {
        
        maxAmount = enemy.maxHP;
        CurrentAmount = enemy.currentHP;
        
        if(sprite!=null)
            Alpha = sprite.color.a;
        if(effect.activeSelf)
            if(!effect.GetComponent<ParticleSystem>().IsAlive())
                effect.SetActive(false);
    }
    public void ShowUI()
    {
        if(sprite.color.a == 0)
        {
            backGround.color = backGround.color - new Color(0,0,0,1);
            back.color = back.color - new Color(0,0,0,1) ;
            front.color = front.color - new Color(0,0,0,1);
        }
        else
        {
            backGround.color = backGround.color + new Color(0,0,0,1);
            back.color = back.color + new Color(0,0,0,1) ;
            front.color = front.color + new Color(0,0,0,1);
        }
    }
        
    
}
