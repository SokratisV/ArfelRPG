using UnityEngine;

namespace RPG.Core
{
	public struct FighterActionData : IActionData
	{
		private IAction _action;
		public GameObject Target;

		public FighterActionData(IAction action, GameObject target)
		{
			_action = action;
			Target = target;
		}

		public IAction GetAction() => _action;
	}
}