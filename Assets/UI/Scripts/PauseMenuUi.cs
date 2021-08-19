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

		public void Toggle(bool toggle)
		{
			if (toggle) Pause();
			else Resume();
		}

		private void Pause()
		{
			var playerController = PlayerFinder.Player.GetComponent<PlayerController>();
			playerController.enabled = false;
			playerController.SetCursor(CursorType.UI);
			Time.timeScale = 0;
		}

		private void Resume()
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