using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
	public interface IAudioPlayer
	{
		void PlaySound(AudioClip clip);
		void PlaySound(IEnumerable<AudioClip> clip);
	}
}