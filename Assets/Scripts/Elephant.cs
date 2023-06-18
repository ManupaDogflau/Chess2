using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : DragDropPiece
{
    public override List<Cell> getMovements()
    {
        return _gridGenerator.GetCells(_cell, Vector2.zero, 0, MovementEnum.ELEPHANT, _isWhite);
    }

    public override void GetCaptured()
    {
        base.GetCaptured();
        Destroy(gameObject);
    }
}
