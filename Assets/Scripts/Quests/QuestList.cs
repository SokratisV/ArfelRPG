using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Inventories;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
	public class QuestList : MonoBehaviour, ISaveable
	{
		public List<QuestStatus> Statuses {get;} = new List<QuestStatus>();
		public event Action OnListUpdated;

		private Inventory _inventory;
		private ItemDropper _dropper;

		private void Awake()
		{
			_inventory = GetComponent<Inventory>();
			_dropper = GetComponent<ItemDropper>();
		}

		public void AddQuest(Quest quest)
		{
			if(HasQuest(quest)) return;
			Statuses.Add(new QuestStatus(quest));
			OnListUpdated?.Invoke();
		}

		public void CompleteObjective(Quest quest, string objective)
		{
			var status = GetStatus(quest);
			status.CompleteObjective(objective);
			if(status.IsComplete)
			{
				GiveReward(quest);
			}

			OnListUpdated?.Invoke();
		}

		private void GiveReward(Quest quest)
		{
			foreach(var reward in quest.GetRewards())
			{
				var success = _inventory.AddToFirstEmptySlot(reward.item, reward.number);
				if(!success)
				{
					_dropper.DropItem(reward.item);
				}
			}
		}

		private bool HasQuest(Quest quest) => GetStatus(quest) != null;

		private QuestStatus GetStatus(Quest quest) => Statuses.FirstOrDefault(status => status.Quest == quest);

		public object CaptureState()
		{
			var state = new List<object>();
			foreach(var status in Statuses)
			{
				state.Add(status.CaptureState());
			}

			return state;
		}

		public void RestoreState(object state)
		{
			if(state is List<object> stateList)
			{
				Statuses.Clear();
				foreach(var obj in stateList)
				{
					Statuses.Add(new QuestStatus(obj));
				}
			}
		}
	}
}