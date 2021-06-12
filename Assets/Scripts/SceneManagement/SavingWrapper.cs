using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{
		public const string CurrentSaveKey = "currentSave";
		[SerializeField] private float fadeInTime = .2f, fadeOutTime = .2f;
		private SavingSystem _saving;
		private Fader _fader;

		private void Start() => _saving = GetComponent<SavingSystem>();

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Delete)) DeleteAllSaves();
		}

		#region Public

		public void ContinueGame() => StartCoroutine(LoadLastScene());

		public void NewGame(string saveFile)
		{
			SetCurrentSave(saveFile);
			StartCoroutine(LoadFirstScene());
		}

		public void LoadGame(string saveFile)
		{
			SetCurrentSave(saveFile);
			ContinueGame();
		}

		public void LoadMenu() => StartCoroutine(LoadMenuScene());

		public void Save() => _saving.Save(GetCurrentSave());
		public void Load() => _saving.Load(GetCurrentSave());
		public void Delete() => _saving.Delete(GetCurrentSave());

		public void DeleteAllSaves()
		{
			foreach (var save in ListSaves())
			{
				_saving.Delete(save);
			}

			SetCurrentSave(string.Empty);
		}

		public IEnumerable<string> ListSaves() => _saving.ListSaves();

		#endregion

		#region Private

		private void SetCurrentSave(string saveFile) => PlayerPrefs.SetString(CurrentSaveKey, saveFile);
		private string GetCurrentSave() => PlayerPrefs.GetString(CurrentSaveKey);

		private IEnumerator LoadMenuScene()
		{
			if (_fader == null) _fader = FindObjectOfType<Fader>();
			yield return _fader.FadeOut(fadeOutTime);
			yield return SceneManager.LoadSceneAsync(0);
			yield return _fader.FadeIn(fadeInTime);
		}

		private IEnumerator LoadFirstScene()
		{
			if (_fader == null) _fader = FindObjectOfType<Fader>();
			yield return _fader.FadeOut(fadeOutTime);
			yield return SceneManager.LoadSceneAsync(1);
			yield return _fader.FadeIn(fadeInTime);
			Save();
		}

		private IEnumerator LoadLastScene()
		{
			if (_fader == null) _fader = FindObjectOfType<Fader>();
			yield return _fader.FadeOut(fadeOutTime);
			yield return _saving.LoadLastScene(GetCurrentSave());
			yield return _fader.FadeIn(fadeInTime);
		}

		#endregion
	}
}