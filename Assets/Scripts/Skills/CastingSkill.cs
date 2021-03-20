using UnityEngine;

namespace RPG.Skills
{
	public partial class SkillUser
	{
		private class CastingSkill
		{
			public Skill Skill {get;}
			public readonly SkillData Data;
			private float _timer;
			private readonly float _castTime;

			public CastingSkill(Skill skill, SkillData data)
			{
				Skill = skill;
				Data = data;
				_castTime = skill.Duration;
				_timer = 0;
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