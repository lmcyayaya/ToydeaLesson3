using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Mines : MonoBehaviour
{
    public bool minesOn;
    AttackTrigger attackTrigger;
    Boss boss;
    void Start()
    {
        attackTrigger = GetComponentInParent<AttackTrigger>();
        boss = GetComponentInParent<Boss>();
    }
    void Update()
    {
        if(minesOn && attackTrigger.playerInHere)
        {
            DisableMines();
            float tmp =  Mathf.Clamp(attackTrigger.atk - ProcessedData.Instance.def,0,int.MaxValue);
            PlayerData.Instance.currentHP -= tmp;
        }
    }
    public void DisableMines()
    {
        minesOn = false;
        transform.DOScale(Vector3.zero,0.5f).SetEase(Ease.InBack);
    }
}
