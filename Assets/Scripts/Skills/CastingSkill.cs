using UnityEngine;

namespace RPG.Skills
{
	public partial class SkillUser
	{
		private class CastingSkill
		{
			public Skill Skill {get;}
			private float _timer;
			private readonly float _castTime;
			private readonly SkillData _data;

			public CastingSkill(Skill skill, SkillData data)
			{
				Skill = skill;
				_castTime = skill.Duration;
				_data = data;
				_timer = 0;
			}

			internal bool Update()
			{
				if(_castTime < 0) return false;
				if(_timer <= _castTime)
				{
					Skill.OnUpdate(_data);
					_timer += Time.deltaTime;
				}
				else
				{
					Skill.OnEnd(_data);
					return true;
				}

				return false;
			}
		}
	}
}