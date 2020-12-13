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
    public float maxEP;
    public float currentEP;
    void Awake()
    {
        instance = this;   
    }
    void Start()
    {
        currentHP = maxHP;
    }
    void Update()
    {
        
    }
}
