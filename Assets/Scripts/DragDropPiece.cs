using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class DragDropPiece : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    protected RectTransform _rectTransform;
    protected Canvas _canvas;
    protected CanvasGroup _canvasGroup;
    protected Transform _oldParent;
    protected Transform _outParent;
    protected GridGenerator _gridGenerator;
    protected GameManager _gameManager;
    protected Cell _cell=null;
    protected bool _isWhite = true;
    protected Image _image;

    public void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _gridGenerator = FindObjectOfType<GridGenerator>();
        _image = GetComponent<Image>();
        _outParent = GameObject.Find("OutOfGrid").transform;
        _gameManager = FindObjectOfType<GameManager>();
    }

    public  virtual void SetWhite(bool isWhite)
    {
        _isWhite = isWhite;
    }

    public bool getWhite()
    {
        return _isWhite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_gameManager.getWhiteTurn(_isWhite))
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

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        if (transform.parent == _outParent)
        {
            transform.SetParent(_oldParent);
            GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        _gridGenerator.Deactivate();

    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public abstract List<Cell> getMovements();

    public void ActivateCells()
    {
        _gridGenerator.Deactivate();
        List<Cell> cells = getMovements();
        foreach (Cell cell in cells)
        {
            if (cell)
            {
                cell.Activate();
            }
        }
    }

    public virtual void GetCaptured()
    {
        _gameManager.setTaken(_isWhite);
    }

    public virtual void Promote()
    {

    }
    
}
