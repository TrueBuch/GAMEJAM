using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField] private Sprite _normal;
    [SerializeField] private Sprite _hovered;
    [SerializeField] private Sprite _clicked;
    private Canvas _canvas;
    private Image _image;
    private Vector2 offset = new(27f, -27f);
    public bool IsHovered;
    public void Awake()
    {
        if (Main.Cursor != null && Main.Cursor != this)
        {
            Destroy(transform.parent.gameObject);
            return;
        }
        _canvas = GetComponentInParent<Canvas>();
        _image = GetComponent<Image>();

        DontDestroyOnLoad(transform.parent);
    }

    private void Update()
    {
        Vector2 mousePos = UnityEngine.Input.mousePosition;
        transform.position = mousePos + offset;

        if (UnityEngine.Input.GetMouseButton(0)) _image.sprite = _clicked;
        else
        {
            if (IsHovered) _image.sprite = _hovered;
            else _image.sprite = _normal;
        }
    }
}