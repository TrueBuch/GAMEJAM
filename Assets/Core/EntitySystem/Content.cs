using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Content
{
    public static GameObject SceneTransition => Load<GameObject>("Main/SceneTransition");

    public static AudioClip AudioNoise => Load<AudioClip>("Audio/Noise");

    private static T Load<T>(string path) where T : Object
    {
        var resource = Resources.Load<T>(path);
        if (resource == null) Debug.LogError($"[Content] not found: {path}");
        return resource;
    }
    
    private static List<T> LoadAll<T>(string path) where T : Object
    {
        var resources = Resources.LoadAll<T>(path).ToList();
        if (resources == null) Debug.LogError($"[Content] not found: {path}");
        return resources;
    }
}