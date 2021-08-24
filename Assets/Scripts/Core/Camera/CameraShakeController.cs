using System.Collections;
using Cinemachine;
using RPG.Core;
using RPG.Core.SystemEvents;
using UnityEngine;

public class CameraShakeController : MonoBehaviour
{
	[SerializeField] private CinemachineVirtualCamera virtualCamera;
	private Coroutine _shakeCoroutine;
	private CinemachineBasicMultiChannelPerlin _shaker;

	private void Awake() => _shaker = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

	public void Shake(CameraShakeData shakeData) => _shakeCoroutine.StartCoroutine(this, ShakeCoroutine(shakeData));

	private IEnumerator ShakeCoroutine(CameraShakeData shakeData)
	{
		if (!_shaker) yield break;
		_shaker.m_AmplitudeGain = shakeData.intensity;
		var progress = 0f;
		while (progress < 1)
		{
			progress += Time.deltaTime / shakeData.duration;
			_shaker.m_AmplitudeGain = Mathf.Lerp(shakeData.intensity, 0, progress);
			yield return null;
		}

		_shaker.m_AmplitudeGain = 0;
	}
}