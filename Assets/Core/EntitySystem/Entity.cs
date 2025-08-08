using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField, ReadOnly, HorizontalGroup("ID", 0.7f)] private string _id;
    public string ID => _id;

    [SerializeReference] private List<Tag> _tags = new();

    [HideInInspector] public EntityVisual Visual;

    public EntityVisual SpawnVisual()
    {
        if (Visual != null) return Visual;

        if (Has<TagView>())
        {
            Visual = Instantiate(Get<TagView>().Prefab).GetComponent<EntityVisual>();
            Visual.Initialize(this);
        }

        return Visual;
    }

    public T Get<T>() where T : Tag
    {
        foreach (var tag in _tags)
        {
            if (tag is T match) return match;
        }
        return null;
    }

    public bool Has<T>() where T : Tag
    {
        return Get<T>() != null;
    }

    public T Define<T>() where T : Tag, new()
    {
        if (!Has<T>())
        {
            var tag = new T();
            _tags.Add(tag);
        }

        return Get<T>();
    }

    public void Remove<T>() where T : Tag
    {
        _tags.RemoveAll(t => t is T);
    }
    
#if UNITY_EDITOR
    [Button("Update ID"), HorizontalGroup("ID", 0.3f)]
    private void UpdateBuiltId() => _id = GenerateId();
#endif

#if UNITY_EDITOR
    private string GenerateId()
    {
        string path = AssetDatabase.GetAssetPath(gameObject);
        if (path.StartsWith("Assets/Resources/Entities/") && path.EndsWith(".prefab"))
        {
            path = path.Substring("Assets/Resources/Entities/".Length);
            path = path.Substring(0, path.Length - ".prefab".Length);
        }
        return path;
    }
#endif
}


public class TagView : Tag
{
    public GameObject Prefab;
    public float FollowSpeed;
    public Sprite Sprite;
    public Sprite Shadow;
}

public class TagDraggable : Tag { }
public class TagSelectable : Tag { }