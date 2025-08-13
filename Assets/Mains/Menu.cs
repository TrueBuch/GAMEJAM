using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _play;
    [SerializeField] private Button _settings;  

    private void Awake()
    {
        _play.onClick.AddListener(() => Main.SceneTransition.SwitchToScene("Main"));
    }  
}