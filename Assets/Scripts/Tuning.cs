using UnityEngine;
using UnityEngine.EventSystems;

public class Tuning : MonoBehaviour, IDraggable
{
    [SerializeField] private float _sensivity;
    [SerializeField] private float _rotationSensivity;

    private RectTransform _rectTransform;

    private float _currentVolume;
    public float CurrentVolume;
    private float _currentValue;
    public float CurrentValue => _currentValue;

    private bool _isCanDragging;

    private Vector3 _currentRotation;

    private void Update()
    {
        Debug.Log(_currentValue);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        var input = Main.Get<Input>();

         _isCanDragging = Vector2.Distance(transform.position, input.MousePosition) < 100f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isCanDragging) return;

        var input = Main.Get<Input>();

        var delta = input.MouseDelta.x * Time.deltaTime;
        _currentValue += delta * _sensivity;
        _currentRotation.z -= delta * _rotationSensivity;

        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
}