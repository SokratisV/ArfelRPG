using System.Collections;
using UnityEngine;

namespace RPG.Skills
{
	public partial class SkillUser
	{
		private class CastingSkill
		{
			public Skill Skill {get;}
			public readonly SkillData Data;
			public readonly IEnumerator UpdateBehavior;
			private float _timer;
			private readonly float _castTime;

			public CastingSkill(Skill skill, (SkillData, IEnumerator) data)
			{
				Skill = skill;
				Data = data.Item1;
				_castTime = skill.Duration;
				_timer = 0;
				UpdateBehavior = data.Item2;
			}

			internal bool Update()
			{
				if(_castTime < 0) return false;
				if(_timer <= _castTime)
				{
					_timer += Time.deltaTime;
				}
				else
				{
					Skill.OnEnd(Data);
					return true;
				}

				return false;
			}
		}
	}
}