using System.Collections.Generic;
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

		public void PlaySound(IEnumerable<AudioClip> clips)
		{
			foreach(var clip in clips)
			{
				PlaySound(clip);
			}
		}
	}
}