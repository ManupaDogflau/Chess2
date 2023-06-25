using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : DragDropPiece
{
    protected AudioClip _promoteAudio;

    public override void Awake()
    {
        base.Awake();
        _promoteAudio = Resources.Load<AudioClip>("Promote");
    }
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


    public override void GetCaptured()
    {
        base.GetCaptured();
        Destroy(gameObject);
    }

    public override void Promote()
    {
        base.Promote();
        SoundEmitter.Instance().PlaySFX(_promoteAudio);
        _gridGenerator.Deactivate();
        FishyQueen queen = Instantiate(Resources.Load<GameObject>("FishyQueen"), transform.position, Quaternion.identity, transform.parent).GetComponent<FishyQueen>();
        queen.SetWhite(_isWhite);
        Destroy(gameObject);
    }
}
