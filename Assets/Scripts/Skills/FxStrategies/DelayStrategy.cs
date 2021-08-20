using System.Collections;
using UnityEngine;

namespace RPG.Skills
{
	[CreateAssetMenu(menuName = "RPG/Skills/DelayStrategy", fileName = "DelayStrategy")]
	public class DelayStrategy : EffectsStrategy
	{
		[SerializeField] private float delay;
		[SerializeField] private EffectsStrategy[] effects;

		public override void ExecuteStrategy(SkillData data)
		{
			data.User.GetComponent<SkillUser>().StartCoroutine(DelayedExecution(data));
		}

		private IEnumerator DelayedExecution(SkillData data)
		{
			yield return new WaitForSeconds(delay);
			foreach (var effect in effects)
			{
				effect.ExecuteStrategy(data);
			}
		}
	}
}