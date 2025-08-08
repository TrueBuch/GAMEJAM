using UnityEngine;

namespace Util
{
    public static class Text
    {
        public static string ApplyColor(string text, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{text}</color>";
        }
    }
}