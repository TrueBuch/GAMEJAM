using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour, ISingleton
{
    private int _currentDon;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _tictac;
    [SerializeField] private AudioClip _dindon;
    [SerializeField] private Image _image;
    [SerializeField] private List<Sprite> _sprites;
    [SerializeField] private CanvasChanger _canvasChanger;
    private int _index;
    public int Index => _index;

    private bool _playDinDon;
    private IEnumerator ClockAnim()
    {
        while (true)
        {
            _index = (_index + 1) % _sprites.Count;
            _image.sprite = _sprites[_index];
            yield return new WaitForSecondsRealtime(0.25f);
        }

    }

    private void Start()
    {
        _canvasChanger = Main.Get<CanvasChanger>();
        StartCoroutine(ClockAnim());
        StartCoroutine(ClockSound());
    }

    public void ChangeDon(int index)
    {
        _currentDon = index;
        var events = Main.EventSystem.FindAll<IOnDonChanged>();
        PlayDindon();
        Debug.Log($"Don changed - {_currentDon}");
        foreach (var e in events) StartCoroutine(e.OnChanged(_currentDon));
    }

    public void Initialize() { }

    private IEnumerator ClockSound()
    {
        while (true)
        {
            _source.clip = _tictac;
            _source.Play();
            yield return new WaitForSecondsRealtime(_tictac.length);

            if (_playDinDon)
            {
                _playDinDon = false;
                _source.clip = _dindon;
                _source.Play();
                yield return new WaitForSecondsRealtime(_dindon.length);
            }
        }
    }

    private void Update()
    {
        _source.panStereo = _canvasChanger.GetPanStereo(2);
    }

    public void PlayDindon()
    {
        _playDinDon = true;
    }
}

public interface IOnDonChanged
{
    public IEnumerator OnChanged(int value);
}