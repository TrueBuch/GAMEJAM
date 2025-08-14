using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour, ISingleton
{
    [SerializeField] private Image _outside;
    [SerializeField] private Image _scary;
    [SerializeField] private Image _code;
    [SerializeField] private List<Sprite> _scarySpites;
    [SerializeField] private GameObject _snow;
    public bool CodeViewed = false;
    private int index;

    private IEnumerator ScaryAnimation()
    {
        while (true)
        {
            index = (index + 1) % _scarySpites.Count;
            _scary.sprite = _scarySpites[index];
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    public void Initialize() { }

    private void Awake()
    {
        _code.gameObject.SetActive(false);
        _scary.gameObject.SetActive(false);
        StartCoroutine(ScaryAnimation());
    }
    public void ViewCode()
    {
        _code.gameObject.SetActive(true);
        CodeViewed = true;
    }

    public void StartScary()
    {
        _code.gameObject.SetActive(false);
        _scary.gameObject.SetActive(true);
    }
}