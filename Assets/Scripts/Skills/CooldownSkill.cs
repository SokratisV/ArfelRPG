using UnityEngine;

namespace RPG.Skills
{
	public partial class SkillUser
	{
		private class CooldownSkill
		{
			public bool HasCooledDown;
			public Skill Skill {get;}
			private readonly float _cooldown;
			private float _timer;

			internal bool Update()
			{
				if(_timer <= _cooldown)
				{
					_timer += Time.deltaTime;
				}
				else
				{
					HasCooledDown = true;
					return true;
				}

				return false;
			}

			public CooldownSkill(Skill skill)
			{
				_cooldown = skill.Cooldown;
				Skill = skill;
			}
		}
	}
}