using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance
    {
        get
        {
            return instance;
        }
    }
    static StateManager instance;
    public enum State
    {
        myTurn,turning,matching,dropping,action,enemyTurn,pause
    }
    public State state;

    public NormalEnemy[] enemys;
    public TreasureChestEnemy treasureChestEnemy;
    public Boss boss;
    public Point bossRoom;
    int mostTimes = 0;
    bool enemyHasAction;
    bool firstTime = false;
    bool f7KeyUp;
    bool f6KeyUp;
    bool f5KeyUp;
    Match3 game;
    void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        game = GetComponent<Match3>();
        Screen.SetResolution(540,960,false);
    }


    void FixedUpdate()
    {
        switch(state)
        {
            case State.myTurn :
            {
                if(!firstTime)
                {
                    firstTime = true;
                    Player.Instance.DetectMap(Player.Instance.detectedMapDis,Player.Instance.index);
                }
                enemyHasAction = false;
                if(Input.GetKey(KeyCode.F7) && !f7KeyUp) 
                {
                    f7KeyUp = true;
                    PlayerData.Instance.currentExp += 100;
                }
                else if(!Input.GetKey(KeyCode.F8) && f7KeyUp)
                {
                    f7KeyUp = false;
                }
                if(Input.GetKey(KeyCode.F6) && !f6KeyUp)
                {
                    f6KeyUp = true;
                    Player.Instance.transform.position = Map.Instance.getNodeAtPoint(bossRoom).getPiece().transform.position;
                    Player.Instance.index = new Point(bossRoom.x,bossRoom.y);
                    Player.Instance.DetectMap(Player.Instance.detectedMapDis,Player.Instance.index);
                }
                else if(!Input.GetKey(KeyCode.F6) && f6KeyUp)
                {
                    f6KeyUp = false;
                }

                if(Input.GetKey(KeyCode.F5) && !f5KeyUp)
                {
                    f5KeyUp = true;
                    PlayerData.Instance.remainPoint += 3;
                }
                else if(!Input.GetKey(KeyCode.F5) && f5KeyUp)
                {
                    f5KeyUp = false;
                }
                break;
            }
            case State.turning :
            {
                game.TurningUpdate();
                break;
            }
            case State.matching :
            {
                game.Match3Update();
                break;
            }
            case State.dropping :
            {
                game.DroppingUpdate();
                break;
            }
            case State.action:
            {
                Player.Instance.ActionUpdate();
                break;
            }
            case State.enemyTurn :
            {
                if(enemyHasAction)
                    return;
                enemyHasAction = true;
                if(boss.open)
                {
                    boss.EnemyNeedToDO();
                }
                else
                {
                   
                    foreach(NormalEnemy enemy in enemys)
                    {
                        if(enemy.GetComponent<SpriteRenderer>().color.a >= 1)
                        {
                            enemy.EnemyNeedToDO();
                            if(enemy.moveTimes > mostTimes)
                                mostTimes = enemy.moveTimes ;
                        }
                    }
                    treasureChestEnemy.EnemyNeedToDO();
                    StartCoroutine(WaitToDo(mostTimes*0.25f,()=> state = State.myTurn)); 
                }
                
                break;
            }
            case State.pause :
            {
                break;
            }
        }
    }
    void EnemyAlpha()
    {
        foreach(Enemy enemy in enemys)
        {
            var sprite = enemy.GetComponent<SpriteRenderer>();
            if(sprite != null)
            {
                sprite.color -= new Color(0,0,0,1);
            }
        }
    }
    IEnumerator WaitToDo(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }
}
