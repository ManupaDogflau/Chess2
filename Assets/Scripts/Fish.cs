using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : DragDropPiece
{
    public override List<Cell> getMovements()
    {
        List<Cell> cells = new List<Cell>();
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, 1), 1, MovementEnum.MOVE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 0), 1, MovementEnum.MOVE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 0), 1, MovementEnum.MOVE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        return cells;
    }

    public override void SetWhite(bool isWhite)
    {
        base.SetWhite(isWhite);
        if (_isWhite)
        {
            _image.color = Color.cyan;
        }
        else
        {
            _image.color = Color.red;
        }
    }

    public override void GetCaptured()
    {
        base.GetCaptured();
        Destroy(gameObject);
    }
}
