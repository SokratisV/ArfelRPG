using UnityEngine;

namespace RPG.Core
{
	public static class PlayerFinder
	{
		private static GameObject _playerObject = null;

		public static GameObject Player
		{
			get
			{
				if(!_playerObject)
				{
					_playerObject = GameObject.FindWithTag("Player");
				}

				return _playerObject;
			}
		}

		public static void ResetPlayer() => _playerObject = null;
	}
}