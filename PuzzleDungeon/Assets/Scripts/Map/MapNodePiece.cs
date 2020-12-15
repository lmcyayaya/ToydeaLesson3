using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNodePiece : MonoBehaviour
{
    public int value;
    public Point index;

    [HideInInspector]
    public Vector3 pos;
    public Color lastColor;
    public SpriteRenderer sprite;
    void Start() 
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void FirstTime()
    {
        sprite.color = new Color(lastColor.r,lastColor.g,lastColor.b,0);
    }

    public void SetIndex(Point p)
    {
        index = p;
        sprite = GetComponent<SpriteRenderer>();
        lastColor = sprite.color;
        ResetPosition();
        UpdateName();
    }
    public void SetColor(Color color)
    {
        lastColor = sprite.color;
        sprite.color = color;
    }
    public void BackToLastColor()
    {
        sprite.color = lastColor;
    }
    public void ResetPosition()
    {
        pos = new Vector2(Map.Instance.transform.position.x + (Map.Instance.cellSize * index.x), Map.Instance.transform.position.y - (Map.Instance.cellSize * index.y));
    }
    void UpdateName()
    {
        if(value == 0)
            transform.name = "Road [" + index.x + ", " + index.y + "]";
        if(value == 1)
            transform.name = "Wall [" + index.x + ", " + index.y + "]";
        if(value == 2)
            transform.name = "EnemyBody [" + index.x + ", " + index.y + "]";
    }

}
