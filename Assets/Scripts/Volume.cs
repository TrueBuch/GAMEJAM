using UnityEngine;
using UnityEngine.EventSystems;

public class Volume : MonoBehaviour, IDraggable
{
    [SerializeField] private Radio _radio;
    [SerializeField] private float _sensivity;
    [SerializeField] private float _rotationSensivity;

    [SerializeField] private float _currentValue;
    public float CurrentValue => _currentValue;

    private bool _isCanDragging;

    private Vector3 _currentRotation;

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
        Mathf.Clamp(_currentValue, 0, 1);
        _currentRotation.z -= delta * _rotationSensivity;

        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void OnEndDrag(PointerEventData eventData) {}
}