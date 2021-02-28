using UnityEngine;

namespace RPG.Core
{
	public struct InteractableActionData : IActionData
	{
		private IAction _action;
		public Transform Target;

		public InteractableActionData(IAction action, Transform target)
		{
			_action = action;
			Target = target;
		}

		public IAction GetAction() => _action;
	}
}