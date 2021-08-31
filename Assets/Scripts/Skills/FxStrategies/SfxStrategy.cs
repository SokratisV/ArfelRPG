using System.Collections;
using RPG.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RPG.Skills
{
	[CreateAssetMenu(menuName = "RPG/Skills/Sfx Strategy", fileName = "SfxStrategy")]
	public class SfxStrategy : EffectsStrategy
	{
		[SerializeField, ValueDropdown(nameof(_targetChoice))]
		private int targetChoice;

		[SerializeField] private AudioClip[] clips;

		public override void ExecuteStrategy(SkillData data)
		{
			switch (targetChoice)
			{
				case 0:
				{
					TargetSfx(data);
					break;
				}
				case 1:
				{
					AreaSfx(data);
					break;
				}
				case 2:
				{
					UserSfx(data);
					break;
				}
			}
		}

		private void AreaSfx(SkillData data)
		{
			if (!data.Point.HasValue) return;
			foreach (var sfx in clips)
			{
				AudioSource.PlayClipAtPoint(sfx, data.Point.Value);
			}
		}

		private void UserSfx(SkillData data)
		{
			if (!data.Targets[0].TryGetComponent(out IAudioPlayer audioPlayer)) return;
			foreach (var clip in clips)
			{
				audioPlayer.PlaySound(clip);
			}
		}
		
		private void TargetSfx(SkillData data)
		{
			if (!data.Targets[1].TryGetComponent(out IAudioPlayer audioPlayer)) return;
			foreach (var clip in clips)
			{
				audioPlayer.PlaySound(clip);
			}
		}

		private static IEnumerable _targetChoice = new ValueDropdownList<int>
		{
			{"Target", 0},
			{"Area", 1},
			{"User", 2}
		};
	}
}