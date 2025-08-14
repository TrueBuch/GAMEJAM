using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour, ISingleton
{
    [SerializeField] private Image _flash;
    [SerializeField] private Image _fading;
    [SerializeField] private List<Sprite> _whiteSprites;
    [SerializeField] private List<Sprite> _redSprites;


    public void Initialize()
    {
        _flash.gameObject.SetActive(false);
        _fading.gameObject.SetActive(false);
    }

    public void StarEndind(bool isFirst)
    {
        if (isFirst) StartCoroutine(FirstEnding());
        else StartCoroutine(SecondEnding());
    }

    public IEnumerator FirstEnding()
    {
        yield return StartCoroutine(FlashAnimation(true));

        yield return new WaitForSecondsRealtime(1f);
        yield return StartCoroutine(FadeAnimation());

        var subs = Main.Get<Subtitles>();

        subs.TypeByKey(Voice.DOCTOR1, true, "ending_1_1C");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR2, true, "ending_1_2C");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR1, true, "ending_1_1C_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR2, true, "ending_1_2C_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR2, true, "ending_1_2C_1_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR1, true, "ending_1_1C_2");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR1, true, "ending_1_1C_2_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR2, true, "ending_1_2C_2");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR2, true, "ending_1_2C_2_1");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR1, true, "ending_1_1С_3");
        yield return new WaitUntil(() => !subs.IsPlaying);
        subs.TypeByKey(Voice.DOCTOR1, true, "ending_1_1С_3_1");
        yield return new WaitUntil(() => !subs.IsPlaying);



    }

    public IEnumerator SecondEnding()
    {
        yield break;
    }

    public IEnumerator FlashAnimation(bool isFirst)
    {
        _flash.gameObject.SetActive(true);
        if (isFirst)
        {
            _flash.sprite = _whiteSprites[0];
            foreach (var s in _whiteSprites)
            {
                _flash.sprite = s;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
        else
        {
            _flash.sprite = _redSprites[0];
            foreach (var s in _redSprites)
            {
                _flash.sprite = s;
                yield return new WaitForSecondsRealtime(0.01f);
            }
        }
    }

    public IEnumerator FadeAnimation()
    {

        Color startColor = _fading.color;
        startColor.a = 0f;
        Color endColor = _fading.color;
        endColor.a = 1f;

        _fading.color = new(1, 1, 1, 0f);

        float time = 0;
        while (time < 3f)
        {
            time += Time.deltaTime;
            _fading.color = Color.Lerp(startColor, endColor, time / 3f);
            yield return null;
        }

        _fading.color = endColor;
    }
}