using UnityEngine;

namespace RPG.Core.SystemEvents
{
	[CreateAssetMenu(fileName = "New Void Event", menuName = "RPG/Events/Void Event")]
	public class VoidEvent : BaseGameEvent<VoidData>
	{
		public void Raise() => Raise(new VoidData());
	}
}