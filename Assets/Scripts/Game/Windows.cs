using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour, ISingleton
{

    [SerializeField] private Image _image;
    [SerializeField] private GameObject _snow;
    [SerializeField] private List<Sprite> _sprites;
    
    public void Initialize() { }

    public void ChangeState(int State)
    {

    }
}