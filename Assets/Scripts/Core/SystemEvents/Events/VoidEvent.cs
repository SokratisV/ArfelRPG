using UnityEngine;

namespace RPG.Core.SystemEvents
{
	[CreateAssetMenu(fileName = "New Void Event", menuName = "RPG/Events/Void Event")]
	public class VoidEvent : BaseGameEvent<VoidData>
	{
		public override void Raise(VoidData _)
		{
			base.Raise(new VoidData());
		}
	}
}