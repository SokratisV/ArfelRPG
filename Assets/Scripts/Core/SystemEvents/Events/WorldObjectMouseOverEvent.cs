using UnityEngine;

namespace RPG.Core.SystemEvents
{
	[CreateAssetMenu(fileName = "New World Object Mouse Over Event", menuName = "RPG/Events/World Object Mouse Over Event")]
	public class WorldObjectMouseOverEvent : BaseGameEvent<WorldObjectTooltipData>
	{
	}
}