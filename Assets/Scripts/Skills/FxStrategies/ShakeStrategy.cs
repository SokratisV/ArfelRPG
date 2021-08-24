using RPG.Core.SystemEvents;
using UnityEngine;

namespace RPG.Skills
{
	[CreateAssetMenu(menuName = "RPG/Skills/Shake Strategy", fileName = "ShakeStrategy")]
	public class ShakeStrategy : EffectsStrategy
	{
		[SerializeField] private CameraShakeEvent shakeEvent;
		[SerializeField] private CameraShakeData shakeData;

		public override void ExecuteStrategy(SkillData data) => shakeEvent.Raise(shakeData);
	}
}