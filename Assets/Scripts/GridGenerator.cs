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
    private GameObject _king;
    private GameObject _rook;
    private GameObject _queen;
    private GameObject _monkey;
    private GameObject _elephant;
    private Vector3 _firstPosition;
    private Canvas _canvas;
    private Dictionary<Vector2, Cell> _grid = new Dictionary<Vector2, Cell>();
    private List<Cell> _jail = new List<Cell>();

    private void Awake()
    {
        _cell = Resources.Load<GameObject>("Cell");
        _fish = Resources.Load<GameObject>("Fish");
        _king = Resources.Load<GameObject>("King");
        _queen = Resources.Load<GameObject>("Queen");
        _monkey = Resources.Load<GameObject>("Monkey");
        _elephant = Resources.Load<GameObject>("Elephant");
        _rook = Resources.Load<GameObject>("Rook");
        _firstPosition = new Vector3(601.0f, 889.0f,0.0f);
        _canvas = FindObjectOfType<Canvas>();
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
                        if (_grid.TryGetValue(position, out cell_) && cell_.hasOponent(isWhite))
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
                        if (_grid.TryGetValue(position, out cell_) && cell_.hasPiece())
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
            case MovementEnum.BEAR:
                cells.AddRange(GetCellsBear());
                break;


        }

        return cells;
    }

    internal List<Cell> GetMonkeyReturnCells(bool isWhite, Cell cell)
    {
        Queue<Cell> queue = new Queue<Cell>();
        HashSet<Cell> visited = new HashSet<Cell>();

        Vector2 vector = cell.getGridPosition();

        List<Cell> finalList = new List<Cell>();

        queue = ProcessAdjacentCell(vector, new Vector2(1, 0), isWhite, queue, finalList, visited);
        queue = ProcessAdjacentCell(vector, new Vector2(-1, 0), isWhite, queue, finalList, visited);
        queue = ProcessAdjacentCell(vector, new Vector2(0, 1), isWhite, queue, finalList, visited);
        queue = ProcessAdjacentCell(vector, new Vector2(0, -1), isWhite, queue, finalList, visited);
        queue = ProcessAdjacentCell(vector, new Vector2(1, 1), isWhite, queue, finalList, visited);
        queue = ProcessAdjacentCell(vector, new Vector2(-1, 1), isWhite, queue, finalList, visited);
        queue = ProcessAdjacentCell(vector, new Vector2(1, -1), isWhite, queue, finalList, visited);
        queue = ProcessAdjacentCell(vector, new Vector2(-1, -1), isWhite, queue, finalList, visited);

        foreach (Cell aux in finalList)
        {
            print(aux.getGridPosition());
        }

        return finalList;
    }

    internal List<Cell> GetJail(bool isWhite)
    {
        if (isWhite)
        {
            List<Cell> cell = _jail.GetRange(0, 1);
            cell.AddRange(_jail.GetRange(2, 1));
            return cell;
        }
        else
        {
            List<Cell> cell = _jail.GetRange(1, 1);
            cell.AddRange(_jail.GetRange(3, 1));
            return cell;

        }
    }

    private IEnumerable<Cell> GetCellsBear()
    {
        List<Cell> cells = new List<Cell>();
        Cell cell;
        if (_grid.TryGetValue(new Vector2(3, 3), out cell)) cells.Add(cell);
        if (_grid.TryGetValue(new Vector2(3, 4), out cell)) cells.Add(cell);
        if (_grid.TryGetValue(new Vector2(4, 3), out cell)) cells.Add(cell);
        if (_grid.TryGetValue(new Vector2(4, 4), out cell)) cells.Add(cell);
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

            queue = ProcessAdjacentCell(vector, new Vector2(1, 0), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, 0), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(0, 1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(0, -1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(1, 1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, 1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(1, -1), isWhite, queue, list, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, -1), isWhite, queue, list, visited);

        }
        return list;
    }

    public List<Cell> CheckKingSaving(bool isWhite, List<Cell> list)
    {
        List<Cell> lista = new List<Cell>();
        Cell cell_;
        if (isWhite)
        {
            if (_grid.TryGetValue(new Vector2(7, 3), out cell_))
            {
                if (list.Contains(cell_) && !cell_.hasOponent(isWhite))
                {
                    try
                    {

                        if (_jail.ToArray()[0].GetPiece().GetSalvable())
                        {
                            lista.Add(_jail.ToArray()[0]);
                        }
                    }
                    catch { }
                }
            }
            if (_grid.TryGetValue(new Vector2(7, 4), out cell_))
            {
                if (list.Contains(cell_) && !cell_.hasOponent(isWhite))
                {
                    try
                    {
                        if (_jail.ToArray()[2].GetPiece().GetSalvable())
                        {
                            lista.Add(_jail.ToArray()[2]);
                        }
                    }
                    catch { }
                }
            }

        }
        else
        {
            if (_grid.TryGetValue(new Vector2(0, 3), out cell_))
            {
                if (list.Contains(cell_) && !cell_.hasOponent(isWhite))
                {
                    try
                    {
                        if (_jail.ToArray()[1].GetPiece().GetSalvable())
                        {
                            lista.Add(_jail.ToArray()[1]);
                        }
                    }
                    catch { }
                }
            }
            if (_grid.TryGetValue(new Vector2(0, 4), out cell_))
            {
                if (list.Contains(cell_) && !cell_.hasOponent(isWhite))
                {
                    try
                    {
                        if (_jail.ToArray()[3].GetPiece().GetSalvable())
                        {
                            lista.Add(_jail.ToArray()[3]);
                        }
                    }
                    catch { }
                }
            }
        }
        List<Cell> eliminate = new List<Cell>();
        foreach (Cell aux in lista)
        {
            Queue<Cell> queue = new Queue<Cell>();
            HashSet<Cell> visited = new HashSet<Cell>();

            Vector2 vectoraux = aux.getGridPosition();
            Vector2 vector;
            if (vectoraux.y < 0)
            {
                vector = vectoraux + new Vector2(0, 1);
            }
            else
            {
                vector = vectoraux - new Vector2(0, 1);
            }
            List<Cell> finalList = new List<Cell>();

            queue = ProcessAdjacentCell(vector, new Vector2(1, 0), isWhite, queue, finalList, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, 0), isWhite, queue, finalList, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(0, 1), isWhite, queue, finalList, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(0, -1), isWhite, queue, finalList, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(1, 1), isWhite, queue, finalList, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, 1), isWhite, queue, finalList, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(1, -1), isWhite, queue, finalList, visited);
            queue = ProcessAdjacentCell(vector, new Vector2(-1, -1), isWhite, queue, finalList, visited);
            if (finalList.Count == 0)
            {
                eliminate.Add(aux);
            }
        }
        foreach (Cell cell in eliminate)
        {
            while (lista.Contains(cell))
            {
                lista.Remove(cell);
            }
        }
        return lista;
    }

    private Queue<Cell> ProcessAdjacentCell(Vector2 position, Vector2 step, bool isWhite, Queue<Cell> queue, List<Cell> list, HashSet<Cell> visited)
    {
        if (_grid.TryGetValue(position + step, out Cell medium) && medium.hasPiece())
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
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        print(_firstPosition);
        for (int i = 0; i < 8; i++)
        {
            if (i == 4 || i == 3)
            {
                GameObject cell = Instantiate(_cell, _firstPosition * _canvas.scaleFactor + new Vector3(8 * 100 * _canvas.scaleFactor, -i * 100 * _canvas.scaleFactor, 0), Quaternion.identity, transform);
                if ((i + 8) % 2 != 0)
                {
                    cell.GetComponent<Image>().color = Color.blue;
                }
                Cell cell_ = cell.GetComponent<Cell>();
                cell_.SetJail();
                cell_.setGridPosition(new Vector2(8, i));
                _jail.Add(cell_);
                cell = Instantiate(_cell, _firstPosition * _canvas.scaleFactor + new Vector3(-1 * 100 * _canvas.scaleFactor , -i * 100 * _canvas.scaleFactor, 0), Quaternion.identity, transform);
                if ((i + -1) % 2 != 0)
                {
                    cell.GetComponent<Image>().color = Color.blue;
                }
                cell_ = cell.GetComponent<Cell>();
                cell_.setGridPosition(new Vector2(-1, i));
                cell_.SetJail();
                _jail.Add(cell_);
            }
            for (int j = 0; j < 8; j++)
            {
                GameObject cell = Instantiate(_cell, _firstPosition * _canvas.scaleFactor + new Vector3(i * 100 * _canvas.scaleFactor, -j * 100 * _canvas.scaleFactor, 0), Quaternion.identity, transform);
                if ((i + j) % 2 != 0)
                {
                    cell.GetComponent<Image>().color = Color.blue;
                }
                Cell cell_ = cell.GetComponent<Cell>();
                cell_.setGridPosition(new Vector2(i, j));
                _grid.Add(new Vector2(i, j), cell_);
                GameObject piece = null;
                if (j== 0 || j == 7)
                {
                    switch (i)
                    {
                        case 0:
                            piece = Instantiate(_rook, cell.transform);
                            break;
                        case 1:
                            piece = Instantiate(_monkey, cell.transform);
                            break;
                        case 2:
                            piece = Instantiate(_fish, cell.transform);
                            break;
                        case 3:
                            if (j == 0)
                            {
                                piece = Instantiate(_king, cell.transform);
                            }
                            else
                            {
                                piece = Instantiate(_queen, cell.transform);
                            }
                            break;
                        case 4:
                            if (j == 0)
                            {
                                piece = Instantiate(_queen, cell.transform);
                            }
                            else
                            {
                                piece = Instantiate(_king, cell.transform);
                            }
                            break;
                        case 5:
                            piece = Instantiate(_fish, cell.transform);
                            break;
                        case 6:
                            piece = Instantiate(_monkey, cell.transform);
                            break;
                        case 7:
                            piece = Instantiate(_rook, cell.transform);
                            break;
                    }
                        
                    
                    DragDropPiece piece_ = piece.GetComponent<DragDropPiece>();
                    if (j == 7)
                    {
                        piece_.SetWhite(false);
                    }
                    else
                    {
                        piece_.SetWhite(true);
                    }
                }
                if (j == 1 || j == 6)
                {
                   
                    if (i==2 || i == 5)
                    {
                        piece = Instantiate(_elephant, cell.transform);
                    }
                    else
                    {
                        piece = Instantiate(_fish, cell.transform);
                    }
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
        foreach (Cell cell in _grid.Values)
        {
            cell.Deactivate();
        }
        foreach (Cell cell in _jail)
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
    
    public Cell GetCell(Vector2 vector)
    {
        Cell cell;
        _grid.TryGetValue(vector, out cell);
        return cell;
    }

}