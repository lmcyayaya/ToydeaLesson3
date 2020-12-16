using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float atk;
    void Start()
    {
        currentHP = maxHP;
    }
    public virtual void EnemyNeedToDO()
    {

    }
    public virtual void Dead()
    {
        
    }

}
