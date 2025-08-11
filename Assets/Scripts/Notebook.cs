using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Notebook : MonoBehaviour, ISelectable
{
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
    }
    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;

        _input.ActivateInputField();
        transform.DOLocalMove(_targetPosition, 1f).OnComplete(() =>
        {
            
        });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;

        _pencil2.gameObject.SetActive(false);
        _pencil.gameObject.SetActive(true);
        _input.DeactivateInputField();
        transform.DOLocalMove(_startedPosition, 1f);
    }

    private void Update()
    {
        if (!_isHovering) return;

        int caretPos = _input.caretPosition;
        TMP_Text text = _input.textComponent;

        if (text.text.Length > 1)
        {
            _pencil.gameObject.SetActive(false);
            _pencil2.gameObject.SetActive(true);

            var charInfo = text.textInfo.characterInfo[caretPos];
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

    public void OnPointerUp(PointerEventData eventData) { }
}