using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazetaFull : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Gazeta _gazeta;
    [SerializeField] private Canvas _canvas;
    private Vector3 _startedPosition;
    public Vector3 StartedPosition => _startedPosition;
    private bool _isOpened;

    [SerializeField] private Image _image;


    private void Awake()
    {
        gameObject.SetActive(false);
        _startedPosition = _canvas.transform.localPosition;
    }

    private void Start()
    {
        Main.Get<Input>().RightClickStarted.AddListener(OnRightClickStarted);
    }
    public void OnClicked()
    {
        _isOpened = true;
        _canvas.transform.DOLocalMove(Vector3.zero, 0.5f);
    }
    public void OnRightClickStarted()
    {
        if (!_isOpened) return;
        _canvas.transform.DOLocalMove(_startedPosition, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _gazeta.Button.gameObject.SetActive(true);
            _isOpened = false;
        });
    }
    
    public void ChangeSprite(int index)
    {
        _image.sprite = _sprites[index];
    }
}