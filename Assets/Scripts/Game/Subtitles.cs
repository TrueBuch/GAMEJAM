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
    private bool _isPlaying;
    public bool IsPlaying => _isPlaying;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _text = GetComponent<TMP_Text>();
    }

    public void TypeByKey(bool delete, string key)
    {
        var text = Locale.Get(key);
        Type(delete, text);
    }

    public void TypeByKey(bool delete, float delay, string key)
    {
        var text = Locale.Get(key);
        Type(delete, delay, text);
    }

    public void Type(bool delete, string text)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _text.text = "";
        }
        _coroutine = StartCoroutine(Play(delete, -1,text));
    }

    public void Type(bool delete, float delay, string text)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _text.text = "";
        }
        _coroutine = StartCoroutine(Play(delete, delay, text));
    }

    private IEnumerator Play(bool delete, float delay, string text)
    {
        var printDelay = delay == -1 ? _clip.length : delay;
        _isPlaying = true;
        _text.text = "";
        foreach (char c in text)
        {

            //_source.PlayOneShot(_clip);
            yield return new WaitForSecondsRealtime(printDelay);
            
            _text.text += c;
        }
        yield return new WaitForSecondsRealtime(1f);
        if (delete) _text.text = "";
        _isPlaying = false;
    }
}