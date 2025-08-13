using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Gazeta : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private Button _button;
    public Button Button => _button;
    private Image _image;
    [SerializeField] private GazetaFull _gazetaFull;

    private void Awake()
    {
        _button.onClick.AddListener(OnClicked);
        _image = GetComponent<Image>();
    }

    private void OnClicked()
    {
        _button.gameObject.SetActive(false);
        _gazetaFull.gameObject.SetActive(true);
        _gazetaFull.OnClicked();
    }

    public void ChangeSprite(int index)
    {
        _image.sprite = _sprites[index];
    }
}