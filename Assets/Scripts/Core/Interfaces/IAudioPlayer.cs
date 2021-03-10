using UnityEngine;

namespace RPG.Core
{
	public interface IAudioPlayer
	{
		void PlaySound(AudioClip clip);
		void PlaySound(AudioClip[] clip);
	}
}