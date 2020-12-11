using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessedData : MonoBehaviour
{
    public static ProcessedData Instance
    {
        get
        {
            return instance;
        }
    }
    static ProcessedData instance;

    [SerializeField]
    const float ATK = 15;
    [SerializeField]
    const float DEF = 2;
    [SerializeField]
    const int MOVE = 3;
    [SerializeField]
    const float HP = 10;
    [SerializeField]
    const float SP = 5;

    public int atkDropsCount;
    public int defDropsCount;
    public int moveDropsCount;
    public int hpDropsCount;
    public int spDropsCount;

    public int combo;
    public float atk;
    public float def;
    public int move;
    public float hp;
    public float sp;
    
    void Awake()
    {
        instance = this;    
    }

    public void InitData()
    {
        atkDropsCount = 0;
        defDropsCount = 0;
        moveDropsCount = 0;
        hpDropsCount = 0;
        spDropsCount = 0;
        combo = 0;
    }
    public void CalculateData()
    {
        atk = atkDropsCount * (1 + (combo - 1) * 0.25f) * ATK;
        def = defDropsCount * (1 + (combo - 1) * 0.25f) * DEF;
        move = (int)((moveDropsCount - 2) * (1 + (combo - 1) * 0.15f));
        hp = hpDropsCount * (1 + (combo - 1) * 0.25f) * HP;
        sp = spDropsCount * (1 + (combo - 1) * 0.25f) * SP;
        move = Mathf.Clamp(move,0,10);
    }
}
