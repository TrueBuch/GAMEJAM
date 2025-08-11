using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Notebook : MonoBehaviour, ISelectable
{
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Vector3 _targetPosition;
    private Vector3 _startedPosition;

    private void Awake()
    {
        _startedPosition = transform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _input.ActivateInputField();
        transform.DOLocalMove(_targetPosition, 1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _input.DeactivateInputField();
        transform.DOLocalMove(_startedPosition, 1f);
        
    }

    public void OnPointerUp(PointerEventData eventData) { }
}