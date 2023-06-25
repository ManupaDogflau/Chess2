using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class King2 : DragDropPiece
{

    private bool _captured = false;
    [SerializeField] private Sprite _whiteImg;
    [SerializeField] private Sprite _blackImg;

    public override void Awake()
    {
        base.Awake();
        _salvable = true;
    }

    public override List<Cell> getMovements()
    {
        if (!_gameManager.getCaptured(_isWhite))
        {
            List<Cell> cells = new List<Cell>();
            if (!_captured)
 
            {
            
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 0), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 0), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));

           
            }
            return cells;
        }
        else
        {
            return _gridGenerator.GetJail(_isWhite);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (_gameManager.getWhiteTurn(_isWhite) && !_gameManager.getCaptured(!_isWhite) && !_gameManager.getSave())
        {
            _cell = GetComponentInParent<Cell>();
            ActivateCells();
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0.6f;
            _oldParent = transform.parent;
            transform.SetParent(_outParent);
        }
        else
        {
            _oldParent = transform.parent;
            transform.SetParent(_outParent);
        }
    }
    public override void GetCaptured()
    {
        base.GetCaptured();
        foreach (Cell cell in _gridGenerator.GetJail(_isWhite))
        {
            if (cell.hasPiece())
            {
                _gameManager.EndGame(_isWhite);
            }
        }
        _captured = true;
        _gameManager.setCaptured(_isWhite);
        transform.SetParent(_outParent);
        _rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }

    public override void Save(Cell cell)
    {
        base.Save(cell);
        _salvable = false;
        _gameManager.setSave(_isWhite);
        _blackSprite = _blackImg;
        _whiteSprite = _whiteImg;
        if (!_isWhite)
        {
            _image.sprite = _whiteImg;
        }
        else
        {
            _image.sprite = _blackImg;
        }
        Cell cell_=transform.parent.gameObject.GetComponent<Cell>();
        Vector2 vector = cell.getGridPosition();
        if (vector.x < 0)
        {
            vector = vector + new Vector2(1, 0);
        }
        else
        {
            vector = vector - new Vector2(1, 0);
        }
        cell_ = _gridGenerator.GetCell(vector);
        transform.SetParent(cell_.transform);
        _rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }
}
