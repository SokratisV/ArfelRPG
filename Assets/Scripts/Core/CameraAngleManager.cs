using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

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
        HillTop
    }

    public class CameraAngleManager : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private AreaAngles[] areaAngles;

        private Dictionary<Areas, Vector3> _areaToAngleDictionary;
        private Vector3 _position;
        private Coroutine _runningCoroutine = null;

        [System.Serializable]
        private class AreaAngles
        {
            public Areas area;
            public SerializableVector3 angles;
        }

        private void Awake()
        {
            DictionaryInit();
        }

        private void OnEnable()
        {
            AreaEventManager.OnEnterArea += ChangeCameraAngles;
        }

        private void OnDisable()
        {
            AreaEventManager.OnEnterArea -= ChangeCameraAngles;
        }

        private void DictionaryInit()
        {
            _areaToAngleDictionary = new Dictionary<Areas, Vector3>();
            foreach(var angle in areaAngles)
            {
                _areaToAngleDictionary.Add(angle.area, angle.angles.ToVector());
            }
        }

        public void ChangeCameraAngles(Areas area)
        {
            if(_areaToAngleDictionary == null)
            {
                DictionaryInit();
            }

            _areaToAngleDictionary.TryGetValue(area, out _position);
            _runningCoroutine = _runningCoroutine.StartCoroutine(this, ChangeAngles());
        }

        private IEnumerator ChangeAngles()
        {
            var b = Quaternion.Euler(_position);
            while(true)
            {
                // transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(position), Time.deltaTime * speed);
                var rotation = transform.rotation;
                rotation = Quaternion.SlerpUnclamped(rotation, b, speed*Time.deltaTime);
                transform.rotation = rotation;
                var angle = Quaternion.Angle(rotation, b);
                if(angle < 1f)
                {
                    _runningCoroutine = null;
                    break;
                }

                yield return null;
            }
        }
    }
}