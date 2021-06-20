using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
	[RequireComponent(typeof(AudioSource))]
	public class MusicManager : MonoBehaviour
	{
		[System.Serializable]
		private class AreaToMusicAreaDictionary : SerializableDictionaryBase<Areas, MusicAreas>
		{
		}

		[System.Serializable]
		private class MusicAreaToMusicDictionary : SerializableDictionaryBase<MusicAreas, AudioClip>
		{
		}

		[System.Serializable]
		private class CombatMusicAreaToMusicDictionary : SerializableDictionaryBase<CombatMusicAreas, AudioClip>
		{
		}

		[SerializeField] private AreaToMusicAreaDictionary areaToMusicArea = new AreaToMusicAreaDictionary();
		[SerializeField] private MusicAreaToMusicDictionary musicAreaToMusic = new MusicAreaToMusicDictionary();
		[SerializeField] private CombatMusicAreaToMusicDictionary areaToBossMusic = new CombatMusicAreaToMusicDictionary();

		private AudioSource _audio;
		private MusicAreas _currentMusicArea;
		private Coroutine _combatMusicCoroutine;
		private static int _enemiesInCombatWith = 0;

		private void Awake() => _audio = GetComponent<AudioSource>();

		private void OnEnable()
		{
			AIController.OnPlayerAggro += ToggleCombatMusic;
			AreaEventManager.OnEnterArea += PlayAreaMusic;
		}

		private void OnDisable()
		{
			AIController.OnPlayerAggro -= ToggleCombatMusic;
			AreaEventManager.OnEnterArea -= PlayAreaMusic;
		}

		public void PlayAreaMusic(Areas area)
		{
			if(_combatMusicCoroutine != null) return;

			areaToMusicArea.TryGetValue(area, out var musicArea);
			if(_currentMusicArea == musicArea) return;

			_currentMusicArea = musicArea;
			musicAreaToMusic.TryGetValue(musicArea, out var music);
			_audio.clip = music; // TODO: Fade out/in
			_audio.Play();
		}

		private void ToggleCombatMusic(bool combat, CombatMusicAreas combatMusic = CombatMusicAreas.CombatNormal)
		{
			if(combat) PlayCombatMusic(combatMusic);
			else EndCombatMusic();
		}

		private void PlayCombatMusic(CombatMusicAreas combatMusic)
		{
			_enemiesInCombatWith++;
			if(_combatMusicCoroutine == null)
				_combatMusicCoroutine = _combatMusicCoroutine.StartCoroutine(this, _PlayCombatMusic(combatMusic));
			else
			{
				if(combatMusic != CombatMusicAreas.CombatNormal)
				{
					// StopCoroutine(combatMusicCoroutine);
					// combatMusicCoroutine = null;
					_combatMusicCoroutine = _combatMusicCoroutine.StartCoroutine(this, _PlayCombatMusic(combatMusic));
				}
			}
		}

		private IEnumerator _PlayCombatMusic(CombatMusicAreas area)
		{
			areaToBossMusic.TryGetValue(area, out var music);
			_audio.clip = music; // TODO: Fade out/in
			_audio.Play();
			yield return null;
		}

		public void EndCombatMusic()
		{
			_enemiesInCombatWith--;
			if(_enemiesInCombatWith == 0)
			{
				musicAreaToMusic.TryGetValue(_currentMusicArea, out var music);
				_audio.clip = music; // TODO: Fade out/in
				_audio.Play();
				_combatMusicCoroutine = null;
			}
		}

		public void PlayDeathMusic()
		{
			_enemiesInCombatWith = 0;
			_combatMusicCoroutine = null;

			musicAreaToMusic.TryGetValue(MusicAreas.Death, out var music);
			_audio.clip = music; // TODO: Fade out/in
			_audio.Play();
		}

		public void ResetMusicPlayer()
		{
			_enemiesInCombatWith = 0;
			_combatMusicCoroutine = null;

			musicAreaToMusic.TryGetValue(MusicAreas.Town, out var music);
			_audio.clip = music; // TODO: Fade out/in
			_audio.Play();
		}

		public void LowerVolume(float volumeLevel) => _audio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);

		public void IncreaseVolume(float volumeLevel) => _audio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);
	}
}