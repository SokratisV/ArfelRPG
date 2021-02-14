using UnityEngine;

namespace RPG.Core
{
    public static class Helper
    {
        public static bool IsWithinDistance(Vector3 from, Vector3 to, float distance)
        {
            return(from - to).sqrMagnitude <= distance * distance;
        }

        public static bool IsWithinDistance(Transform from, Transform to, float distance)
        {
            return IsWithinDistance(from.position, to.position, distance);
        }
    }
}