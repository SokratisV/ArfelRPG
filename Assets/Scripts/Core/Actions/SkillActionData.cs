using UnityEngine;

namespace RPG.Core
{
	public struct SkillActionData
	{
		private IAction _action;
		public GameObject[] Targets;
		public int SkillIndex;

		public SkillActionData(IAction action, GameObject[] targets, int skillIndex)
		{
			_action = action;
			Targets = targets;
			SkillIndex = skillIndex;
		}

		public IAction GetAction() => _action;
	}
}