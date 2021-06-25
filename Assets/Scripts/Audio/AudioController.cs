using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Audio
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