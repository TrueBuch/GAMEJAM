using UnityEngine;

public class Game : MonoBehaviour, ISingleton
{
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _containerVisual;

    public void Initialize() { }

    public void Start()
    {

    }
}