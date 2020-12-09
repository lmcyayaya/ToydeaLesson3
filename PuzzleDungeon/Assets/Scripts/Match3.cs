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
    int width = 13;
    int height = 13;
    int[] fills;
    Node[,] board;
    List<List<Point>> comboList;
    bool clearing = false  ;
    
    List<NodePiece> dead;
    List<NodePiece> update;
    System.Random random;
    void Awake()
    {
    }

    void Start()
    {
        StartGame();
    }
    public void DroppingUpdate()
    {
        if(update.Count == 0)
            StateManager.Instance.state = StateManager.State.matching;

        List<NodePiece> finishedUpdating = new List<NodePiece>();
        for(int i = 0; i < update.Count; i++)
        {
            NodePiece piece = update[i];
            if (!piece.UpdatePiece()) finishedUpdating.Add(piece);
        }

        for (int i = 0; i < finishedUpdating.Count; i++)
        {
            NodePiece piece = finishedUpdating[i];
            
            update.Remove(piece);
        }
    }
    public void Match3Update()
    {
        if(clearing)
            return;
        List<Point> connected = new List<Point>();
        comboList.Clear();
        for(int x = 4; x < 9; x++)
        {
            for(int y = 4;y < 9; y++)
            {
                if(!isContainPointInPointList(connected,new Point(x,y)))
                    AddPoints(ref connected,isConnected(new Point(x,y),false));

            }
        }

        if(connected.Count == 0)
        {
            //If do not match anything do something here
            CopyBoard();
            StateManager.Instance.state = StateManager.State.enemyTurn;
            return;
        }
        else
        {
            CheckConnectTypeAndAmount(connected);
            ComboListAdjust();
            RemoveEmptyCombo();

            StartCoroutine(ClearCombo());
        }
    }
    IEnumerator ClearCombo()
    {
        clearing = true;
        foreach(List<Point> pointList in comboList)
        {
            foreach(Point p in pointList)
            {
                Node node = getNodeAtPoint(p);
                NodePiece nodePiece =node.getPiece();
                if(nodePiece!=null)
                {
                    nodePiece.gameObject.SetActive(false);
                    dead.Add(nodePiece);
                }
                node.SetPiece(null); 
            }
            if(comboList.IndexOf(pointList)==comboList.Count-1)
                yield return null;
            else
                yield return new WaitForSeconds(0.2f);
        }
        clearing = false;
        ApplyGravityToBoard();
        StateManager.Instance.state = StateManager.State.dropping;
    }

    void ApplyGravityToBoard()
    {
        for (int x = 4; x < 9; x++)
        {
            for (int y = 8 ; y > 3; y--) //Start at the bottom and grab the next
            {
                Point p = new Point(x, y);
                Node node = getNodeAtPoint(p);
                int val = getValueAtPoint(p);
                if (val != 0) continue; //If not a hole, move to the next

                for(int ny = (y-1); ny >= 3 ; ny--)
                {
                    Point next = new Point(x, ny);
                    int nextVal = getValueAtPoint(next);
                    if(nextVal == 0)
                        continue;

                    if(nextVal != -1)
                    {
                        Node got = getNodeAtPoint(next);
                        NodePiece piece = got.getPiece();

                        //Set the hole
                        node.SetPiece(piece);
                        update.Add(piece);

                        //Replace the hole
                        got.SetPiece(null);
                    }
                    else //hit an end
                    {
                        //Fill in the hole
                        int newVal = fillPiece();
                        NodePiece piece;
                        Point fallPoint = new Point(x,3);
                        if(dead.Count > 0)
                        {
                            NodePiece revived = dead[0];
                            revived.gameObject.SetActive(true);
                            revived.rect.anchoredPosition = getPositionFromPoint(fallPoint);
                            piece = revived;
                            


                            dead.RemoveAt(0);
                        }
                        else
                        {
                            GameObject obj = Instantiate(nodePiece, gameBoard);
                            NodePiece n =obj.GetComponent<NodePiece>();
                            RectTransform rect = obj.GetComponent<RectTransform>();
                            rect.anchoredPosition = getPositionFromPoint(fallPoint);
                            piece = n;
                        }
                        piece.Initialize(newVal, p, pieces[newVal - 1]);
                        Node hole = getNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);
                    }
                    break;
                }
            }
        }
    }
    void StartGame()
    {
        fills = new int[width];
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        comboList = new List<List<Point>>();
        update = new List<NodePiece>();
        dead = new List<NodePiece>();

        InitializeBoard();
        VerifyBoard();
        InstantiateBoard();
        CopyBoard();
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
        for (int x = 4; x < 9; x++)
        {
            for (int y = 4; y < 9; y++)
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
                rect.anchoredPosition = new Vector2(-224 + (64 * x), 224  - (64 * y));
                piece.Initialize(val, new Point(x, y), pieces[val - 1]);
                node.SetPiece(piece);
            }
        }
    }
    void CheckConnectTypeAndAmount(List<Point> connected)
    {
        int atk = 0;
        int move = 0 ;
        int def = 0;
        int sp = 0;

        foreach(Point p in connected)
        {
            //Debug.Log("("+p.x+","+p.y+")");
            switch(getValueAtPoint(p))
            {
                case 1 :
                {
                    atk += 1;
                    break;
                }
                case 2 :
                {
                    move += 1;
                    break;
                }
                case 3 :
                {
                    def += 1;
                    break;
                }
                case 4 :
                {
                    sp += 1;
                    break;
                }
                
            }
        }
        Debug.Log("Cube有"+atk+"個,"+"cylinder有"+move+"個,"+"prymid有"+def+"個,"+"sphere有"+sp+"個,");
    }
    void ComboListAdjust()
    {

        Point[] directions =
        {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };
        for(int i = 0; i < comboList.Count; i++)
        {
            for(int j = 0; j < comboList[i].Count;j++)
            {
                foreach(Point dir in directions)
                {
                    Point check = Point.add(comboList[i][j],dir);
                    for(int k = i+1; k < comboList.Count; k ++)
                    {
                        if(isContainPointInPointList(comboList[k],check) && getValueAtPoint(comboList[i][j]) == getValueAtPoint(check))
                        {
                            comboList[i] = forComboAddPoints(comboList[i],comboList[k]);
                            comboList[k].Clear();
                        }
                    }
                }
                
            }
        }



    }
    void RemoveEmptyCombo()
    {
        for(int i = 0; i< comboList.Count;i++)
        {
            if(comboList[i].Count == 0)
            {
                comboList.Remove(comboList[i]);
            }
        }
    }
    List<Point> isConnected(Point p, bool main)
    {
        
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        if(val == 0)
            return connected;
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
            line.Add(p);
            for(int i = 1; i < 5; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if(check.x < 4 ||check.x > 8 || check.y < 4 || check.y > 8)
                    continue;
                if(getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
                else
                    break;
            }

            if (same > 1) //If there are more than 1 of the same shape in the direction then we know it is a match
            {
                AddPoints(ref connected, line); //Add these points to the overarching connected list
                comboList.Add(line);
            }
                
        }

        for(int i = 0; i < 2; i++) //Checking if we are in the middle of two of the same shapes
        {
            List<Point> line = new List<Point>();

            int same = 0;
            line.Add(p);
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2]) };
            
            foreach (Point next in check) //Check both sides of the piece, if they are the same value, add them to the list
            {
                if(next.x < 4 ||next.x > 8 || next.y < 4 || next.y > 8)
                    continue;
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
            {
                AddPoints(ref connected, line); //Add these points to the overarching connected list
                comboList.Add(line);
            }
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
    List<Point> forComboAddPoints(List<Point> points, List<Point> add)
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
        return points;
    }
    string ShowName(Point p)
    {
        switch(getValueAtPoint(p))
        {
            case 0:
                return "blank";
            
            case 1:
                return "cube";
            
            case 2:
                return "cylider";
            
            case 3:
                return "pryamid";
            
            case 4:
                return "sphere";
        }
        return null;
    }
    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }

    int getValueAtPoint(Point p)
    {
        if (p.x < 4 || p.x > 8 || p.y < 4 || p.y > 8) return -1;
        return board[p.x, p.y].value;
    }
    bool isContainPointInPointList(List<Point> list, Point p)
    {
        for(int i = 0 ; i < list.Count; i++)
        {
            if(list[i].x == p.x && list[i].y == p.y)
                return true;
        }
        return false;
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
    void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        update.Add(piece);
    }
    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdeghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for (int i = 0; i < 20; i++)
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        return seed;
    }
    public void UpdateNodeData()
    {
        foreach(Node node in board)
        {
            if(node.getPiece()!=null)
                node.value = node.getPiece().value;
        }
    }
    public void CopyBoard()
    {
        //Left
        for(int x = 0; x < 4;x++)
        {
            for(int y = 4; y < 9; y++)
            {
                Point p = new Point(x,y);
                Point bordP = new Point(x+5,y);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);
                Node node = getNodeAtPoint(p);
                node.getPiece().value = val;
                node.getPiece().RestSprite(pieces[val-1]);

            }
        }
        //Top
        for(int x = 4; x < 9;x++)
        {
            for(int y = 0; y < 4; y++)
            {
                Point p = new Point(x,y);
                Point bordP = new Point(x,y+5);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);
                Node node = getNodeAtPoint(p);
                node.getPiece().value = val;
                node.getPiece().RestSprite(pieces[val-1]);
            }
        }
        //Right
        for(int x = 9; x < width;x++)
        {
            for(int y = 4; y < 9; y++)
            {
                Point p = new Point(x,y);
                Point bordP = new Point(x-5,y);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);
                Node node = getNodeAtPoint(p);
                node.getPiece().value = val;
                node.getPiece().RestSprite(pieces[val-1]);
            }
        }
        //bottom
        for(int x = 4; x < 9;x++)
        {
            for(int y = 9; y < height; y++)
            {   
                Point p = new Point(x,y);
                Point bordP = new Point(x,y-5);
                int val = getValueAtPoint(bordP);
                if(val <=0 ) continue;

                setValueAtPoint(p,val);
                Node node = getNodeAtPoint(p);
                node.getPiece().value = val;
                node.getPiece().RestSprite(pieces[val-1]);
            }
        }
    }

    public Node getNodeAtPoint(Point p)
    {
        return board[p.x, p.y];
    }
    public Vector2 getPositionFromPoint(Point p)
    {
        return new Vector2(-224 + (64 * p.x), 224 - (64 * p.y));
    }
    public List<Node> getNodesFromTheSameLineX(Point p)
    {
        List<Node> line = new List<Node>();
        for(int x = 0; x < width; x++)
        {
            line.Add(getNodeAtPoint(new Point(x,p.y)));
        }
        return line;
    }
    public List<NodePiece> getNodePiecesFromTheSameLineX(NodePiece piece)
    {
        List<NodePiece> line = new List<NodePiece>();
        for(int x = 0; x < width; x++)
        {
            line.Add(getNodeAtPoint(new Point(x,piece.index.y)).getPiece());
        }
        return line;
    }
    public List<NodePiece> getNodePiecesFromTheSameLineY(NodePiece piece)
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

