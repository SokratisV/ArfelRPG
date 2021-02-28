using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{
		private const string SaveFile = "save";
		[SerializeField] private float fadeInTime = .2f;
		private SavingSystem _saving;

		private void Awake()
		{
			_saving = GetComponent<SavingSystem>();
			StartCoroutine(LoadLastScene());
		}

		private IEnumerator LoadLastScene()
		{
			yield break;
			yield return _saving.LoadLastScene(SaveFile);
			var fader = FindObjectOfType<Fader>();
			fader.FadeOutImmediate();
			yield return fader.FadeIn(fadeInTime);
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.F9))
				Load();
			else if(Input.GetKeyDown(KeyCode.F5))
				Save();
			else if(Input.GetKeyDown(KeyCode.Delete))
				Delete();
		}

		public void Save() => _saving.Save(SaveFile);

		public void Load() => _saving.Load(SaveFile);

		public void Delete() => _saving.Delete(SaveFile);
	}
}