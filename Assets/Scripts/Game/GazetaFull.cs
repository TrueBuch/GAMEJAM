using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GazetaFull : MonoBehaviour
{
    [SerializeField] private AudioClip _sound;
    [SerializeField] private AudioSource _source;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Gazeta _gazeta;
    [SerializeField] private Canvas _canvas;
    private Vector3 _startedPosition;
    public Vector3 StartedPosition => _startedPosition;
    private bool _isOpened;

    private int _index;
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
        _source.PlayOneShot(_sound);
        _canvas.transform.DOLocalMove(Vector3.zero, 0.5f);
        var events = Main.EventSystem.FindAll<IOnGazetaOpened>();
        foreach (var e in events) _gazeta.StartCoroutine(e.OnOpened(_index));
    }
    public void OnRightClickStarted()
    {
        if (!_isOpened) return;
        _source.PlayOneShot(_sound);
        _canvas.transform.DOLocalMove(_startedPosition, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _gazeta.Button.gameObject.SetActive(true);
            _isOpened = false;
        });
    }

    public void Change(int index)
    {
        _index = index;
        _image.sprite = _sprites[index];
    }
}

public interface IOnGazetaOpened
{
    public IEnumerator OnOpened(int index);
}