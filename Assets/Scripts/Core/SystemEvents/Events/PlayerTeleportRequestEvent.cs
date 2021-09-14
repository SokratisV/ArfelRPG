using UnityEngine;

namespace RPG.Core.SystemEvents
{
	[CreateAssetMenu(fileName = "New Player Teleport Request Event", menuName = "RPG/Events/Teleport Request Event")]
	public class PlayerTeleportRequestEvent : BaseGameEvent<TeleportData>
	{
	}
}