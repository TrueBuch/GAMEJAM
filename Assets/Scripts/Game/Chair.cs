using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Chair : MonoBehaviour, ISingleton
{
    [SerializeField] private Vector3 _doorPosition;
    private Vector3 _defaultPosition;
    [SerializeField] private Button _button;
    [SerializeField] private Sprite _default;
    [SerializeField] private Sprite _door;
    public bool IsNormal;

    public void ChangeState(bool normal)
    {
        IsNormal = normal;
        if (normal)
        {
            _button.image.sprite = _default;
            _button.transform.localPosition = _defaultPosition;
        }
        else
        {
            _button.image.sprite = _door;
            _button.transform.localPosition = _doorPosition;
        }
    }

    private void Start()
    {
        IsNormal = true;
        _button.onClick.AddListener(() =>
        {
            var events = Main.EventSystem.FindAll<IOnChairClicked>();
            foreach (var e in events) StartCoroutine(e.OnClicked(IsNormal));
        });
    }

    public void Initialize() { }
}

public interface IOnChairClicked
{
    public IEnumerator OnClicked(bool IsNormal);
}