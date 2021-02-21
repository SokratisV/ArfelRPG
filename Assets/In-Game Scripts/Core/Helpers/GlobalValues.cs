using UnityEngine;

namespace RPG.Core
{
	public static class GlobalValues
	{
		public static float OutlineOffDelay {get;} = .05f;
		public static Color32 PickupColor {get;} = new Color32(255, 255, 0, 0);
		public static float InteractableRange {get;} = 1f;
		public static float DefaultAttackSpeed {get;} = 1f;
	}
}