using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
	public class SaveLoadUi : MonoBehaviour
	{
		[SerializeField] private GameObject buttonPrefab;
		[SerializeField] private Transform contentRoot;

		private void OnEnable()
		{
			var savingWrapper = FindObjectOfType<SavingWrapper>();
			if (savingWrapper == null) return;

			foreach (Transform child in contentRoot)
			{
				Destroy(child.gameObject);
			}

			foreach (var save in savingWrapper.ListSaves())
			{
				var obj = Instantiate(buttonPrefab, contentRoot);
				obj.GetComponentInChildren<TMP_Text>().SetText(save);
				obj.GetComponentInChildren<Button>().onClick.AddListener(() => { savingWrapper.LoadGame(save); });
			}
		}
	}
}