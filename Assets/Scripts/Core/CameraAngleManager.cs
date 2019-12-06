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
        Coroutine runningCoroutine = null;

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
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }
            runningCoroutine = StartCoroutine(ChangeAngles());
        }

        private IEnumerator ChangeAngles()
        {
            Quaternion b = Quaternion.Euler(position);
            while (true)
            {
                // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(position), Time.deltaTime * speed);
                transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, b, speed * Time.deltaTime);
                float angle = Quaternion.Angle(transform.rotation, b);
                if (angle < 1f)
                {
                    runningCoroutine = null;
                    break;
                }
                yield return null;
            }
        }
    }
}