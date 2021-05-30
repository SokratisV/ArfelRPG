using RPG.Core;
using RPG.Quests;
using UnityEngine;

namespace RPG.UI.Quests
{
	public class QuestsListUi : MonoBehaviour
	{
		[SerializeField] private QuestItemUi questPrefab;
		private QuestList _questList;

		private void Start()
		{
			_questList = PlayerFinder.Player.GetComponent<QuestList>();
			_questList.OnListUpdated += UpdateUi;
			UpdateUi();
		}

		private void UpdateUi()
		{
			foreach(Transform child in transform)
			{
				Destroy(child.gameObject);
			}
			
			foreach(var stats in _questList.Statuses)
			{
				var questObject = Instantiate(questPrefab, transform);
				questObject.Setup(stats);
			}
		}
	}
}