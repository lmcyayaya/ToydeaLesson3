using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance
    {
        get
        {
            return instance;
        }
    }
    static PlayerData instance;
    public int CurrentLevel
    {
        get 
        {
            return currentLevel;
        }
        set
        {
            if(value > currentLevel)
            {
                currentLevel = value;
                remainPoint += 3;
                levelUpHint.FadeIn();
            }
        }
            
    }
    int currentLevel;
    public UIBreathFade levelUpHint;
    
    public bool isPoision;
    public int times;
    public int poisonDamage;

    public int currentExp;
    public int remainPoint;
    public float maxHP;
    public float currentHP;
    public float maxSP;
    public float currentSP;
    public float ATK;
    public float DEF;
    public int MOVE;
    public float HP;
    public float SP;
    bool doOnce;

    void Awake()
    {
        instance = this;   
    }
    void Start()
    {
        currentHP = maxHP;
        currentSP = maxSP;
    }
    void Update()
    {
        CurrentLevel = (currentExp + 80) / 80;
        currentSP = Mathf.Clamp(currentSP,0,maxSP);
        currentHP = Mathf.Clamp(currentHP,0,maxHP);

        switch(StateManager.Instance.state)
        {
            case StateManager.State.myTurn:
            {
                if(isPoision)
                {
                    if(!doOnce)
                    {
                        doOnce = true;
                        currentHP -= poisonDamage;
                        ShowDamageText();
                        times -= 1;
                        if(times == 0)
                        {
                            isPoision = false;
                        }
                    }
                }
                break;
            }
            case StateManager.State.enemyTurn:
            {
                doOnce = false;
                break;
            }
        }









    }
    void ShowDamageText()
    {
        var damageText = ObjectPool.TakeFromPool("Damage");
        var uiDamage = damageText.GetComponent<UINumberPopUp>();
        damageText.position = Player.Instance.transform.position;
        float damage = poisonDamage;
        uiDamage.amount = damage;
        uiDamage.ShowDamage();
    }
}
