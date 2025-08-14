using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class MenuController : MonoBehaviour, ISingleton
{
    private List<IMenu> _menus;
    public void Initialize()
    {
        Main.Get<Input>().EscStarted.AddListener(OnEscStarted);
        _menus = Reflection.FindAllMonoBehaviours<IMenu>();
        foreach (var m in _menus)
        {
            m.Initialize();
        }
        HideAll();
    }

    public void OnEscStarted()
    {
        if (SceneManager.GetActiveScene().name != "Main")
        {
            HideAll();
            return;
        }

        ViewMenu<EscapeMenu>();
    }
    public void ViewMenu<T>() where T : IMenu
    {
        foreach (var menu in _menus)
        {
            if (menu is MonoBehaviour monoBehaviour)
                monoBehaviour.gameObject.SetActive(menu.GetType() == typeof(T));
        }
    }

    public void HideAll()
    {
        foreach (var menu in _menus)
        {
            if (menu is MonoBehaviour monoBehaviour)
                monoBehaviour.gameObject.SetActive(false);
        }
    }
}

public interface IMenu
{
    public void Initialize();
}