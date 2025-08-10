using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasChanger : MonoBehaviour
{
    [SerializeField] private List<Canvas> _walls;
    [SerializeField] private int _currentIndex;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    private bool _isPlaying;

    private void Awake()
    {
        _leftButton.onClick.AddListener(OnLeftButtonClicked);
        _rightButton.onClick.AddListener(OnRightButtonClicked);
    }

    private void OnLeftButtonClicked()
    {
        if (_isPlaying) return;
        StartCoroutine(Change(-1));
    }

    private void OnRightButtonClicked()
    {
        if (_isPlaying) return;
        StartCoroutine(Change(1));
    }

    private IEnumerator Change(int dir)
    {
        var next = (_currentIndex + dir + _walls.Count) % _walls.Count;
        _isPlaying = true;

        var currentCanvas = _walls[_currentIndex];
        var nextCanvas = _walls[next];

        var startPos = currentCanvas.transform.localPosition;
        var newPos = currentCanvas.transform.localPosition;
        var nextCanvasPos = nextCanvas.transform.localPosition;
        newPos.x -= 1920 * dir;

        var time = 0f;

        while (time < _duration)
        {
            time += Time.deltaTime;
            var t = Mathf.Clamp01(time / _duration);
            currentCanvas.transform.localPosition = Vector2.Lerp(startPos, newPos, t);
            yield return null;
        }

        time = 0;

        newPos.x = -newPos.x;
        nextCanvas.transform.localPosition = newPos;
        currentCanvas.transform.localPosition = nextCanvasPos;

        while (time < _duration)
        {
            Debug.Log("123");
            time += Time.deltaTime;
            var t = Mathf.Clamp01(time / _duration);
            nextCanvas.transform.localPosition = Vector2.Lerp(newPos, startPos, t);
            yield return null;
        }

        nextCanvas.transform.localPosition = startPos;
        _currentIndex = next;
        _isPlaying = false;
    }
}