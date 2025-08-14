using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour, ISingleton
{
    [SerializeField] private Sprite _normalSprite;
    [SerializeField] private Sprite _nnmSprite;

    [SerializeField] private Button _button;
    public Button Button => _button;
    [SerializeField] private BookFull _bookFull;

    [SerializeField] private Image _image;
    private bool _isNormal = true;
    public bool IsNormal => _isNormal;

    private void Awake()
    {
        _button.onClick.AddListener(OnClicked);
        _isNormal = true;
    }

    private void OnClicked()
    {
        _button.gameObject.SetActive(false);
        _bookFull.gameObject.SetActive(true);
        _bookFull.OnClicked();
    }

    public void ChangeState(bool normal)
    {
        Debug.Log("book changed");
        _isNormal = normal;
        _bookFull.UpdateView();
        _image.sprite = normal ? _normalSprite : _nnmSprite;
    
    }

    public void Initialize() { }
}