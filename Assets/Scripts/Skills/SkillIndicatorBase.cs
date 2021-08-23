using RPG.Skills.Behaviors;
using UnityEngine;

namespace RPG.Skills
{
	public abstract class SkillIndicatorBase : MonoBehaviour
	{
		[SerializeField] private IndicatorType type;
		
		public IndicatorType IndicatorType() => type;
	}
}