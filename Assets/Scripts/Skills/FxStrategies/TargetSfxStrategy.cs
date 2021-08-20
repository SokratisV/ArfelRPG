using RPG.Core;
using UnityEngine;

namespace RPG.Skills
{
	[CreateAssetMenu(menuName = "RPG/Skills/Sfx Strategy", fileName = "SfxStrategy")]
	public class TargetSfxStrategy : EffectsStrategy
	{
		[SerializeField] private AudioClip[] clips;

		public override void ExecuteStrategy(SkillData data)
		{
			if (!data.User.TryGetComponent(out IAudioPlayer audioPlayer)) return;
			foreach (var clip in clips)
			{
				audioPlayer.PlaySound(clip);
			}
		}
	}
}