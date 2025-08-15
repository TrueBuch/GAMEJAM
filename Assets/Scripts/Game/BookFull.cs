using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BookFull : MonoBehaviour
{
    [SerializeField] private AudioClip _open;
    [SerializeField] private AudioClip _close;
    [SerializeField] private AudioSource _source;
    [SerializeField] private Button _left;
    [SerializeField] private Button _right;
    private Image _image;

    [SerializeField] private int page = 0;
    [SerializeField] private Book _book;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private List<Sprite> _defaultPages;
    [SerializeField] private List<Sprite> _nnmPages;

    private Vector3 _startedPosition;
    public Vector3 StartedPosition => _startedPosition;

    private bool _isOpened;

    private void Awake()
    {
        gameObject.SetActive(false);
        _image = GetComponent<Image>();
        _startedPosition = _canvas.transform.localPosition;

        _left.onClick.AddListener(OnLeftButtonClicked);
        _right.onClick.AddListener(OnRightButtonClicked);
    }

    private void Start()
    {
        Main.Get<Input>().RightClickStarted.AddListener(OnRightClickStarted);
        UpdateView();
    }
    public void OnClicked()
    {
        _isOpened = true;
        _source.PlayOneShot(_open);
        _image.sprite = _book.IsNormal ? _defaultPages[page] : _nnmPages[page];
        _canvas.transform.DOLocalMove(Vector3.zero, 0.5f);
        var events = Main.EventSystem.FindAll<IOnPageChanged>();
        foreach (var e in events) StartCoroutine(e.OnChanged(page));
    }

    private void OnLeftButtonClicked()
    {
        page = Math.Max(0, page - 1);
        UpdateView();

        var events = Main.EventSystem.FindAll<IOnPageChanged>();
        foreach (var e in events) StartCoroutine(e.OnChanged(page));
    }

    private void OnRightButtonClicked()
    {
        page = Math.Min(page + 1, _book.IsNormal ? _defaultPages.Count - 1 : _nnmPages.Count - 1);
        UpdateView();

        var events = Main.EventSystem.FindAll<IOnPageChanged>();
        foreach (var e in events) StartCoroutine(e.OnChanged(page));
    }

    public void UpdateView()
    {

        page = Math.Min(page, _book.IsNormal ? _defaultPages.Count - 1 : _nnmPages.Count - 1);
        _image.sprite = _book.IsNormal ? _defaultPages[page] : _nnmPages[page];
        _left.gameObject.SetActive(page != 0);

        _right.gameObject.SetActive(_book.IsNormal ? page != _defaultPages.Count - 1 : page != _nnmPages.Count - 1);

    }

    public void OnRightClickStarted()
    {
        if (!_isOpened) return;
        _source.PlayOneShot(_close);
        _canvas.transform.DOLocalMove(_startedPosition, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _book.Button.gameObject.SetActive(true);
            _isOpened = false;
        });
    }
}

public interface IOnPageChanged { public IEnumerator OnChanged(int index); }