using Unity.VisualScripting;
using UnityEngine;

public class EntityObject : MonoBehaviour
{
    protected Entity _entity;
    public Entity Entity => _entity;

    protected EntityObjectVisual _visual;
    public EntityObjectVisual Visual => _visual;

    public Vector3 Offset;

    public virtual void Initialize(Entity entity)
    {
        _entity = entity;
    }
}


public class TagDraggable : Tag { }
public class TagSelectable : Tag { }