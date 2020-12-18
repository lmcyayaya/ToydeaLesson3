using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Treasure : MonoBehaviour
{
    public int type;
    public Point index;
    public Canvas canvas;
    public RectTransform playerDataPage;
    public int point;
    public GameObject getSkill;
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
                    GetSkill();
                    CloseChest();
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
        
        var pointText = ObjectPool.TakeFromPool("Point");
        pointText.position = transform.position;
        var ui = pointText.GetComponent<UINumberPopUp>(); 
        ui.amount = point;
        ui.ShowPoints(playerDataPage.position);
        
        
    }
    void GetSkill()
    {
        Vector2 nPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(),Camera.main.WorldToScreenPoint(transform.position),canvas.worldCamera,out nPos);
        getSkill.GetComponent<RectTransform>().anchoredPosition = nPos;
        getSkill.SetActive(true);
    }

    void CloseChest()
    {
        transform.gameObject.SetActive(false);
    }
}
