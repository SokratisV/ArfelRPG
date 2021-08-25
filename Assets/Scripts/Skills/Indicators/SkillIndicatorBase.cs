using RPG.Skills.Behaviors;
using UnityEngine;

namespace RPG.Skills
{
	public abstract class SkillIndicatorBase : MonoBehaviour
	{
		protected IndicatorType Type;
		
		public IndicatorType IndicatorType() => Type;
	}
}