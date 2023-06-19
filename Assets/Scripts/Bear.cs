using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bear : DragDropPiece
{
	public override List<Cell> getMovements()
	{
		if (_cell is null)
		{
			return _gridGenerator.GetCells(_cell, Vector2.zero, 0, MovementEnum.BEAR, false);
		}
		else
		{
			List<Cell> cells = new List<Cell>();
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, 1), 1, MovementEnum.MOVE, _isWhite));
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 0), 1, MovementEnum.MOVE, _isWhite));
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 0), 1, MovementEnum.MOVE, _isWhite));
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, 1), 1, MovementEnum.MOVE, _isWhite));
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, 1), 1, MovementEnum.MOVE, _isWhite));
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(0, -1), 1, MovementEnum.MOVE, _isWhite));
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(-1, -1), 1, MovementEnum.MOVE, _isWhite));
			cells.AddRange(_gridGenerator.GetCells(_cell, new Vector2(1, -1), 1, MovementEnum.MOVE, _isWhite));
			return cells;
		}
	}
	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (_gameManager.getWhiteTurn(!getWhite()))
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
	public override bool getWhite()
	{
		return _gameManager.getWhiteTurn(false);
	}

	public override void GetCaptured()
	{
		base.GetCaptured();
		Destroy(gameObject);
	}


}
