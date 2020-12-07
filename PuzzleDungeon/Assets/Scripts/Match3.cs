using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;

    [Header("UI Elements")]
    public Sprite[] pieces;
    public RectTransform gameBoard;
    [Header("Prefabs")]
    public GameObject nodePiece;
    [HideInInspector]
    public int width = 15;
    [HideInInspector]
    public int height = 15;
    int[] fills;
    Node[,] board;

    List<NodePiece> update;
    List<FlippedPieces> flipped;
    List<NodePiece> dead;

    System.Random random;
    void Awake()
    {
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        
    }

    // public void ApplyGravityToBoard()
    // {
    //     for (int x = 5; x < width; x++)
    //     {
    //         for (int y = (height - 5); y >= 0; y--) //Start at the bottom and grab the next
    //         {
    //             Point p = new Point(x, y);
    //             Node node = getNodeAtPoint(p);
    //             int val = getValueAtPoint(p);
    //             if (val != 0) continue; //If not a hole, move to the next
    //             for (int ny = (y - 1); ny >= -1; ny--)
    //             {
    //                 Point next = new Point(x, ny);
    //                 int nextVal = getValueAtPoint(next);
    //                 if (nextVal == 0)
    //                     continue;
    //                 if (nextVal != -1)
    //                 {
    //                     Node gotten = getNodeAtPoint(next);
    //                     NodePiece piece = gotten.getPiece();

    //                     //Set the hole
    //                     node.SetPiece(piece);
    //                     update.Add(piece);

    //                     //Make a new hole
    //                     gotten.SetPiece(null);
    //                 }
    //                 else//Use dead ones or create new pieces to fill holes (hit a -1) only if we choose to
    //                 {
    //                     int newVal = fillPiece();
    //                     NodePiece piece;
    //                     Point fallPnt = new Point(x, (-1 - fills[x]));
    //                     if(dead.Count > 0)
    //                     {
    //                         NodePiece revived = dead[0];
    //                         revived.gameObject.SetActive(true);
    //                         piece = revived;

    //                         dead.RemoveAt(0);
    //                     }
    //                     else
    //                     {
    //                         GameObject obj = Instantiate(nodePiece, gameBoard);
    //                         NodePiece n = obj.GetComponent<NodePiece>();
    //                         piece = n;
    //                     }

    //                     piece.Initialize(newVal, p, pieces[newVal - 1]);
    //                     piece.rect.anchoredPosition = getPositionFromPoint(fallPnt);

    //                     Node hole = getNodeAtPoint(p);
    //                     hole.SetPiece(piece);
    //                     ResetPiece(piece);
    //                     fills[x]++;
    //                 }
    //                 break;
    //             }
    //         }
    //     }
    // }

    // FlippedPieces getFlipped(NodePiece p)
    // {
    //     FlippedPieces flip = null;
    //     for (int i = 0; i < flipped.Count; i++)
    //     {
    //         if (flipped[i].getOtherPiece(p) != null)
    //         {
    //             flip = flipped[i];
    //             break;
    //         }
    //     }
    //     return flip;
    // }

    void StartGame()
    {
        fills = new int[width];
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();
        // flipped = new List<FlippedPieces>();
        // dead = new List<NodePiece>();

        InitializeBoard();
        VerifyBoard();
        CopyBoard();
        InstantiateBoard();
        
    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? - 1 : fillPiece(), new Point(x, y));
            }
        }
    }

    void VerifyBoard()
    {
        List<int> remove;
        for (int x = 5; x < width; x++)
        {
            for (int y = 5; y < height; y++)
            {
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                if (val <= 0) continue;

                remove = new List<int>();
                while (isConnected(p, true).Count > 0)
                {
                    val = getValueAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }
            }
        }
    }
    void CopyBoard()
    {
        //左邊複製
        for(int x = 0; x < 5;x++)
        {
            for(int y = 5; y < 10; y++)
            {
                Point p = new Point(x,y);
                Point bordP = new Point(x+5,y);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);

            }
        }
        //上面複製
        for(int x = 5; x < 10;x++)
        {
            for(int y = 0; y < 5; y++)
            {
                Point p = new Point(x,y);
                Point bordP = new Point(x,y+5);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);
            }
        }
        //右邊
        for(int x = 10; x < width;x++)
        {
            for(int y = 5; y < 10; y++)
            {
                Point p = new Point(x,y);
                Point bordP = new Point(x-5,y);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);
            }
        }
        //下面
        for(int x = 5; x < 10;x++)
        {
            for(int y = 10; y < height; y++)
            {   
                Point p = new Point(x,y);
                Point bordP = new Point(x,y-5);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);
            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = getNodeAtPoint(new Point(x, y));

                int val = node.value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(-288 + (64 * x), 288  - (64 * y));
                piece.Initialize(val, new Point(x, y), pieces[val - 1]);
                node.SetPiece(piece);
            }
        }
    }
     
    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        update.Add(piece);
    }

    // public void FlipPieces(Point one, Point two, bool main)
    // {
    //     if (getValueAtPoint(one) < 0) return;

    //     Node nodeOne = getNodeAtPoint(one);
    //     NodePiece pieceOne = nodeOne.getPiece();
    //     if (getValueAtPoint(two) > 0)
    //     {
    //         Node nodeTwo = getNodeAtPoint(two);
    //         NodePiece pieceTwo = nodeTwo.getPiece();
    //         nodeOne.SetPiece(pieceTwo);
    //         nodeTwo.SetPiece(pieceOne);

    //         if(main)
    //             flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

    //         update.Add(pieceOne);
    //         update.Add(pieceTwo);
    //     }
    //     else
    //         ResetPiece(pieceOne);
    // }

    List<Point> isConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };
        
        foreach(Point dir in directions) //Checking if there is 2 or more same shapes in the directions
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for(int i = 1; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if(getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) //If there are more than 1 of the same shape in the direction then we know it is a match
                AddPoints(ref connected, line); //Add these points to the overarching connected list
        }

        for(int i = 0; i < 2; i++) //Checking if we are in the middle of two of the same shapes
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2]) };
            foreach (Point next in check) //Check both sides of the piece, if they are the same value, add them to the list
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
                AddPoints(ref connected, line);
        }

        for(int i = 0; i < 4; i++) //Check for a 2x2
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;

            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i], directions[next])) };
            foreach (Point pnt in check) //Check all sides of the piece, if they are the same value, add them to the list
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if(main) //Checks for other matches along the current match
        {
            for (int i = 0; i < connected.Count; i++)
                AddPoints(ref connected, isConnected(connected[i], false));
        }
        return connected;
    }
    
    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach(Point p in add)
        {
            bool doAdd = true;
            for(int i = 0; i < points.Count; i++)
            {
                if(points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) points.Add(p);
        }
    }

    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }

    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }

    void setValueAtPoint(Point p, int v)
    {
        board[p.x, p.y].value = v;
    }


    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }

    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdeghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for (int i = 0; i < 20; i++)
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        return seed;
    }
    public Node getNodeAtPoint(Point p)
    {
        return board[p.x, p.y];
    }

    public Vector2 getPositionFromPoint(Point p)
    {
        return new Vector2(-288 + (64 * p.x), 288 - (64 * p.y));
    }
    public List<NodePiece> GetNodePiecesFromTheSameLineX(NodePiece piece)
    {
        List<NodePiece> line = new List<NodePiece>();
        for(int x = 0; x < width; x++)
        {
            line.Add(getNodeAtPoint(new Point(x,piece.index.y)).getPiece());
        }
        return line;
    }
    public List<NodePiece> GetNodePiecesFromTheSameLineY(NodePiece piece)
    {
        List<NodePiece> line = new List<NodePiece>();
        for(int y = 0; y < height; y++)
        {
            line.Add(getNodeAtPoint(new Point(piece.index.x,y)).getPiece());
        }
        return line;
    }

}

[System.Serializable]
public class Node
{
    public int value; //0 = blank, 1 = cube, 2 = sphere, 3 = cylinder, 4 = pryamid, 5 = diamond, -1 = hole
    public Point index;
    NodePiece piece;

    public Node(int v, Point i)
    {
        value = v;
        index = i;
    }

    public void SetPiece(NodePiece p)
    {
        piece = p;
        value = (piece == null) ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index);
    }

    public NodePiece getPiece()
    {
        return piece;
    }
}

[System.Serializable]
public class FlippedPieces
{
    public NodePiece one;
    public NodePiece two;

    public FlippedPieces(NodePiece o, NodePiece t)
    {
        one = o; two = t;
    }

    public NodePiece getOtherPiece(NodePiece p)
    {
        if (p == one)
            return two;
        else if (p == two)
            return one;
        else
            return null;
    }
}
