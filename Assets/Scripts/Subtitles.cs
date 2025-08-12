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
        _isPlaying = true;
        _text.text = "";
        foreach (char c in text)
        {
            if (!char.IsWhiteSpace(c))
            {
                //_source.PlayOneShot(_clip);
                yield return new WaitForSecondsRealtime(_clip.length);
            }
            _text.text += c;
        }
        yield return new WaitForSecondsRealtime(1f);
        _text.text = "";
        _isPlaying = false;
    }
}