using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook2 : DragDropPiece
{
    public override void GetCaptured()
    {
        base.GetCaptured();
        Destroy(gameObject);
    }

    public override List<Cell> getMovements()
    {
        List<Cell> cells= _gridGenerator.getAllEmptyCells();
        if (_gameManager.getTaken(_isWhite))
        {
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, 1), 1, MovementEnum.CAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, -1), 1, MovementEnum.CAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 0), 1, MovementEnum.CAPTURE, _isWhite));
            cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 0), 1, MovementEnum.CAPTURE, _isWhite));
        }
        cells.Remove(_cell);
        cells = new List<Cell>(new HashSet<Cell>(cells));
        return cells;
    }
}
