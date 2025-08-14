using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour, ISingleton
{
    [SerializeField] private Image _flash;
    [SerializeField] private List<Sprite> _whiteSprites;
    [SerializeField] private List<Sprite> _redSprites;
    public void Initialize() { }

    public void StarEndind(bool isFirst)
    {
        if (isFirst) StartCoroutine(FirstEnding());
        else StartCoroutine(SecondEnding());
    }

    public IEnumerator FirstEnding()
    {
        yield break;
    }

    public IEnumerator SecondEnding()
    {
        yield break;
    }

    public IEnumerator FlashAnimation()
    {
        yield break;
    }
}