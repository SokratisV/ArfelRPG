using UnityEngine;

namespace RPG.Skills
{
	public partial class SkillUser
	{
		private class ActivatedSkill
		{
			public bool HasEnded;
			public Skill Skill {get;}
			private float _timer;
			private readonly float _duration;
			private readonly SkillData _data;

			//TODO: create new instance of skill here?
			public ActivatedSkill(Skill skill, SkillData data)
			{
				Skill = skill;
				_duration = skill.Duration;
				_data = data;
				_timer = 0;
			}

			internal bool Update()
			{
				if(_duration < 0) return false;
				if(_timer <= _duration)
				{
					Skill.OnUpdate(_data);
					_timer += Time.deltaTime;
				}
				else
				{
					Skill.OnEnd(_data);
					HasEnded = true;
					return true;
				}

				return false;
			}
		}
	}
}