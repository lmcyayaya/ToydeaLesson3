using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    bool click;
    public virtual void ShowUp()
    {

    }
    public virtual void Close()
    {

    }
    public void NextStep(float waitTime)
    {
        if(!click)
            click = true;
        StartCoroutine(WaitToDO(waitTime,()=>
        {
            TutorialManager.Instance.step +=1;
            TutorialManager.Instance.showUp = false;
            click = false;
        }));
        
    }
    public void BackStep(float waitTime)
    {
        if(!click)
            click = true;
        StartCoroutine(WaitToDO(waitTime,()=>
        {
            TutorialManager.Instance.step -=1;
            TutorialManager.Instance.showUp = false;
            click = false;
        }));
        
    }
    IEnumerator WaitToDO(float time, Action aciton)
    {
        yield return new WaitForSeconds(time);
        aciton();
    }
    
}
