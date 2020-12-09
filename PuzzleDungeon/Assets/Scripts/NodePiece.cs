using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int value;
    public Point index;

    [HideInInspector]
    public Vector2 pos;
    [HideInInspector]
    public RectTransform rect;
    Image img;

    public void Initialize(int v, Point p, Sprite piece)
    {
        img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        value = v;
        SetIndex(p);
        img.sprite = piece;
    }

    public void SetIndex(Point p)
    {
        index = p;
        ResetPosition();
        UpdateName();
    }
    public void RestSprite(Sprite piece)
    {
        img.sprite = piece;
    }
    public void ResetPosition()
    {
        pos = new Vector2(-224 + (64 * index.x), 224 - (64 * index.y));
    }
    public void MovePositionBack()
    {
        rect.anchoredPosition = pos;
    }
    public void MovePosition(Vector2 move)
    {
        rect.anchoredPosition += move * Time.deltaTime * 16f;
    }

    public void MovePositionTo(Vector2 move,float speed)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * speed);
    }
    public bool UpdatePiece()
    {
        if(Vector3.Distance(rect.anchoredPosition, pos) > 1)
        {
            MovePositionTo(pos,10f);
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            return false;
        }
    }
    
    void UpdateName()
    {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(StateManager.Instance.state == StateManager.State.myTurn || StateManager.Instance.state == StateManager.State.turning )
            MovePieces.Instance.MovePiece(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(StateManager.Instance.state != StateManager.State.turning)
            return;
       MovePieces.Instance.DropPiece();
    }
}
