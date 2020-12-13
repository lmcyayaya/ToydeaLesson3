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
        myTurn,turning,matching,dropping,action,enemyTurn
    }
    public State state;

    public Enemy[] enemys;
    Match3 game;
    void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        game = GetComponent<Match3>();
    }


    void FixedUpdate()
    {
        switch(state)
        {
            case State.myTurn :
            {
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
                foreach(Enemy enemy in enemys)
                {
                    enemy.EnemyNeedToDO();
                }
                state = State.myTurn;
                break;
            }
        }
    }
}
