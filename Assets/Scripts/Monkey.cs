using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monkey : DragDropPiece
{
    private Cell _save_cell;


    public override List<Cell> getMovements()
    {
        if (!_gameManager.getSave(_isWhite))
        {
            List<Cell> cells = new List<Cell>();
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 0), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 0), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, -1), 1, MovementEnum.MONKEY, _isWhite));
        cells.AddRange(_gridGenerator.CheckKingSaving(_isWhite,cells));
        while (cells.Contains(_cell))
        {
            cells.Remove(_cell);
        }

        cells = new List<Cell>(new HashSet<Cell>(cells));

        return cells;
        }
        else
        {
            return _gridGenerator.GetMonkeyReturnCells(_isWhite,_save_cell);
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (_gameManager.getWhiteTurn(_isWhite) && !_gameManager.getCaptured() && !_gameManager.getSave(!_isWhite))
        {
            _cell = GetComponentInParent<Cell>();
            this.ActivateCells();
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

    public override void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        _gridGenerator.Deactivate();
        if (_gameManager.getSave(_isWhite))
        {
            return;
        }
        if (transform.parent == _outParent)
        {
            transform.SetParent(_oldParent);
            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }


    }

    public override void GetCaptured()
    {
        base.GetCaptured();
        Destroy(gameObject);
        
    }

    public override void Save(Cell cell)
    {
        base.Save(cell);
        Vector2 vector = cell.getGridPosition();
        if (vector.x < 0)
        {
            vector = vector + new Vector2(1, 0);
        }
        else
        {
            vector = vector - new Vector2(1, 0);
        }
        cell = _gridGenerator.GetCell(vector);
        print(cell.getGridPosition());
        transform.SetParent(_outParent);
        _rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        _save_cell = cell;
    }

}
