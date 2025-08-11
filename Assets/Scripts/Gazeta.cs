using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Gazeta : MonoBehaviour
{
    [SerializeField] private Button _button;
    public Button Button => _button;
    [SerializeField] private GazetaFull _gazetaFull;
    
    private void Awake()
    {
        _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        _button.gameObject.SetActive(false);
        _gazetaFull.gameObject.SetActive(true);
        _gazetaFull.OnClicked();
    }
}