using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance
    {
        get
        {
            return instance;
        }
    }
    private static TutorialManager instance;
    public int step;
    public bool showUp;
    public Step[] steps;
    void Awake() 
    {
        instance = this;
    }
    void Update()
    {   
        if(!showUp)
        {
            showUp = true;
            steps[step].ShowUp();
        }
    }
}
