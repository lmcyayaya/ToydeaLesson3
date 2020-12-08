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
        myTurn,turning,matching,dropping,enemyTurn
    }
    public State state;
    Match3 game;
    
    void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        game = GetComponent<Match3>();
    }


    void Update()
    {
        switch(state)
        {
            case State.myTurn :
            {
                break;
            }
            case State.turning :
            {
                break;
            }
            case State.matching :
            {
                game.Match3Update();
                break;
            }
            case State.dropping :
            {
                state = State.enemyTurn;
                break;
            }
            case State.enemyTurn :
            {
                state = State.myTurn;
                break;
            }
        }
    }
}
