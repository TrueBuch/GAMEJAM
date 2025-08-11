using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GazetaFull : MonoBehaviour, ISelectable
{
    [SerializeField] private Gazeta _gazeta;
    [SerializeField] private Canvas _canvas;
    private Vector3 _startedPosition;
    public Vector3 StartedPosition => _startedPosition;

    private void Awake()
    {
        gameObject.SetActive(false);
        _startedPosition = _canvas.transform.localPosition;
    }

    public void OnClicked()
    {
        _canvas.transform.DOLocalMove(Vector3.zero, 0.25f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;
        _canvas.transform.DOLocalMove(_startedPosition, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);  
            _gazeta.Button.gameObject.SetActive(true);
        });
    }

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }

    public void OnPointerUp(PointerEventData eventData) { }
}