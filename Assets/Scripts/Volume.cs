using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Volume : MonoBehaviour, ISelectable
{
    [SerializeField] private Radio _radio;
    [SerializeField] private float _sensivity;

    [SerializeField] private List<Sprite> _sprites;
    private int _currentIndex;
    private Image _image;

    [SerializeField] private float _currentValue;
    public float CurrentValue => _currentValue;

    private bool _isHovering;

    private Input _input;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

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

        if (delta != 0)
        {
            _currentIndex = (_currentIndex + 1) % _sprites.Count;
            _image.sprite = _sprites[_currentIndex];
        } 

        _currentValue += delta * _sensivity;
        _currentValue = Mathf.Clamp(_currentValue, 0, 1);
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