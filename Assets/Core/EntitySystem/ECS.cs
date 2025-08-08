using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class ECS
{
    public EntityTable<Entity> entityTable = new();

    public Entity Spawn(Entity entity)
    {
        var obj = UnityEngine.Object.Instantiate(entity);

        if (obj.Has<TagDraggable>()) obj.AddComponent<Draggable>();
        if (obj.Has<TagSelectable>()) obj.AddComponent<Selectable>();
        return obj;
    }

    public Entity Spawn(string id)
    {
        return Spawn(Get(id));
    }

    public void Init()
    {
        AddAll();
    }

    private void AddAll()
    {
        var entities = Resources.LoadAll<Entity>("Entities");
        foreach (var entity in entities) entityTable.Add(entity);
    }

    public Entity Get(string id)
    {
        return entityTable.FindByID(id);
    }

    public T Get<T>(string id = null) where T : Entity
    {
        id ??= typeof(T).Name;

        var finded = entityTable.FindByID(id) as T;

        if (finded == null) return null;

        return finded;
    }

    public List<T> GetAll<T>() where T : Entity
    {
        return entityTable.GetAll().OfType<T>().ToList();
    }
    
    public List<Entity> GetAllWith<T>() where T : Tag
    {
        return entityTable.GetAll().Where(entity => entity.Has<T>()).ToList();
    }
}

public class EntityTable<T> where T : Entity
{
    private Dictionary<string, T> _entities = new();

    public void Add(T entity)
    {
        if (entity == null)
        {
            Debug.Log("entity is null");
            return;
        }

        if (entity.ID == null)
        {
            Debug.Log($"entity.ID is null : {entity.GetType().Name}");
            return;
        }
        if (!_entities.ContainsKey(entity.ID))
        {
            _entities.Add(entity.ID, entity);
            Debug.Log($"Added {entity.ID}");
            return;
        }

    }

    public T FindByID(string id)
    {

        return _entities.TryGetValue(id, out var result) ? result : null;
    }

    public List<T> GetAll()
    {
        return _entities.Values.ToList();
    }
}

[Serializable]
public abstract class Tag
{}
