using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Content
{
    public static GameObject SceneTransition => Load<GameObject>("Main/SceneTransition");

    //public static CardData CardData(string name) => Load<CardData>($"Card/Data/{name}");

    //public static List<Location> Locations = LoadAll<Location>("Level/Locations");

    private static T Load<T>(string path) where T : Object
    {
        var resource = Resources.Load<T>(path);
        if (resource == null) Debug.LogWarning($"[Content] not found: {path}");
        return resource;
    }
    
    private static List<T> LoadAll<T>(string path) where T : Object
    {
        var resources = Resources.LoadAll<T>(path).ToList();
        if (resources == null) Debug.LogWarning($"[Content] not found: {path}");
        return resources;
    }
}