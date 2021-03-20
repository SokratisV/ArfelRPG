using UnityEngine;

namespace RPG.Skills
{
	public partial class SkillUser
	{
		private class ActivatedSkill
		{
			public bool HasEnded;
			public Skill Skill {get;}
			public readonly SkillData Data;
			private float _timer;
			private readonly float _duration;

			public ActivatedSkill(Skill skill, SkillData data)
			{
				Skill = skill;
				Data = data;
				_duration = skill.Duration;
				_timer = 0;
			}

			internal bool Update()
			{
				if(_duration < 0) return false;
				if(_timer <= _duration)
				{
					_timer += Time.deltaTime;
				}
				else
				{
					Skill.OnEnd(Data);
					HasEnded = true;
					return true;
				}

				return false;
			}
		}
	}
}