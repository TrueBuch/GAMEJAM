using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tuning : MonoBehaviour, ISelectable
{
    [SerializeField] private Radio _radio;
    [SerializeField] private float _sensivity;
    [SerializeField] private float _rotationSensivity;

    private float _currentValue;
    public float CurrentValue => _currentValue;

    private bool _isHovering;

    private Vector3 _currentRotation;

    private Input _input;

    public void Init()
    {
        _input = Main.Get<Input>();
        _radio.WaveChanged.AddListener(OnWaveChanged);
    }

    private void OnWaveChanged(Entity old, Entity wave)
    {
        var newMin = wave.Get<TagWave>().Min;
        var newMax = wave.Get<TagWave>().Max;

        if (old != null)
        {
            var oldMin = old.Get<TagWave>().Min;
            var oldMax = old.Get<TagWave>().Max;

            _currentValue = newMin + (_currentValue - oldMin) / (oldMax - oldMin) * (newMax - newMin);
        }
        _currentValue = Mathf.Clamp(_currentValue, newMin, newMax);

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
        _currentValue = Mathf.Clamp(_currentValue, _radio.Wave.Get<TagWave>().Min, _radio.Wave.Get<TagWave>().Max);
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