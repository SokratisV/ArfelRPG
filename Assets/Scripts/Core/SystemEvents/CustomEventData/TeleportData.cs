using UnityEngine;

namespace RPG.Core.SystemEvents
{
	[System.Serializable]
	public struct TeleportData
	{
		public Vector3 position;
		public Quaternion rotation;

		public TeleportData(Vector3 teleportPosition, Quaternion teleportRotation)
		{
			position = teleportPosition;
			rotation = teleportRotation;
		}
	}
}