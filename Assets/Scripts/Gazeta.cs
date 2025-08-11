using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gazeta : MonoBehaviour, ISelectable
{
    private Button _gazeta;
    [SerializeField] private Canvas _gazetaFull;
    private Vector3 _startedPosition;

    private void Awake()
    {
        _gazeta.onClick.AddListener(OnClicked);
        _startedPosition = _gazeta.transform.localPosition;
    }

    private void OnClicked()
    {
        _gazeta.transform.DOLocalMove(Vector3.zero, 1f);
    }

    public void OnPointerDown(PointerEventData eventData) { }

    public void OnPointerEnter(PointerEventData eventData) {}

    public void OnPointerExit(PointerEventData eventData) {}

    public void OnPointerUp(PointerEventData eventData) {}


}