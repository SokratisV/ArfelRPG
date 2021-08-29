using UnityEngine;

namespace RPG.Core
{
	public static class PlayerFinder
	{
		private static GameObject _playerObject = null;
		private static Camera _playerCamera = null;

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

		public static Camera PlayerCamera
		{
			get
			{
				if (!_playerCamera)
				{ 
					_playerCamera = Player.transform.parent.GetComponentInChildren<Camera>();
				}
		
				return _playerCamera;
			}
		}
		
		// public static Camera PlayerCamera
		// {
		// 	get
		// 	{
		// 		if (!_playerCamera)
		// 		{
		// 			_playerCamera = Camera.main;
		// 		}
		//
		// 		return _playerCamera;
		// 	}
		// }

		public static void ResetPlayer() => _playerObject = null;
	}
}