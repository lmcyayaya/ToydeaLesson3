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
        atk = (int)((1 + (atkDropsCount - 3) * 0.25f) * PlayerData.Instance.ATK * (1 + (combo - 1) * 0.25f));
        def = (int)((1 + (defDropsCount - 3) * 0.25f) * PlayerData.Instance.DEF * (1 + (combo - 1) * 0.25f));
        float tmp = Mathf.Clamp((0.5f - (PlayerData.Instance.MOVE * 0.1f)),0,float.MaxValue);
        move =(int)((1 + (moveDropsCount- 3) * 0.25f) * PlayerData.Instance.MOVE * (1 + (combo - 1) * tmp));
        hp =  (int)((1 + (hpDropsCount -  3) * 0.25f) * PlayerData.Instance.HP  * (1 + (combo - 1) * 0.25f));
        sp =  (int)((1 + (spDropsCount -  3) * 0.25f) * PlayerData.Instance.SP  * (1 + (combo - 1) * 0.25f));
        move = Mathf.Clamp(move,0,10);
        if(moveDropsCount == 0)
            move = 0;
        if(defDropsCount == 0)
            def = 0;
        if(hpDropsCount == 0)
            hp = 0;
        if(spDropsCount == 0)
            sp = 0;
    }
}
