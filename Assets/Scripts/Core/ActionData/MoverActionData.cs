using UnityEngine;

namespace RPG.Core
{
	public struct MoverActionData : IActionData
	{
		private IAction _action;
		public Vector3 Destination;
		public float Speed;
		public float StopDistance;

		public MoverActionData(IAction action, Vector3 destination, float speed, float stopWithinDistance)
		{
			_action = action;
			Destination = destination;
			Speed = speed;
			StopDistance = stopWithinDistance;
		}

		public IAction GetAction() => _action;
	}
}