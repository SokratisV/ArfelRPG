using UnityEngine;

namespace RPG.Core
{
	public static class PlayerFinder
	{
		private static GameObject PlayerObject = null;

		public static GameObject Player
		{
			get
			{
				if(PlayerObject == null)
				{
					PlayerObject = GameObject.FindWithTag("Player");
				}

				return PlayerObject;
			}
		}

		public static void ResetPlayer() => PlayerObject = null;
	}
}