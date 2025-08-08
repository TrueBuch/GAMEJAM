using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Entity _entity;
    public Entity Entity => _entity;

    private bool _isDragging = false;
    public bool IsDragging => _isDragging;

    private Vector3 _pointPosition;
    private Vector2 _difference;

    private void Awake()
    {
        _entity = GetComponent<Entity>();        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        _pointPosition = Main.Get<Input>().MousePosition;
        _difference = _pointPosition - transform.position;

        _isDragging = true;

        foreach (var e in Main.EventSystem.FindAll<IOnEntityBeginDrag>())
        {
            StartCoroutine(e.OnDeginDrag(this));
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        Vector2 pointPosition = Main.Get<Input>().MousePosition;

        transform.position = pointPosition - _difference;

        foreach (var e in Main.EventSystem.FindAll<IOnEntityDrag>())
        {
            StartCoroutine(e.OnDrag(this));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        _isDragging = false;
        
        foreach (var e in Main.EventSystem.FindAll<IOnEntityEndDrag>())
        {
            StartCoroutine(e.OnEndDrag(this));
        }
    }
}

public interface IOnEntityBeginDrag{public IEnumerator OnDeginDrag(Draggable draggable);}
public interface IOnEntityEndDrag {public IEnumerator OnEndDrag(Draggable draggable);}
public interface IOnEntityDrag{public IEnumerator OnDrag(Draggable draggable);}
