using RPG.Control;
using RPG.Core;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.UI
{
	public class PauseMenuUi : MonoBehaviour
	{
		private SavingWrapper _savingWrapper;
		
		private void Start() => _savingWrapper = FindObjectOfType<SavingWrapper>();

		private void OnEnable()
		{
			var playerController = PlayerFinder.Player.GetComponent<PlayerController>();
			playerController.enabled = false;
			playerController.SetCursor(CursorType.UI);
			Time.timeScale = 0;
		}

		private void OnDisable()
		{
			Time.timeScale = 1;
			PlayerFinder.Player.GetComponent<PlayerController>().enabled = true;
		}

		public void Save() => _savingWrapper.Save();

		public void SaveAndQuit()
		{
			_savingWrapper.Save();
			_savingWrapper.LoadMenu();
		}
	}
}