using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class Subtitles : MonoBehaviour, ISingleton
{
    public void Initialize() { }

    [SerializeField] private List<AudioClip> _playerVoice;
    [SerializeField] private List<AudioClip> _monsterVoice;
    [SerializeField] private List<AudioClip> _doctor1Voice;
    [SerializeField] private List<AudioClip> _doctor2Voice;
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

    public void TypeByKey(Voice voice, bool delete, string key)
    {
        var text = Locale.Get(key);
        Type(voice, delete, text);
    }

    public void TypeByKey(Voice voice, bool delete, float delay, string key)
    {
        var text = Locale.Get(key);
        Type(voice, delete, delay, text);
    }

    public void Type(Voice voice, bool delete, string text)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _text.text = "";
        }
        _coroutine = StartCoroutine(Play(voice, delete, 0, text));
    }

    public void Type(Voice voice, bool delete, float delay, string text)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _text.text = "";
        }
        _coroutine = StartCoroutine(Play(voice, delete, delay, text));
    }

    private IEnumerator Play(Voice voice, bool delete, float delay, string text)
    {
        var voices = GetVoices(voice);
        _isPlaying = true;
        _text.text = "";
        foreach (char c in text)
        {
            var printDelay = delay;
            if (voice != Voice.NONE)
            {
                var clip = voices[UnityEngine.Random.Range(0, voices.Count)];
                printDelay = delay == 0 ? clip.length : delay;
                _source.PlayOneShot(clip);
            }
            if (!char.IsWhiteSpace(c))
            {
                yield return new WaitForSecondsRealtime(printDelay);
            }

            _text.text += c;
        }
        yield return new WaitForSecondsRealtime(1f);
        if (delete) _text.text = "";
        _isPlaying = false;
    }

    private List<AudioClip> GetVoices(Voice voice)
    {

        return voice switch
        {
            Voice.PLAYER => _playerVoice,
            Voice.MONSTER => _monsterVoice,
            Voice.DOCTOR1 => _doctor1Voice,
            Voice.DOCTOR2 => _doctor2Voice,
            _ => new List<AudioClip>(),
        };
    }
}

public enum Voice
{
    NONE,
    PLAYER,
    MONSTER,
    DOCTOR1,
    DOCTOR2
}