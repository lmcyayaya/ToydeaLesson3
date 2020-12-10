using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNodePiece : MonoBehaviour
{
    public bool hasOnMap;
    public int value;
    public Point index;

    [HideInInspector]
    public Vector3 pos;

    void Start() 
    {
            
    }
    public void Initialize(int val,Point p)
    {

    }
    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
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
    }

}
