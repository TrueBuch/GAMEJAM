using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button _play;


    private void Awake()
    {
        _play.onClick.AddListener(() => Main.SceneTransition.SwitchToScene("Main"));
    }  
}