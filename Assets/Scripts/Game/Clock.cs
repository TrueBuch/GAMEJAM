using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private int _currentDon;
    [SerializeField] private Image _image;
    [SerializeField] private List<Sprite> _sprites;
    private int _index;

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
        StartCoroutine(ClockAnim());
    }

    public void ChangeDon()
    {
        _currentDon++;
        var events = Main.EventSystem.FindAll<IOnDonChanged>();
        foreach (var e in events) StartCoroutine(e.OnChanged(_currentDon));
    }
}

public interface IOnDonChanged
{
    public IEnumerator OnChanged(int value);
}