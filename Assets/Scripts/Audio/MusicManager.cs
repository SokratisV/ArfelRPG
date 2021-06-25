using RotaryHeart.Lib.SerializableDictionary;
using RPG.Core;
using UnityEngine;

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

		private AudioSource _audio;
		private Areas _currentArea;
		private bool _isCombatMusicPlaying;

		private void Awake() => _audio = GetComponent<AudioSource>();

		public void PlayAreaMusic(Areas area)
		{
			if (_currentArea == area) return;
			_currentArea = area;
			areasToAudio.TryGetValue(area, out var clip);
			_audio.clip = clip;
			_audio.Play();
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
			_audio.clip = combatMusic;
			_audio.Play();
		}

		public void EndCombatMusic()
		{
			if (!_isCombatMusicPlaying) return;
			areasToAudio.TryGetValue(_currentArea, out var music);
			_audio.clip = music;
			_audio.Play();
		}

		public void PlayDeathMusic()
		{
			_audio.clip = deathMusic;
			_audio.Play();
		}

		public void ResetMusicPlayer()
		{
			areasToAudio.TryGetValue(Areas.Village, out var music);
			_audio.clip = music;
			_audio.Play();
		}
	}
}