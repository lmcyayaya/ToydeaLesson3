using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public static Map Instance
    {
        get
        {
            return instance;
        }
    }
    static Map instance;
    public Vector3 centerPoint;
    public float cellSize;
    public int width;
    public int height;
    public GameObject road;
    public List<MapNodePiece> hasOnMap;
    MapNode[,] mapBoard;
    void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        InitializedBoard();
        InstantiateBoard();
    }

    void InitializedBoard()
    {
        transform.position = centerPoint + (Vector3.left * width / 2  + Vector3.up * height / 2) * cellSize;

        for(int i = 0; i < hasOnMap.Count; i++)
        {
            hasOnMap[i].index.x = (int)((hasOnMap[i].transform.position.x - transform.position.x) / cellSize);
            hasOnMap[i].index.y = (int)((transform.position.y - hasOnMap[i].transform.position.y) / cellSize);
        }

        mapBoard = new MapNode[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                foreach(MapNodePiece piece in hasOnMap)
                {
                    if(x == piece.index.x && y == piece.index.y)
                    {
                        mapBoard[x, y] = new MapNode(piece.value, new Point(x, y));
                        mapBoard[x, y].SetPiece(piece);
                        mapBoard[x, y].getPiece().FirstTime();
                        hasOnMap.Remove(piece);
                        break;
                    }   
                }
                if(mapBoard[x,y]==null)
                    mapBoard[x, y] = new MapNode(0, new Point(x, y));
            }
        }
    }
    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                MapNode node = getNodeAtPoint(new Point(x, y));

                int val = node.value;
                if (val != 0 ) continue;
                GameObject p = Instantiate(road, transform);
                MapNodePiece piece = p.GetComponent<MapNodePiece>();
                Transform rect = p.GetComponent<Transform>();
                rect.position = new Vector2(transform.position.x + (cellSize * x)+cellSize/2, transform.position.y - (cellSize * y)-cellSize/2);
                node.SetPiece(piece);
                node.getPiece().FirstTime();
                
            }
        }
    }
    public MapNode getNodeAtPoint(Point p)
    {
        return mapBoard[p.x, p.y];
    }
    public void SetNewValueToNord(Point p,int value)
    {
        getNodeAtPoint(p).getPiece().value = value;
        getNodeAtPoint(p).value = value;
    }
    public void SwitchNodePieceAndValue(Point a,Point b)
    {
        MapNodePiece pieceA = getNodeAtPoint(a).getPiece();
        MapNode nodeA = getNodeAtPoint(a);
        MapNodePiece pieceB = getNodeAtPoint(b).getPiece();
        MapNode nodeB = getNodeAtPoint(b);

        nodeA.SetPiece(pieceB);
        nodeB.SetPiece(pieceA);
    }
    public bool isContainPointInPointList(List<Point> list, Point p)
    {
        for(int i = 0 ; i < list.Count; i++)
        {
            if(list[i].x == p.x && list[i].y == p.y)
                return true;
        }
        return false;
    }

    
}
[System.Serializable]
public class MapNode
{
    public int value; //0 = road, 1 = wall,2 = enemy
    public Point index;

    MapNodePiece piece;
    

    public MapNode(int v, Point i)
    {
        value = v;
        index = i;
    }

    public void SetPiece(MapNodePiece p)
    {
        piece = p;
        value = (piece == null) ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index);
    }
    public MapNodePiece getPiece()
    {
        return piece;
    }
}
