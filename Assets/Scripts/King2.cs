using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King2 : DragDropPiece
{
    public override List<Cell> getMovements()
    {
        List<Cell> cells = new List<Cell>();
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 0), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 0), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));
        cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, -1), 1, MovementEnum.MOVEANDCAPTURE, _isWhite));

        return cells;
    }

    //TO-DO
    public override void GetCaptured()
    {
        base.GetCaptured();
    }
}
