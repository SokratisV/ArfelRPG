using UnityEngine;

namespace RPG.Core
{
	public struct PickableActionData : IActionData
	{
		private IAction _action;
		public Transform Treasure;

		public PickableActionData(IAction action, Transform treasure)
		{
			_action = action;
			Treasure = treasure;
		}

		public IAction GetAction() => _action;
	}
}