using UnityEngine;

namespace RPG.Core
{
	public struct MoverActionData : IActionData
	{
		private IAction _action;
		public Vector3 Destination;
		public float SpeedFraction;
		public float StopDistance;

		public MoverActionData(IAction action, Vector3 destination, float speedFraction, float stopWithinDistance)
		{
			_action = action;
			Destination = destination;
			SpeedFraction = speedFraction;
			StopDistance = stopWithinDistance;
		}

		public IAction GetAction() => _action;
	}
}