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
    public RectTransform selectBox;
    Match3 game;
    NodePiece selectedPiece;
    List<NodePiece> moving;
    Point newIndex;
    Vector2 startPos;
    Vector2 mouseStart;
    //移動個數
    int moveAmount;
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
            //selectedPiece.rect.anchoredPosition = nPos;
            selectBox.position = selectedPiece.rect.position;
            
            //becaus the anchor preset of the piece is top Left ,so need to  minus (board width /2) 因為珠子的錨點在左上角所以要減去盤面的一半
            float disX = nPos.x + rect.sizeDelta.x/2 - startPos.x;
            float disY = nPos.y - rect.sizeDelta.y/2 - startPos.y;
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            if (dir.magnitude > 32 && cDir == 0) 
            {
                if (aDir.x > aDir.y)
                    cDir = 1;
                else if(aDir.y > aDir.x)
                    cDir = 2;
            }
            else if(dir.magnitude < 32)
                cDir = 0;

            //avoid piece cross over the board 避免拉超過盤面 每個位置上的珠子都只能拉到最旁邊為止
            switch(selectedPiece.index.x)
            {
                //    
                //  45678 ←index
                //4 XXXXX
                //5 XXXXX
                //6 XXXXX ←board
                //7 XXXXX
                //8 XXXXX

                //most left
                case 4:
                {
                    //because the size of piece is 64x64
                    if(disX > 64 * 4)
                        disX = 64 * 4;
                    else if(disX < 0)
                        disX = 0;
                    break;
                }
                //
                case 5:
                {
                    if(disX > 64 * 3)
                        disX = 64 * 3;
                    else if(disX < -64 * 1)
                        disX = -64 * 1;

                    break;
                }
                // middle
                case 6:
                {
                    if(disX > 64 * 2)
                        disX = 64 * 2;
                    else if(disX < -64 * 2)
                        disX = -64 * 2;
                    break;
                }
                
                case 7:
                {
                    if(disX > 64 * 1)
                        disX = 64 * 1;
                    else if(disX < -64 * 3)
                        disX = -64 * 3;
                   break; 
                }
                //most right
                case 8:
                {
                    if(disX > 0)
                        disX = 0;
                    else if(disX < -64 * 4)
                        disX = -64 * 4;
                    break;
                }     
            }

            switch(selectedPiece.index.y)
            {
                case 4:
                {
                    if(disY > 0)
                        disY = 0;
                    else if(disY < -64 * 4)
                        disY = -64 * 4;
                    break;
                }
                    
                case 5:
                {
                    if(disY > 64 * 1)
                        disY = 64 * 1;
                    else if(disY < -64 * 3)
                        disY = -64 * 3;
                    break;
                }
                
                case 6:
                {
                     if(disY > 64 * 2)
                        disY = 64 * 2;
                    else if(disY < -64 * 2)
                        disY = -64 * 2;
                    break;
                }
                
                case 7:
                {
                     if(disY > 64 * 3)
                        disY = 64 * 3;
                    else if(disY < -64 * 1)
                        disY = -64 * 1;
                   break; 
                }
                
                case 8:
                {
                    if(disY > 64 * 4)
                        disY = 64 * 4;
                    else if(disY < 0)
                        disY = 0;
                    break;
                }     
            }

            if(disX > 0 )
                disX = disX + 32;
            else if(disX < 0)
                disX = disX -32;
            
            if(disY > 0 )
                disY = disY + 32;
            else if(disY < 0)
                disY = disY -32;

            if(Mathf.Abs((int)((disX)/63)) > 0 || Mathf.Abs((int)((disY)/63)) > 0)
                StateManager.Instance.state = StateManager.State.turning;


            switch(cDir)
            {
                case 1:
                {
                    moving = game.getNodePiecesFromTheSameLineX(selectedPiece);
                    foreach(NodePiece piece in moving)
                    {
                        Vector2 pos = Vector2.right * (int)((disX)/63) * 64;
                        moveAmount = (int)((disX)/63);
                        piece.MovePositionTo(game.getPositionFromPoint(piece.index)+pos,30f);
                    }
                    break;
                }
                    
                case 2:
                {
                    moving = game.getNodePiecesFromTheSameLineY(selectedPiece);
                    foreach(NodePiece piece in moving)
                    {
                        Vector2 pos = Vector2.up * (int)((disY)/63) * 64;
                        moveAmount = -(int)((disY)/63);
                        piece.MovePositionTo(game.getPositionFromPoint(piece.index)+pos,20f);
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
        if (selectedPiece == null ) 
        {
            //StateManager.Instance.state = StateManager.State.matching;
            return;
        }
        
        selectedPiece = null;
        selectBox.anchoredPosition = new Vector2(-432,0);

        if (moving == null || moveAmount == 0)
        {
            //StateManager.Instance.state = StateManager.State.matching;
            return;
        }
        

        if(moveAmount > 0)
        {
            int j = 8-moveAmount;
            for(int i = 8; i > 3; i--)
            {
                moving[i].value = moving[j].value;
                moving[i].RestSprite(game.pieces[moving[i].value-1]);
                j--;
            }
        }
        else if(moveAmount < 0)
        {
            int j = 4-moveAmount;
            for(int i = 4; i < 9; i++)
            {
                moving[i].value = moving[j].value;
                moving[i].RestSprite(game.pieces[moving[i].value-1]);
                j++;
            }
        }


        foreach(NodePiece piece in moving)
        {
            piece.MovePositionBack();
        }
        cDir = 0;
        game.UpdateNodeData();
        game.CopyBoard();
        moving = null;
    }
}
