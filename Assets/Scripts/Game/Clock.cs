using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour, ISingleton
{
    private int _currentDon;
    [SerializeField] private Image _image;
    [SerializeField] private List<Sprite> _sprites;
    private int _index;
    public int Index => _index;

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
        Debug.Log($"Don changed - {_currentDon}");
        foreach (var e in events) StartCoroutine(e.OnChanged(_currentDon));
    }

    public void Initialize() { }
}

public interface IOnDonChanged
{
    public IEnumerator OnChanged(int value);
}