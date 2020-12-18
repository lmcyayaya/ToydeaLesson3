using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int type;
    public Point index;
    public int poisonDamage;
    public int times;
    public ParticleSystem particle;
    bool hasIn;
    void Update()
    {
        if(Player.Instance.index.Equals(index))
        {
            switch(type)
            {
                case 1 :
                {
                    Poison();
                    CloseTrap();
                    break;
                }
                case 2 :
                {
                    break;
                }
                case 3 :
                {
                    break;
                }
                case 4 :
                {
                    break;
                }
                case 5 :
                {
                    break;
                }
            }
        }
        
    }
    void Poison()
    {
        if(hasIn)
            return;
        hasIn = true;
        PlayerData.Instance.isPoision = true;
        PlayerData.Instance.poisonDamage = poisonDamage;
        PlayerData.Instance.times = times;
        particle.gameObject.SetActive(true);
    }
    void CloseTrap()
    {
        
        if(particle.isStopped)
        {
            gameObject.SetActive(false);
        }
    }
}
