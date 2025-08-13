using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    private Image _image;
    [SerializeField] private List<Sprite> _sprites;
    private int _currentIndex;

    private void Awake()
    {
        _image = GetComponent<Image>();   
    }
    private void Start()
    {
        StartCoroutine(LampAnim());
    }

    private IEnumerator LampAnim()
    {
        while (true)
        {
            _currentIndex = (_currentIndex + 1) % _sprites.Count;
            _image.sprite = _sprites[_currentIndex];

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}