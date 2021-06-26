using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using RPG.Core;
using UnityEngine;
using UnityEngine.Audio;

namespace RPG.Audio
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicManager : MonoBehaviour
	{
		[System.Serializable]
		private class AreasToAudio : SerializableDictionaryBase<Areas, AudioClip>
		{
		}

		[SerializeField] private AudioClip deathMusic, combatMusic;
		[SerializeField] private AreasToAudio areasToAudio = new AreasToAudio();
		[SerializeField] private EnemyAggroKeeper keeper;
		[SerializeField] private AudioMixer musicMixer;

		private AudioSource _audio;
		private Coroutine _audioFadeRoutine;
		private Areas _currentArea;
		private bool _isCombatMusicPlaying;

		private void Awake() => _audio = GetComponent<AudioSource>();

		public void PlayAreaMusic(Areas area)
		{
			if (_currentArea == area) return;
			_currentArea = area;
			areasToAudio.TryGetValue(area, out var clip);
			_audioFadeRoutine.StartCoroutine(this, SwitchTracks(clip));
		}

		public void ToggleCombatMusic()
		{
			if (keeper.NumberOfEnemiesInCombatWith > 0) PlayCombatMusic();
			else EndCombatMusic();
		}

		private void PlayCombatMusic()
		{
			if (_isCombatMusicPlaying) return;
			_isCombatMusicPlaying = true;
			_audioFadeRoutine.StartCoroutine(this, SwitchTracks(combatMusic));
		}

		public void EndCombatMusic()
		{
			if (!_isCombatMusicPlaying) return;
			areasToAudio.TryGetValue(_currentArea, out var clip);
			_audioFadeRoutine.StartCoroutine(this, SwitchTracks(clip));
		}

		public void PlayDeathMusic() => _audioFadeRoutine.StartCoroutine(this, SwitchTracks(deathMusic));

		public void ResetMusicPlayer()
		{
			areasToAudio.TryGetValue(Areas.Village, out var clip);
			_audioFadeRoutine.StartCoroutine(this, SwitchTracks(clip));
		}

		private IEnumerator SwitchTracks(AudioClip nextClip)
		{
			yield return StartFade(musicMixer, "Volume", 1f, 0f);
			_audio.clip = nextClip;
			_audio.Play();
			yield return StartFade(musicMixer, "Volume", 1f, PlayerPrefs.GetFloat(GlobalValues.MusicVolume));
		}

		private static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
		{
			float currentTime = 0;
			audioMixer.GetFloat(exposedParam, out var currentVol);
			currentVol = Mathf.Pow(10, currentVol / 20);
			var targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				var newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
				audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
				yield return null;
			}
		}
	}
}