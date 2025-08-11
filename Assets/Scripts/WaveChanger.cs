using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveChanger : MonoBehaviour
{
    [SerializeField] private Radio _radio;
    private List<Entity> _waves = new();

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

    public void Init()
    {
        _waves.Clear();

        var ecs = Main.ECS;
        _waves.Add(ecs.Get("Wave/FM"));
        _waves.Add(ecs.Get("Wave/AM"));
        _waves.Add(ecs.Get("Wave/SW"));
    }

    private void OnClick()
    {
        _currentIndex = (_currentIndex + 1) % _waves.Count;
        _image.sprite = _sprites[_currentIndex];
        _radio.ChangeWave(_waves[_currentIndex]);

        //Main.Get<Subtitles>().TypeByKey("ExampleSubtitle");
    }
}