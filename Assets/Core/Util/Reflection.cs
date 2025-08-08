using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util
{
    public static class Reflection
    {
        public static List<Type> FindAllSubclasses<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes()).Where(t => t.IsSubclassOf(typeof(T))
                && !t.IsAbstract
                && !typeof(MonoBehaviour).IsAssignableFrom(t)
                ).ToList();
        }

        public static List<T> FindAllMonoBehaviours<T>()
        {
            return UnityEngine.Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<T>().ToList();
        }
    }
}