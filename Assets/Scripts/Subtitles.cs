using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class Subtitles : MonoBehaviour, ISingleton
{
    public void Initialize() {}

    [SerializeField] private AudioClip _clip;
    private AudioSource _source;
    private TMP_Text _text;
    private Coroutine _coroutine;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _text = GetComponent<TMP_Text>();
    }

    public void TypeByKey(string key)
    {
        var text = Locale.Get(key);
        Type(text);
    }

    public void Type(string text)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _text.text = "";
        }
        _coroutine = StartCoroutine(Play(text));
    }

    private IEnumerator Play(string text)
    {
        
        _text.text = "";
        foreach (char c in text)
        {
            if (!char.IsWhiteSpace(c))
            {
                _source.PlayOneShot(_clip);
                yield return new WaitForSecondsRealtime(_clip.length);
            }
            _text.text += c;
        }
    }
}