using System.Collections;
using UnityEngine;

namespace RPG.Skills
{
	public partial class SkillUser
	{
		private class ActivatedSkill
		{
			public bool HasEnded;
			public Skill Skill {get;}
			public readonly IEnumerator UpdateBehavior;
			public readonly SkillData Data;
			private float _timer;
			private readonly float _duration;

			public ActivatedSkill(Skill skill, (SkillData, IEnumerator) data)
			{
				Skill = skill;
				Data = data.Item1;
				_duration = skill.Duration;
				_timer = 0;
				UpdateBehavior = data.Item2;
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