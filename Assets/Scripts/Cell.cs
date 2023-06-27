using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IDropHandler
{
    private Vector2 _position;
    private Image _image;
    private Color _color;
    private GridGenerator _gridGenerator;
    private GameManager _gameManager;
    private bool _isJail = false;
    [SerializeField] private AudioClip _moveSound;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _gridGenerator = FindObjectOfType<GridGenerator>();
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void SetJail()
    {
        _isJail = true;
    }


    private void Start()
    {
        _color = _image.color;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDropPiece dragDropPiece = eventData.pointerDrag.GetComponent<DragDropPiece>();
            DragDropPiece child = transform.childCount > 0 ? transform.GetChild(0).GetComponent<DragDropPiece>() : null;

            foreach (Cell cell in dragDropPiece.getMovements())
            {
                if (cell == this && (transform.childCount == 0 || (child != null && child.getWhite() != dragDropPiece.getWhite())))
                {
                    SoundEmitter soundEmitter = SoundEmitter.Instance();
                    if (soundEmitter != null)
                    {
                        soundEmitter.PlaySFX(_moveSound);
                    }

                    _gameManager.ToogleTurn();
                    _gameManager.resetTaken();

                    eventData.pointerDrag.transform.SetParent(transform);
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

                    if (child != null)
                    {
                        child.GetCaptured();
                    }

                    //promote
                    if ((getGridPosition().y == 0 && !dragDropPiece.getWhite()) || (getGridPosition().y == 7 && dragDropPiece.getWhite()))
                    {
                        dragDropPiece.Promote();
                    }
                }

                if (child != null && cell == this && child.getWhite() == dragDropPiece.getWhite() && _isJail && child.GetSalvable())
                {
                    child.Save(this);
                    dragDropPiece.Save(this);
                }
            }

            _gridGenerator.Deactivate();
        }
    }

    public DragDropPiece GetPiece()
    {
        if (!hasPiece())
        {
            throw new Exception("No piece");
        }
        return transform.GetChild(0).GetComponent<DragDropPiece>();
    }

    public bool hasPiece()
    {
        return (transform.childCount != 0);
    }

    public bool hasOponent(bool isWhite)
    {
        if (transform.childCount == 0) return false;
        return transform.GetChild(0).GetComponent<DragDropPiece>().getWhite() != isWhite;
    }

    public Vector2 getGridPosition()
    {
        return _position;
    }

    public void setGridPosition(Vector2 position)
    {
        _position = position;
    }

    internal void Activate()
    {
        if (transform.childCount != 0)
        {
            _image.color = Color.green;
        }
        else
        {
            _image.color = Color.black;
        }
    }

    internal void Deactivate()
    {
        _image.color = _color;
    }
}
