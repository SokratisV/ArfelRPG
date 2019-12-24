using System.Collections;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace RPG.Core
{
    enum MusicAreas
    {
        Forest,
        Town,
        Hill
    }
    public enum CombatMusicAreas
    {
        BossVillage,
        BossTroll,
        CombatNormal
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
            print("GOT IT");
        }
        //Unity event call
        public void PlayAreaMusic(Areas area)
        {
            AudioClip music;
            areaToMusicArea.TryGetValue(area, out currentMusicArea);
            musicAreaToMusic.TryGetValue(currentMusicArea, out music);
            print("!(GOT IT)");
            Destroy(gameObject);
            print($"{gameObject.name} name + {transform.parent.name} parent");
            // m_audio.clip = music; // TODO: Fade out/in
            // m_audio.Play();
        }
        //Unity event call
        public void ToggleCombatMusic(bool combat)
        {
            if (combat)
            {
                PlayCombatMusic();
            }
            else
            {
                EndCombatMusic();
            }

        }

        private void PlayCombatMusic()
        {
            enemiesInCombatWith++;
            if (combatMusicCoroutine == null)
            {
                combatMusicCoroutine = StartCoroutine(_PlayCombatMusic(CombatMusicAreas.CombatNormal));
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
            print(enemiesInCombatWith);
            if (enemiesInCombatWith == 0)
            {
                AudioClip music;
                musicAreaToMusic.TryGetValue(currentMusicArea, out music);
                m_audio.clip = music; // TODO: Fade out/in
                m_audio.Play();
                combatMusicCoroutine = null;
            }
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
