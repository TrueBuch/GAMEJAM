using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool IsSelected;

    private bool _isHovering;
    public bool IsHovering => _isHovering;

    [SerializeField] private Entity _entity;
    public Entity Entity => _entity;

    public void Awake()
    {
        _entity = GetComponent<Entity>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
        foreach (var e in Main.EventSystem.FindAll<IOnPointerEnter>())
        {
            StartCoroutine(e.OnEnter(this, eventData));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        foreach (var e in Main.EventSystem.FindAll<IOnPointerExit>())
        {
            StartCoroutine(e.OnExit(this, eventData));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach (var e in Main.EventSystem.FindAll<IOnPointerDown>())
        {
            StartCoroutine(e.OnDown(this, eventData));
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (var e in Main.EventSystem.FindAll<IOnPointerUp>())
        {
            StartCoroutine(e.OnUp(this, eventData));
        }
    }

}

public interface IOnPointerEnter
{
    public IEnumerator OnEnter(Selectable selectable, PointerEventData eventData);
}

public interface IOnPointerExit
{
    public IEnumerator OnExit(Selectable selectable, PointerEventData eventData);
}

public interface IOnPointerDown
{
    public IEnumerator OnDown(Selectable selectable, PointerEventData eventData);
}

public interface IOnPointerUp
{
    public IEnumerator OnUp(Selectable selectable, PointerEventData eventData);
}