using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CursorHovered : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData) => Main.Cursor.IsHovered = true;
    public void OnPointerExit(PointerEventData eventData) => Main.Cursor.IsHovered = false;
}