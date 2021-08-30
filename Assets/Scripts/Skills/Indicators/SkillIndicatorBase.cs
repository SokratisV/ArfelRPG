using RPG.Skills.Behaviors;
using UnityEngine;

namespace RPG.Skills
{
	public abstract class SkillIndicatorBase : MonoBehaviour
	{
		public IndicatorType IndicatorType() => Type;

		protected IndicatorType Type;
		protected virtual Color32 SkillIndicatorEnabled { get; } = new Color32(0, 150, 190, 150);
		protected virtual Color32 SkillIndicatorDisabled { get; } = new Color32(200, 10, 10, 15);
		private bool _indicatorState;

		public void Awake()
		{
			Init();
			ToggleColorState(true);
		}

		protected abstract void Init();
		protected abstract void ChangeColor(Color32 color);

		public void ToggleColorState(bool toggle)
		{
			if (_indicatorState == toggle) return;
			ChangeColor(toggle ? SkillIndicatorEnabled : SkillIndicatorDisabled);
			_indicatorState = toggle;
		}
	}
}