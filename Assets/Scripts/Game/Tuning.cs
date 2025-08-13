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

    private int _currentValue;
    public int CurrentValue => _currentValue;

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

    private void OnWaveChanged(Wave old, Wave wave)
    {
        var newMin = wave.Min;
        var newMax = wave.Max;
        var newValue = (float) _currentValue;
        if (old != null)
        {
            var oldMin = old.Min;
            var oldMax = old.Max;

            float percent = (_currentValue - oldMin) / (oldMax - oldMin);
            newValue = newMin + percent * (newMax - newMin);
        }
        newValue = Mathf.Clamp(newValue, newMin, newMax);
        _currentValue = Mathf.RoundToInt(newValue);
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

        var wave = _radio.State.CurrentWave;
        newValue = Mathf.Clamp(newValue, wave.Min, wave.Max);
        newValue = Mathf.RoundToInt(newValue);

        if (newValue != _currentValue)
        {
            _currentValue = Mathf.RoundToInt(newValue);
            ValueChanged.Invoke();

            _currentIndex = (_currentIndex + 1) % _sprites.Count;
            _image.sprite = _sprites[_currentIndex];
        }

        if (delta != 0)
        {
            float percent = (_currentValue - wave.Min) /
                            (wave.Max - wave.Min);
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