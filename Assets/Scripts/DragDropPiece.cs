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
    protected Cell _cell = null;
    protected bool _isWhite = true;
    protected Image _image;
    protected bool _salvable = false;
    protected AudioClip _capturedAudio;
    [SerializeField] protected Sprite _whiteSprite;
    [SerializeField] protected Sprite _blackSprite;

    public virtual void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _gridGenerator = FindObjectOfType<GridGenerator>();
        _image = GetComponent<Image>();
        _outParent = GameObject.Find("OutOfGrid").transform;
        _gameManager = FindObjectOfType<GameManager>();
        _capturedAudio = Resources.Load<AudioClip>("Punch");
 
        if (!_isWhite)
        {
            _image.sprite = _whiteSprite;
        }
        else
        {
            _image.sprite = _blackSprite;
        }
    }

    public virtual void SetWhite(bool isWhite)
    {
        _isWhite = isWhite;
        if (!_isWhite)
        {
            _image.sprite = _whiteSprite;
        }
        else
        {
            _image.sprite = _blackSprite;
        }
    }

    public virtual bool getWhite()
    {
        return _isWhite;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (_gameManager.getWhiteTurn(_isWhite) && !_gameManager.getCaptured() && !_gameManager.getSave())
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

    public virtual void OnEndDrag(PointerEventData eventData)
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
        SoundEmitter.Instance().PlaySFX(_capturedAudio);
        _gameManager.setTaken(_isWhite);
    }

    public virtual void Promote()
    {

    }

    public bool GetSalvable()
    {
        return _salvable;
    }

    public void SetSalvable(bool salvable)
    {
        _salvable = salvable;
    }

    public virtual void Save(Cell cell)
    {

    }

}
