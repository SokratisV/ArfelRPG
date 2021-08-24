using UnityEngine.Events;

namespace RPG.Core.SystemEvents
{
	[System.Serializable]
	public class UnityCameraShakeEvent : UnityEvent<CameraShakeData>
	{
	}
}