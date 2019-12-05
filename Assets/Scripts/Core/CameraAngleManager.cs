using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public enum Areas
    {
        None,
        TownEntrance,
        TownInside,
        ForestEntrance,
        ForestExit,
        HillClimbing,
        HillTop,
    }
    public class CameraAngleManager : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] AreaAngles[] areaAngles;

        Dictionary<Areas, Vector3> areaToAngleDictionary;
        Vector3 position;

        [System.Serializable]
        private class AreaAngles
        {
            public Areas area;
            public SerializableVector3 angles;
        }

        private void Awake()
        {
            areaToAngleDictionary = new Dictionary<Areas, Vector3>();
            foreach (var angle in areaAngles)
            {
                areaToAngleDictionary.Add(angle.area, angle.angles.ToVector());
            }
        }

        public void ChangeCameraAngles(Areas area)
        {
            areaToAngleDictionary.TryGetValue(area, out position);
            StartCoroutine(ChangeAngles());
        }

        private IEnumerator ChangeAngles()
        {
            while (true)
            {
                // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(position), Time.deltaTime * speed);
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, Quaternion.Euler(position), speed * Time.deltaTime);
                float angle = Quaternion.Angle(transform.rotation, Quaternion.Euler(position));
                if (angle < 0.66f)
                {
                    break;
                }
                yield return null;
            }
        }
    }
}