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
    public float maxHP;
    public float currentHP;
    public float maxSP;
    public float currentSP;
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
        currentSP = Mathf.Clamp(currentSP,0,maxSP);
        currentHP = Mathf.Clamp(currentHP,0,maxHP);
    }
}
