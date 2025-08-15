using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lamp : MonoBehaviour
{
    private Image _image;
    [SerializeField] private GameObject _lighting;
    [SerializeField] private Button _button;
    [SerializeField] private Sprite _off;
    [SerializeField] private List<Sprite> _sprites;
    bool isEnabled = true;
    bool isCanPASHALKO = true;
    private float time = 0;
    private int _currentIndex;

    private void Awake()
    {
        _button.onClick.AddListener(OnClicked);
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (isCanPASHALKO)
        {
            time += Time.deltaTime;
            if (time > 10)
            {
                Debug.Log("isCanPASHALKO removed");
                isCanPASHALKO = false;
            }
        }
        
    }
    private void OnClicked()
    {
        if (isCanPASHALKO)
        {
            isEnabled = false;
            _lighting.SetActive(false);
            _image.sprite = _off;
            Main.Get<Ending>().PASHALKO();
        }
    }
    private void Start()
    {
        StartCoroutine(LampAnim());
    }

    private IEnumerator LampAnim()
    {
        while (true)
            {
                if (!isEnabled)
                {
                    _image.sprite = _off;
                    yield break;
                }
                _currentIndex = (_currentIndex + 1) % _sprites.Count;
                _image.sprite = _sprites[_currentIndex];

                yield return new WaitForSecondsRealtime(0.5f);
            }
    }
}