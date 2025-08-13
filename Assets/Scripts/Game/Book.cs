using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Book : MonoBehaviour
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite nnmSprite;

    [SerializeField] private Button _button;
    public Button Button => _button;
    [SerializeField] private BookFull _bookFull;

    private bool _isNormal = true;
    public bool IsNormal => _isNormal;

    private void Awake()
    {
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        _button.gameObject.SetActive(false);
        _bookFull.gameObject.SetActive(true);
        _bookFull.OnClicked();
    }
}