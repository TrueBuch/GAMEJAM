using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class EntityPrefab : MonoBehaviour
{
    [SerializeField, ReadOnly, HorizontalGroup("ID", 0.7f)] private string _builtId;
    public string ID => _builtId;

#if UNITY_EDITOR
    [Button("Update ID"), HorizontalGroup("ID", 0.3f)]
    private void UpdateBuiltId() => _builtId = GenerateId();
#endif

    [SerializeReference] protected List<Tag> _components = new();
    public List<Tag> Components => _components;

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

