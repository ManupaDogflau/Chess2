using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject _cell;
    private GameObject _fish;
    private Vector3 _firstPosition;
    private Dictionary<Vector2, Cell> _grid = new Dictionary<Vector2, Cell>();

    private void Awake()
    {
        _cell = Resources.Load<GameObject>("Cell");
        _fish = Resources.Load<GameObject>("Fish");
        _firstPosition = transform.GetChild(0).position;
    }

    internal List<Cell> GetCells(Cell cell, Vector2 vector, int quantity, MovementEnum type, bool isWhite)
    {
        List<Cell> cells = new List<Cell>();
        Vector2 vector_ = isWhite ? vector : -vector;

        switch (type)
        {
            case MovementEnum.JUMP:
                for (int i = 1; i <= quantity; i++)
                {
                    if (_grid.TryGetValue(cell.getGridPosition() + vector_ * i, out Cell cell_))
                    {
                        cells.Add(cell_);
                    }
                }
                break;

            case MovementEnum.MOVE:
                for (int i = 1; i <= quantity; i++)
                {
                    Vector2 position = cell.getGridPosition() + vector_ * i;
                    if (_grid.TryGetValue(position, out Cell cell_) && !cell_.hasPiece())
                    {
                        cells.Add(cell_);
                    }
                    else
                    {
                        break;
                    }
                }
                break;
            case MovementEnum.MOVEANDCAPTURE:
                for (int i = 1; i <= quantity; i++)
                {
                    Vector2 position = cell.getGridPosition() + vector_ * i;
                    if (_grid.TryGetValue(position, out Cell cell_) && !cell_.hasPiece())
                    {
                        cells.Add(cell_);
                    }
                    else
                    {
                        if (_grid.TryGetValue(position, out  cell_) && cell_.hasOponent(isWhite))
                        {
                            cells.Add(cell_);
                        }
                        break;
                    }
                }
                break;
            case MovementEnum.CAPTURE:
                for (int i = 1; i <= quantity; i++)
                {
                    Vector2 position = cell.getGridPosition() + vector_ * i;
                    if (_grid.TryGetValue(position, out Cell cell_) && cell_.hasOponent(isWhite))
                    {
                        cells.Add(cell_);
                        break;
                    }
                    else
                    {
                        if (cell_.hasPiece())
                        {
                            break;
                        }
                    }
                }
                break;
            case MovementEnum.MONKEY:
                cells.AddRange(GetCellsMonkey(cell, isWhite));
                break;
            case MovementEnum.ELEPHANT:
                cells.AddRange(GetCellsElephant(cell, isWhite));
                break;


        }

        return cells;
    }

    private List<Cell> GetCellsMonkey(Cell cell, bool isWhite)
    {
        Queue<Cell> queue = new Queue<Cell>();
        List<Cell> list = new List<Cell>();
        HashSet<Cell> visited = new HashSet<Cell>(); 
        queue.Enqueue(cell);
        int i = 0;
        while (queue.Count != 0)
        {
            Cell currentCell = queue.Dequeue();
            visited.Add(currentCell);
            Vector2 vector = currentCell.getGridPosition();

            queue = ProcessAdjacentCell(vector , new Vector2(1, 0), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector , new Vector2(-1, 0), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector , new Vector2(0, 1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector , new Vector2(0, -1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(1, 1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, 1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(1, -1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, -1), isWhite, queue, list, visited);

        }
        return list;
    }

    private Queue<Cell> ProcessAdjacentCell(Vector2 position,Vector2 step, bool isWhite, Queue<Cell> queue, List<Cell> list, HashSet<Cell> visited)
    {
        if (_grid.TryGetValue(position+step, out Cell medium) && medium.hasPiece())
        {
            Vector2 endPosition = position + 2 * step;
            if (_grid.TryGetValue(endPosition, out Cell end))
            {
                if (!end.hasPiece())
                {
                    if (!visited.Contains(end))
                    {
                        queue.Enqueue(end);
                    }
                        list.Add(end);

                }
                else if (end.hasOponent(isWhite))
                {
                    list.Add(end);
                }
            }
        }
        return queue;
    }

    private List<Cell> GetCellsElephant(Cell cell, bool isWhite)
    {
        Vector2 cellPosition = cell.getGridPosition();
        List<Cell> list = new List<Cell>();

        // Celda diagonal superior derecha
        if (TryGetCellAtPosition(cellPosition + new Vector2(1, 1), out Cell medium) && TryGetCellAtPosition(cellPosition + new Vector2(2, 2), out Cell end))
        {
            if (!medium.hasPiece() && (!end.hasPiece() || end.hasOponent(isWhite)))
            {
                list.Add(end);
            }
        }

        // Celda diagonal superior izquierda
        if (TryGetCellAtPosition(cellPosition + new Vector2(-1, 1), out medium) && TryGetCellAtPosition(cellPosition + new Vector2(-2, 2), out end))
        {
            if (!medium.hasPiece() && (!end.hasPiece() || end.hasOponent(isWhite)))
            {
                list.Add(end);
            }
        }

        // Celda diagonal inferior derecha
        if (TryGetCellAtPosition(cellPosition + new Vector2(1, -1), out medium) && TryGetCellAtPosition(cellPosition + new Vector2(2, -2), out end))
        {
            if (!medium.hasPiece() && (!end.hasPiece() || end.hasOponent(isWhite)))
            {
                list.Add(end);
            }
        }

        // Celda diagonal inferior izquierda
        if (TryGetCellAtPosition(cellPosition + new Vector2(-1, -1), out medium) && TryGetCellAtPosition(cellPosition + new Vector2(-2, -2), out end))
        {          
            if (!medium.hasPiece() && (!end.hasPiece() || end.hasOponent(isWhite)))
            {
                list.Add(end);
            }
        }
        return list;
    }

    private bool TryGetCellAtPosition(Vector2 position, out Cell cell)
    {
        if (_grid.TryGetValue(position, out cell))
        {
            return true;
        }
        cell = null;
        return false;
    }


    void Start()
    {
       
       
        for(int i=0; i<8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject cell=Instantiate(_cell, _firstPosition + new Vector3(i * 100, -j * 100, 0), Quaternion.identity, transform);
                if ((i+j)%2!=0)
                {
                    cell.GetComponent<Image>().color = Color.blue;
                }
                Cell cell_ = cell.GetComponent<Cell>();
                cell_.setGridPosition(new Vector2(i, j));
                _grid.Add(new Vector2(i, j), cell_);
                if (j == 1 || j==6)
                {
                    GameObject piece = Instantiate(_fish, cell.transform);
                    DragDropPiece piece_ = piece.GetComponent<DragDropPiece>();
                    if (j == 6)
                    {
                        piece_.SetWhite(false);
                    }
                    else
                    {
                        piece_.SetWhite(true);
                    }
                }
            }
        }
    }

   public void Deactivate()
    {
        foreach(Cell cell in _grid.Values)
        {
            cell.Deactivate();
        }
    }

    public List<Cell> getAllEmptyCells()
    {
        List<Cell> cells = new List<Cell>();
        foreach (Cell cell in _grid.Values)
        {
            if (!cell.hasPiece())
            {
                cells.Add(cell);
            }
        }
        return cells;
    }
}
