using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class Subtitles : MonoBehaviour, ISingleton
{
    public void Initialize() {}

    private TMP_Text _text;
    private Coroutine _coroutine;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void TypeByKey(string key)
    {
        var text = Locale.Get(key);
        Type(text);
    }

    public void Type(string text)
    {
        if (_coroutine != null) StopCoroutine(_coroutine);
        StartCoroutine(Play(text));
    }

    private IEnumerator Play(string text)
    {
        _text.text = "";
        foreach (char c in text)
        {
            _text.text += c;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }
}