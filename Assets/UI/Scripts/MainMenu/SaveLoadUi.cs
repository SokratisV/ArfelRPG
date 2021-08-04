using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace RPG.UI
{
	public class SaveLoadUi : MonoBehaviour
	{
		[SerializeField] private GameObject buttonPrefab;
		[SerializeField] private Transform contentRoot;
		[SerializeField] private UnityEvent onSaveDeleted;

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
				var buttons = obj.GetComponentsInChildren<Button>();
				buttons[0].onClick.AddListener(() => { savingWrapper.LoadGame(save); });
				buttons[1].onClick.AddListener(() =>
				{
					savingWrapper.DeleteSave(save);
					onSaveDeleted?.Invoke();
					Destroy(obj);
				});
			}
		}
	}
}