using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class Subtitles : MonoBehaviour, ISingleton
{
    public void Initialize() { }

    [SerializeField] private List<AudioClip> _playerVoice;
    [SerializeField] private List<AudioClip> _monsterVoice;
    [SerializeField] private List<AudioClip> _doctor1Voice;
    [SerializeField] private List<AudioClip> _doctor2Voice;
    private AudioSource _source;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _bg;
    private Coroutine _coroutine;
    private bool _isPlaying;
    public bool IsPlaying => _isPlaying;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _bg.gameObject.SetActive(false);
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
        _bg.gameObject.SetActive(true);
        var voices = GetVoices(voice);
        _isPlaying = true;
        _text.text = "";

        string colorHex = voice switch
        {
            Voice.MONSTER => "#6F2929",  
            Voice.SPEC => "#6F2929",     
            Voice.PLAYER => "#B59E90",   
            Voice.DOCTOR1 => "#38405D",  
            Voice.DOCTOR2 => "#4C546D",  
            _ => "#B59E90",
        };

        _text.text = $"<color={colorHex}>";

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
        yield return new WaitForSecondsRealtime(2f);
        if (delete)
        {
            _bg.gameObject.SetActive(false);
            _text.text = "";
        }
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
    DOCTOR2,
    SPEC
}