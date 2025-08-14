using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour, ISingleton
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Image _image;
    [SerializeField] private Image _eye;

    private int _currentState;
    private void Awake()
    {
        _image = GetComponent<Image>();
        ChangeState(0);
    }

    public void Initialize() { }

    public void ChangeState(int state)
    {
        _currentState = state;
        _image.sprite = _sprites[_currentState];
    }
}