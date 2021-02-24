using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using RPG.Attributes;
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

		private AudioSource _mAudio;
		private MusicAreas _currentMusicArea;
		private Coroutine _combatMusicCoroutine;
		private static int EnemiesInCombatWith = 0;

		private void Awake() => _mAudio = GetComponent<AudioSource>();

		private void OnEnable()
		{
			AIController.OnPlayerAggro += ToggleCombatMusic;
			AreaEventManager.OnEnterArea += PlayAreaMusic;
			Health.OnPlayerDeath += PlayDeathMusic;
		}

		private void OnDisable()
		{
			AIController.OnPlayerAggro -= ToggleCombatMusic;
			AreaEventManager.OnEnterArea -= PlayAreaMusic;
			Health.OnPlayerDeath -= PlayDeathMusic;
		}

		public void PlayAreaMusic(Areas area)
		{
			if(_combatMusicCoroutine != null)
				return;

			areaToMusicArea.TryGetValue(area, out var musicArea);
			if(_currentMusicArea == musicArea)
				return;

			_currentMusicArea = musicArea;
			musicAreaToMusic.TryGetValue(musicArea, out var music);
			_mAudio.clip = music; // TODO: Fade out/in
			_mAudio.Play();
		}

		private void ToggleCombatMusic(bool combat, CombatMusicAreas combatMusic = CombatMusicAreas.CombatNormal)
		{
			if(combat)
				PlayCombatMusic(combatMusic);
			else
				EndCombatMusic();
		}

		private void PlayCombatMusic(CombatMusicAreas combatMusic)
		{
			EnemiesInCombatWith++;
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

			_mAudio.clip = music; // TODO: Fade out/in
			_mAudio.Play();
			yield return null;
		}

		private void EndCombatMusic()
		{
			EnemiesInCombatWith--;
			if(EnemiesInCombatWith == 0)
			{
				musicAreaToMusic.TryGetValue(_currentMusicArea, out var music);
				_mAudio.clip = music; // TODO: Fade out/in
				_mAudio.Play();
				_combatMusicCoroutine = null;
			}
		}

		private void PlayDeathMusic()
		{
			EnemiesInCombatWith = 0;
			_combatMusicCoroutine = null;

			musicAreaToMusic.TryGetValue(MusicAreas.Death, out var music);
			_mAudio.clip = music; // TODO: Fade out/in
			_mAudio.Play();
		}

		public void ResetMusicPlayer()
		{
			EnemiesInCombatWith = 0;
			_combatMusicCoroutine = null;

			musicAreaToMusic.TryGetValue(MusicAreas.Town, out var music);
			_mAudio.clip = music; // TODO: Fade out/in
			_mAudio.Play();
		}

		public void LowerVolume(float volumeLevel) => _mAudio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);

		public void IncreaseVolume(float volumeLevel) => _mAudio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);
	}
}