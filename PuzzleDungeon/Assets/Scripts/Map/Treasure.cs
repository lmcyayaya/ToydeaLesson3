using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public int type;
    public Point index;
    public int point;
    SpriteRenderer sprite;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Map.Instance.getNodeAtPoint(index).getPiece().sprite.color.a == 0 )
        {
            sprite.color = new Color(1,1,1,0);
        }
        else
        {
            sprite.color = new Color(1,1,1,1);
        }
        if(Player.Instance.index.Equals(index))
        {
            switch(type)
            {
                case 1 :
                {
                    GetPoints();
                    CloseChest();
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
    void GetPoints()
    {
        PlayerData.Instance.remainPoint += point;
        PlayerData.Instance.levelUpHint.FadeIn();
    }
    void CloseChest()
    {
        transform.gameObject.SetActive(false);
    }
}
