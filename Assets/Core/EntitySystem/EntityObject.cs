using Unity.VisualScripting;
using UnityEngine;

public class EntityObject : MonoBehaviour
{
    protected Entity _entity;
    public Entity Entity => _entity;

    protected EntityObjectVisual _visual;
    public EntityObjectVisual Visual => _visual;

    public Slot Slot;

    public Vector3 Offset;

    public virtual void Initialize(Entity entity)
    {
        _entity = entity;

        if (_entity.Has<TagDraggable>()) this.AddComponent<Draggable>();
        if (_entity.Has<TagSelectable>()) this.AddComponent<Selectable>();

        if (_entity.Has<TagView>())
        {
            var tagView = _entity.Get<TagView>();
            _visual = Instantiate(tagView.VisualPrefab).GetComponent<EntityObjectVisual>();
            _visual.Initialize(this);
        }
    }
}


public class TagDraggable : Tag { }
public class TagSelectable : Tag { }