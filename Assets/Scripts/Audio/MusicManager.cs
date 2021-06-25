using RotaryHeart.Lib.SerializableDictionary;
using RPG.Core;
using UnityEngine;

namespace RPG.Audio
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

		[SerializeField] private EnemyAggroKeeper keeper;
		private AudioSource _audio;
		private MusicAreas _currentMusicArea;
		private bool _isCombatMusicPlaying;

		private void Awake() => _audio = GetComponent<AudioSource>();
		private void OnEnable() => AreaEventManager.OnEnterArea += PlayAreaMusic;
		private void OnDisable() => AreaEventManager.OnEnterArea -= PlayAreaMusic;

		public void PlayAreaMusic(Areas area)
		{
			areaToMusicArea.TryGetValue(area, out var musicArea);
			if (_currentMusicArea == musicArea) return;
			_currentMusicArea = musicArea;
			musicAreaToMusic.TryGetValue(musicArea, out var music);
			_audio.clip = music;
			_audio.Play();
		}

		public void ToggleCombatMusic()
		{
			if (keeper.NumberOfEnemiesInCombatWith > 0) PlayCombatMusic(CombatMusicAreas.CombatNormal);
			else EndCombatMusic();
		}

		private void PlayCombatMusic(CombatMusicAreas combatMusic)
		{
			if (_isCombatMusicPlaying) return;
			_isCombatMusicPlaying = true;
			areaToBossMusic.TryGetValue(combatMusic, out var music);
			_audio.clip = music;
			_audio.Play();
		}

		public void EndCombatMusic()
		{
			if (!_isCombatMusicPlaying) return;
			musicAreaToMusic.TryGetValue(_currentMusicArea, out var music);
			_audio.clip = music;
			_audio.Play();
		}

		public void PlayDeathMusic()
		{
			musicAreaToMusic.TryGetValue(MusicAreas.Death, out var music);
			_audio.clip = music; // TODO: Fade out/in
			_audio.Play();
		}

		public void ResetMusicPlayer()
		{
			musicAreaToMusic.TryGetValue(MusicAreas.Town, out var music);
			_audio.clip = music; // TODO: Fade out/in
			_audio.Play();
		}

		public void LowerVolume(float volumeLevel) => _audio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);

		public void IncreaseVolume(float volumeLevel) => _audio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);
	}
}