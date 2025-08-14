using UnityEngine;
namespace Util
{
    public static class Math
    {
        public static float RoundFloat(float value)
        {
            return Mathf.Round(value * 2f) / 2f;
        }

        public static Quaternion RoundQuaternion(Quaternion q)
        {
            Vector3 euler = q.eulerAngles;
            euler.x = Mathf.Round(euler.x / 90) * 90;
            euler.y = Mathf.Round(euler.y / 90) * 90;
            euler.z = Mathf.Round(euler.z / 90) * 90;
            return Quaternion.Euler(euler);
        }

        public static Vector3 RoundDirection(Vector3 direction)
        {
            float absX = Mathf.Abs(direction.x);
            float absY = Mathf.Abs(direction.y);
            float absZ = Mathf.Abs(direction.z);

            if (absX > absY && absX > absZ) return direction.x > 0 ? Vector3.right : Vector3.left;
            else if (absY > absX && absY > absZ) return direction.y > 0 ? Vector3.up : Vector3.down;
            else return direction.z > 0 ? Vector3.forward : Vector3.back;
        }

        public static float GetPercent(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }

        public static Vector2 DirectionToPoint(Vector2 start, Vector2 end)
        {
            return (end - start).normalized;
        }

    }
}
