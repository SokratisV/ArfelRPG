using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
	public class QuestList : MonoBehaviour, ISaveable
	{
		public List<QuestStatus> Statuses {get;} = new List<QuestStatus>();
		public event Action OnListUpdated;

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
			OnListUpdated?.Invoke();
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