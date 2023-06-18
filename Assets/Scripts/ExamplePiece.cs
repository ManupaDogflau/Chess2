using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePiece : DragDropPiece
{

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

    public override List<Cell> getMovements()
    {
        return _gridGenerator.GetCells(_cell, new Vector2(0, 1),2,MovementEnum.MOVEANDCAPTURE, _isWhite);
    }


}
