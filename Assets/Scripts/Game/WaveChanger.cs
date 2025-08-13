using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveChanger : MonoBehaviour
{
    [SerializeField] private Radio _radio;
    [SerializeField] private List<string> _waves = new();

    [SerializeField] private List<Sprite> _sprites;
    private Image _image;

    int _currentIndex = 0;
    private Button _button;


    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _currentIndex = (_currentIndex + 1) % _waves.Count;
        _image.sprite = _sprites[_currentIndex];
        _radio.ChangeWave(Main.Get<Radio>().State.Waves[_waves[_currentIndex]]);

        //Main.Get<Subtitles>().TypeByKey("ExampleSubtitle");
    }
}