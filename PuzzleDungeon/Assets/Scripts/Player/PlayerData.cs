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
            }
        }
            
    }
    private int currentLevel;
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
    }
}
