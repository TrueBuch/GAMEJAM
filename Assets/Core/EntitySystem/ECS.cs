using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class ECS
{
    public EntityTable<Entity> entityTable = new();

    public void Init()
    {
        AddAll();
    }

    private void AddAll()
    {
        var allTypes = Util.Reflection.FindAllSubclasses<Entity>();

        foreach (var type in allTypes)
        {
            if (Activator.CreateInstance(type) is Entity entity) entityTable.Add(entity);
        }

        var resources = Resources.LoadAll<EntityPrefab>("Entities");
        foreach (var prefab in resources)
        {
            var entity = new Entity();
            entity.ID = prefab.ID;
            entity.Set(prefab.Components);

            entityTable.Add(entity);
        }
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

    public void Reset()
    {
        entityTable.Clear();
        AddAll();
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
            //Debug.Log($"Added {entity.ID}");
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

    public void Clear()
    {
        _entities.Clear();
    }
}

public class Entity
{
    public string ID;

    [SerializeReference] private Dictionary<Type, Tag> _components = new();

    public T Define<T>() where T : Tag, new()
    {
        var type = typeof(T);
        if (!_components.ContainsKey(type))
        {
            var instance = new T();
            _components[type] = instance;
        }

        return (T)_components[type];
    }

    public T Get<T>() where T : Tag
    {
        if (_components.TryGetValue(typeof(T), out var component))
            return (T)component;

        return null;
    }

    public bool Has<T>() where T : Tag
    {
        return _components.ContainsKey(typeof(T));
    }

    public void Remove<T>() where T : Tag
    {
        _components.Remove(typeof(T));
    }

    public void Set(List<Tag> components)
    {
        _components.Clear();

        foreach (var component in components)
        {
            var type = component.GetType();
            _components[type] = component;
        }
    }
}

[Serializable]
public abstract class Tag {}