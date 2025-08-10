using UnityEngine;
using UnityEngine.EventSystems;

public class Volume : MonoBehaviour, ISelectable
{
    [SerializeField] private Radio _radio;
    [SerializeField] private float _sensivity;
    [SerializeField] private float _rotationSensivity;

    [SerializeField] private float _currentValue;
    public float CurrentValue => _currentValue;

    private Vector3 _currentRotation;

    private bool _isHovering;

    private Input _input;

    public void Init()
    {
        _input = Main.Get<Input>();
    }

    private void Update()
    {
        UpdateValue();
    }
    public void UpdateValue()
    {
        if (!_isHovering) return;

        var delta = _input.Scroll.y;
        _currentValue += delta * _sensivity;
        _currentValue = Mathf.Clamp(_currentValue, 0, 1);
        _currentRotation.z -= delta * _rotationSensivity;

        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
    }

    public void OnPointerDown(PointerEventData eventData) {}

    public void OnPointerUp(PointerEventData eventData) {}
}