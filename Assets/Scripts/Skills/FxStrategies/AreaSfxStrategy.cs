using UnityEngine;

namespace RPG.Skills
{
	[CreateAssetMenu(menuName = "RPG/Skills/Sfx Strategy", fileName = "SfxStrategy")]
	public class AreaSfxStrategy : EffectsStrategy
	{
		[SerializeField] private AudioClip[] clips;

		public override void ExecuteStrategy(SkillData data)
		{
			if (!data.Point.HasValue) return;
			foreach (var sfx in clips)
			{
				AudioSource.PlayClipAtPoint(sfx, data.Point.Value);
			}
		}
	}
}