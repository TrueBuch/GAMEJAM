using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Eye : MonoBehaviour, ISingleton
{
    [SerializeField] private GameObject _paintingEye;
    [SerializeField] private GameObject _paintingEye2;
    public bool IsEnabled;
    [SerializeField] private float radius;
    [SerializeField] private float paintRadius;
    [SerializeField] private float paint2Radius;
    [SerializeField] private List<GameObject> _eyes;
    [SerializeField] private Image _door;
    [SerializeField] private Image _shkaf;
    [SerializeField] private Sprite _doorScary;
    [SerializeField] private Sprite _shkafScary;
    private Input _input;
    public void Initialize()
    {
        _input = Main.Get<Input>();
    }

    private void Awake()
    {
        _paintingEye.gameObject.SetActive(false);
    }

    public void On()
    {
        IsEnabled = true;
        _door.sprite = _doorScary;
        _shkaf.sprite = _shkafScary;
        _paintingEye.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!IsEnabled) return;
        var mousePos = _input.MousePosition;

        foreach (var obj in _eyes)
        {
            var dir = Util.Math.DirectionToPoint(obj.transform.position, mousePos);

            if (Vector2.Distance(obj.transform.parent.position, mousePos) > (radius * 2))
            {
                dir *= radius;
                obj.transform.localPosition = dir;
            }
            else
            {
                obj.transform.localPosition = Vector2.zero;
            }
        }

        var dirPainting = Util.Math.DirectionToPoint(_paintingEye.transform.position, mousePos);

        if (Vector2.Distance(_paintingEye.transform.parent.position, mousePos) > (paintRadius * 6))
        {
            var edir = dirPainting * paintRadius;
            var e2dir = dirPainting * paint2Radius;

            _paintingEye.transform.localPosition = edir;
            _paintingEye2.transform.localPosition = e2dir;
        }
        else
        {
            _paintingEye.transform.localPosition = Vector2.zero;
            _paintingEye2.transform.localPosition = Vector2.zero;
        }
    }
}