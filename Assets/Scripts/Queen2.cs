using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Queen2 : DragDropPiece
{
    private bool _captured = false;
    public override List<Cell> getMovements()
    {
        if (!_gameManager.getCaptured(_isWhite))
        {
            List<Cell> cells = new List<Cell>();
            if (!_captured)

            {

                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, 1), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));
                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 0), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));
                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 0), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));
                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 1), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));
                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 1), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));
                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, -1), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));
                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, -1), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));
                cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, -1), 8, MovementEnum.MOVEANDCAPTURE, _isWhite));


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
    public override void GetCaptured()
    {
        base.GetCaptured();
        _captured = true;
        _gameManager.setCaptured(_isWhite);
        transform.SetParent(_outParent);
        _rectTransform.anchoredPosition = new Vector3(0, 0, 0);
    }
}
