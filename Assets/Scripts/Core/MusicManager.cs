using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Core
{
    enum MusicAreas
    {
        Forest,
        Town,
        Hill,
        Death
    }
    public enum CombatMusicAreas
    {
        CombatNormal,
        BossVillage,
        BossTroll
    }

    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        [System.Serializable]
        class AreaToMusicAreaDictionary : SerializableDictionaryBase<Areas, MusicAreas> { }

        [System.Serializable]
        class MusicAreaToMusicDictionary : SerializableDictionaryBase<MusicAreas, AudioClip> { }

        [System.Serializable]
        class CombatMusicAreaToMusicDictionary : SerializableDictionaryBase<CombatMusicAreas, AudioClip> { }

        [SerializeField] AreaToMusicAreaDictionary areaToMusicArea = new AreaToMusicAreaDictionary();
        [SerializeField] MusicAreaToMusicDictionary musicAreaToMusic = new MusicAreaToMusicDictionary();
        [SerializeField] CombatMusicAreaToMusicDictionary areaToBossMusic = new CombatMusicAreaToMusicDictionary();

        AudioSource m_audio;
        MusicAreas currentMusicArea;
        private Coroutine combatMusicCoroutine;
        static int enemiesInCombatWith = 0;

        private void Awake()
        {
            m_audio = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            AIController.onPlayerAggro += ToggleCombatMusic;
            AreaEventManager.onEnterArea += PlayAreaMusic;
            Health.onPlayerDeath += PlayDeathMusic;
        }
        private void OnDisable()
        {
            AIController.onPlayerAggro -= ToggleCombatMusic;
            AreaEventManager.onEnterArea -= PlayAreaMusic;
            Health.onPlayerDeath -= PlayDeathMusic;
        }
        public void PlayAreaMusic(Areas area)
        {
            if (combatMusicCoroutine != null) { return; }

            AudioClip music;
            MusicAreas musicArea;
            areaToMusicArea.TryGetValue(area, out musicArea);
            if (currentMusicArea == musicArea) { return; }

            currentMusicArea = musicArea;
            musicAreaToMusic.TryGetValue(musicArea, out music);
            m_audio.clip = music; // TODO: Fade out/in
            m_audio.Play();
        }
        private void ToggleCombatMusic(bool combat, CombatMusicAreas combatMusic = CombatMusicAreas.CombatNormal)
        {
            if (combat)
            {
                PlayCombatMusic(combatMusic);
            }
            else
            {
                EndCombatMusic();
            }
        }
        private void PlayCombatMusic(CombatMusicAreas combatMusic)
        {
            enemiesInCombatWith++;
            if (combatMusicCoroutine == null)
            {
                combatMusicCoroutine = StartCoroutine(_PlayCombatMusic(combatMusic));
            }
            else
            {
                if (combatMusic != CombatMusicAreas.CombatNormal)
                {
                    // StopCoroutine(combatMusicCoroutine);
                    // combatMusicCoroutine = null;
                    combatMusicCoroutine = StartCoroutine(_PlayCombatMusic(combatMusic));
                }
            }
        }
        private IEnumerator _PlayCombatMusic(CombatMusicAreas area)
        {
            AudioClip music;
            areaToBossMusic.TryGetValue(area, out music);

            m_audio.clip = music; // TODO: Fade out/in
            m_audio.Play();
            yield return null;
        }
        private void EndCombatMusic()
        {
            enemiesInCombatWith--;
            if (enemiesInCombatWith == 0)
            {
                AudioClip music;
                musicAreaToMusic.TryGetValue(currentMusicArea, out music);
                m_audio.clip = music; // TODO: Fade out/in
                m_audio.Play();
                combatMusicCoroutine = null;
            }
        }
        private void PlayDeathMusic()
        {
            enemiesInCombatWith = 0;
            combatMusicCoroutine = null;

            AudioClip music;
            musicAreaToMusic.TryGetValue(MusicAreas.Death, out music);
            m_audio.clip = music; // TODO: Fade out/in
            m_audio.Play();
        }
        public void ResetMusicPlayer()
        {
            enemiesInCombatWith = 0;
            combatMusicCoroutine = null;

            AudioClip music;
            musicAreaToMusic.TryGetValue(MusicAreas.Town, out music);
            m_audio.clip = music; // TODO: Fade out/in
            m_audio.Play();
        }
        public void LowerVolume(float volumeLevel)
        {
            m_audio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);
        }
        public void IncreaseVolume(float volumeLevel)
        {
            m_audio.volume = Mathf.Clamp(volumeLevel, 0.1f, 1.0f);
        }
    }
}
