using UnityEngine;

namespace RPG.Core
{
	public class AudioController : MonoBehaviour, IAudioPlayer
	{
		[SerializeField] private AudioSource audioSource;

		public void PlaySound(AudioClip clip)
		{
			if (clip != null) audioSource.PlayOneShot(clip);
		}

		public void PlaySound(AudioClip[] clips)
		{
			foreach(var clip in clips)
			{
				PlaySound(clip);
			}
		}
	}
}