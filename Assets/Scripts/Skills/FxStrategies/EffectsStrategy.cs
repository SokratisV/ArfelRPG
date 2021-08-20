using UnityEngine;

namespace RPG.Skills
{
	public abstract class EffectsStrategy : ScriptableObject
	{
		public abstract void ExecuteStrategy(SkillData data);
	}
}