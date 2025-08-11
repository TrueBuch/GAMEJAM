using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tuning : MonoBehaviour, ISelectable
{
    [SerializeField] private Radio _radio;
    [SerializeField] private float _sensivity;
    [SerializeField] private List<Sprite> _sprites;

    [SerializeField] private List<Sprite> _arrowSprites;
    [SerializeField] private Image _arrowImage;

    [SerializeField] private TMP_Text _text;
    private int _currentIndex;
    private Image _image;

    private float _currentValue;
    public float CurrentValue => _currentValue;

    private bool _isHovering;

    private Input _input;

    public readonly UnityEvent ValueChanged = new(); 

    private void Awake()
    {
        _image = GetComponent<Image>();
    }
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

            float percent = (_currentValue - oldMin) / (oldMax - oldMin);
            _currentValue = newMin + percent * (newMax - newMin);
        }
        _currentValue = Mathf.Clamp(_currentValue, newMin, newMax);
        _currentValue = Mathf.RoundToInt(_currentValue);
    }

    private void Update()
    {
        UpdateValue();
        UpdateText();
    }

    public void UpdateText()
    {
        if (!_radio.IsEnabled) _text.text = "";
        else _text.text = _currentValue.ToString();
    }
    public void UpdateValue()
    {
        if (!_isHovering) return;

        var delta = _input.Scroll.y;

        var newValue = _currentValue + delta * _sensivity;
        newValue = Mathf.Clamp(newValue, _radio.Wave.Get<TagWave>().Min, _radio.Wave.Get<TagWave>().Max);
        if (newValue != _currentValue)
        {
            ValueChanged.Invoke();
            _currentIndex = (_currentIndex + 1) % _sprites.Count;
            _image.sprite = _sprites[_currentIndex];
        }
        _currentValue = newValue;
        if (delta != 0)
        {
            float percent = (_currentValue - _radio.Wave.Get<TagWave>().Min) / (_radio.Wave.Get<TagWave>().Max - _radio.Wave.Get<TagWave>().Min);
            var arrowIndex = Mathf.RoundToInt(percent * (_arrowSprites.Count - 1));
            _arrowImage.sprite = _arrowSprites[arrowIndex];
        } 
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