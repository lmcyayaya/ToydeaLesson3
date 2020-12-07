using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePieces : MonoBehaviour
{
    public static MovePieces Instance
    {
        get
        {
            return instance;
        }
    }
    static MovePieces instance;
    public Canvas canvas;
    Match3 game;
    NodePiece selectedPiece;
    List<NodePiece> moving;
    Point newIndex;
    Vector2 startPos;
    Vector2 mouseStart;
    int cDir = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        game = GetComponent<Match3>();
    }

    void Update()
    {
        if(selectedPiece != null)
        {
            RectTransform rect = selectedPiece.rect.parent.GetComponent<RectTransform>();
            Vector2 nPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect,Input.mousePosition,canvas.worldCamera,out nPos);

            //因為珠子的錨點在左上角所以要減去盤面的一半
            float disX = nPos.x + rect.sizeDelta.x/2 - startPos.x;
            float disY = nPos.y - rect.sizeDelta.x/2 - startPos.y;
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            if (dir.magnitude > 1 && cDir == 0) 
            {
                
                if (aDir.x > aDir.y)
                    cDir = 1;
                else if(aDir.y > aDir.x)
                    cDir = 2;
            }
            else if(dir.magnitude < 0)
                cDir = 0;
            Debug.Log((int)((disX+32)/64)+"個");

            switch(selectedPiece.index.x)
            {
                case 5:
                {
                    if(disX > 64 * ((game.width / 3) - 0.5f))
                        disX = 64 * ((game.width / 3) - 0.5f);
                    else if(disX < -32)
                        disX = -32;
                    break;
                }
                    
                case 6:
                {
                    if(disX > 64 * ((game.width/3) - 1 - 0.5f))
                        disX = 64 * ((game.width/3) - 1 - 0.5f);
                    else if(disX < -64 * ((game.width/3) - 3 - 0.5f))
                        disX = -64 * ((game.width/3) - 3 - 0.5f);

                    break;
                }
                
                case 7:
                {
                    if(disX > 64 * ((game.width/3) - 2 - 0.5f))
                        disX = 64 * ((game.width/3) - 2 - 0.5f);
                    else if(disX < -64 * ((game.width/3) - 2 - 0.5f))
                        disX = -64 * ((game.width/3) - 2 - 0.5f);
                    break;
                }
                
                case 8:
                {
                    if(disX > 64 * ((game.width/3) - 4 - 0.5f))
                        disX = 64 * ((game.width/3) - 4 - 0.5f);
                    else if(disX < -64 * ((game.width/3) - 1 - 0.5f))
                        disX = -64 * ((game.width/3) - 1 - 0.5f);
                   break; 
                }
                
                case 9:
                {
                    if(disX > 32)
                        disX = 32;
                    else if(disX < -64 * ((game.width / 3) - 0.5f))
                        disX = -64 * ((game.width / 3) - 0.5f);
                    break;
                }
                    
            }

            switch(cDir)
            {
                case 1:
                {
                    moving = game.GetNodePiecesFromTheSameLineX(selectedPiece);
                    foreach(NodePiece piece in moving)
                    {
                        Vector2 pos = Vector2.right * disX;
                        piece.MovePositionTo(game.getPositionFromPoint(piece.index)+pos);
                    }
                    break;
                }
                    
                case 2:
                {
                    moving = game.GetNodePiecesFromTheSameLineY(selectedPiece);
                    foreach(NodePiece piece in moving)
                    {
                        Vector2 pos = Vector2.up * disY;
                        piece.MovePositionTo(game.getPositionFromPoint(piece.index)+pos);
                    }
                    break;
                }  
            }
            
            

        }
    }


    public void MovePiece(NodePiece piece)
    {
        if (selectedPiece != null) return;
        selectedPiece = piece;
        mouseStart = Input.mousePosition;
        startPos = piece.rect.anchoredPosition;
    }

    public void DropPiece()
    {
        if (selectedPiece == null) return;
        cDir = 0;
        selectedPiece = null;
    }
    void CheckIndex(int i)
    {
        

    }
}
