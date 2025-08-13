using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _play;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _quit;

    private void Awake()
    {
        _play.onClick.AddListener(() => Main.SceneTransition.SwitchToScene("Main"));
        _quit.onClick.AddListener(() => Application.Quit());
        _settings.onClick.AddListener(() => Main.Get<MenuController>().ViewMenu<SettingsMenu>());
    }  
}