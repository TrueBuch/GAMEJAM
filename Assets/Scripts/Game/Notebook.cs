using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Notebook : MonoBehaviour, ISingleton, ISelectable
{
    public bool notebook = false;
    [SerializeField] private Image _pencil2;
    [SerializeField] private Image _pencil;
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private float _offset;
    private Vector3 _targetPosition;
    private Vector3 _startedPosition;
    private Vector3 _startedPencil2Positon;

    private bool _isHovering;

    private void Awake()
    {
        _pencil2.gameObject.SetActive(false);
        _startedPosition = transform.localPosition;
        _targetPosition = _startedPosition;
        _targetPosition.y += _offset;
        _startedPencil2Positon = _pencil2.transform.localPosition;
        _input.caretPosition = _input.text.Length;
    }
    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;

        var events = Main.EventSystem.FindAll<IOnNotebookUp>();
        foreach (var e in events) StartCoroutine(e.OnUp());
        _input.ActivateInputField();
        transform.DOLocalMove(_targetPosition, 0.25f).OnComplete(() => { });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;

        var events = Main.EventSystem.FindAll<IOnNotebookDown>();
        foreach (var e in events) StartCoroutine(e.OnDown());
        
        _pencil2.gameObject.SetActive(false);
        _pencil.gameObject.SetActive(true);
        _input.DeactivateInputField();
        transform.DOLocalMove(_startedPosition, 0.25f);
    }

    private void Update()
    {
        if (!_isHovering) return;

        int caretPos = _input.caretPosition;

        TMP_Text text = _input.textComponent;
        if (text.text.Length > 1 && caretPos > 0)
        {
            _pencil.gameObject.SetActive(false);
            _pencil2.gameObject.SetActive(true);

            var charInfo = text.textInfo.characterInfo[caretPos - 1];
            var charPos = charInfo.bottomRight;
            charPos.x += 2.5f;
            _pencil2.transform.localPosition = charPos;
        }
        else
        {
            _pencil2.gameObject.SetActive(false);
            _pencil.gameObject.SetActive(true);
        }
    }

    public void AddText(string text)
    {
        _input.text += text;
    }

    public void OnPointerUp(PointerEventData eventData) { }

    public void Initialize() { }
}

public interface IOnNotebookUp
{
    public IEnumerator OnUp();
}

public interface IOnNotebookDown
{
    public IEnumerator OnDown();
}