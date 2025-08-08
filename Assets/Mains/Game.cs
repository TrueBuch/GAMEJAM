using UnityEngine;

public class Game : MonoBehaviour, ISingleton
{
    [SerializeField] private Transform _container;
    [SerializeField] private Transform _containerVisual;

    public void Initialize() { }

    public void Start()
    {
        var obj = Main.ESC.Spawn("GameObject");
        obj.transform.SetParent(_container);
        obj.transform.localPosition = Vector3.zero;
        var visual = obj.SpawnVisual();
        if (visual != null) visual.transform.SetParent(_containerVisual);
    }
}