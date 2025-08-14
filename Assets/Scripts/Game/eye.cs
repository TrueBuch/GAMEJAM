using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour, ISingleton
{
    public bool IsEnabled;
    [SerializeField] private float radius;
    [SerializeField] private List<GameObject> _eyes;
    private Input _input;
    public void Initialize()
    {
        _input = Main.Get<Input>();
    }

    private void Update()
    {
        if (!IsEnabled) return;
        var mousePos = _input.MousePosition;

        foreach (var obj in _eyes)
        {
            var dir = Util.Math.DirectionToPoint(obj.transform.position, mousePos);
            
            if (Vector2.Distance(obj.transform.parent.position, _input.MousePosition) > (radius * 2))
            {
                dir *= radius;
                obj.transform.localPosition = dir;
            }
            else
            {
                obj.transform.localPosition = Vector2.zero;
            }
        }
    }
}