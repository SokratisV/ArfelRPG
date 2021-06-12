using RPG.SceneManagement;
using RPG.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class MainMenuUi : MonoBehaviour
	{
		[SerializeField] private TMP_InputField newGameNameField;
		[SerializeField] private Button continueButton;
		
		private LazyValue<SavingWrapper> _savingWrapper;

		private void Awake() => _savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);

		private void Start()
		{
			if (string.IsNullOrEmpty(PlayerPrefs.GetString(SavingWrapper.CurrentSaveKey)))
			{
				continueButton.interactable = false;
			}
		}

		public void NewGame() => _savingWrapper.Value.NewGame(newGameNameField.text);

		public void ContinueGame() => _savingWrapper.Value.ContinueGame();

		public void ExitGame()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
			Application.OpenURL("itch.io");
#else
			Application.Quit();
#endif
		}

		private static SavingWrapper GetSavingWrapper() => FindObjectOfType<SavingWrapper>();
	}
}