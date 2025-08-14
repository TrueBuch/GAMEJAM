using UnityEngine;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour, IMenu
{
    [SerializeField] private Button _continue;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _exit;

    public void Initialize()
    {
        _continue.onClick.AddListener(() => Main.Get<MenuController>().HideAll());
        _settings.onClick.AddListener(() => Main.Get<MenuController>().ViewMenu<SettingsMenu>());
        _exit.onClick.AddListener(() => Main.SceneTransition.SwitchToScene("Menu"));
    }
}