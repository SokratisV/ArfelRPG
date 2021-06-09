using RPG.SceneManagement;
using RPG.Utils;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
	public class MainMenuUi : MonoBehaviour
	{
		[SerializeField] private TMP_InputField newGameNameField;
		private LazyValue<SavingWrapper> _savingWrapper;

		private void Awake() => _savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);

		public void NewGame() => _savingWrapper.Value.NewGame(newGameNameField.text);

		public void ContinueGame() => _savingWrapper.Value.ContinueGame();
		public void LoadGame(){}
		public void ExitGame(){Application.Quit();}

		private static SavingWrapper GetSavingWrapper() => FindObjectOfType<SavingWrapper>();
	}
}