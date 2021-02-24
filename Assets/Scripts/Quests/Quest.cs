using System.Collections.Generic;
using System.Linq;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Quests
{
	[CreateAssetMenu(fileName = "new Quest", menuName = "RPG/Quests/New Quest", order = 0)]
	public class Quest : ScriptableObject
	{
		[SerializeField] private List<Objective> objectives = new List<Objective>();
		[SerializeField] private List<Reward> rewards = new List<Reward>();

		[System.Serializable]
		public class Reward
		{
			[Min(1)] public int number;
			public InventoryItem item;
		}

		[System.Serializable]
		public class Objective
		{
			public string reference;
			public string description;
		}

		public string Title => name;
		public int ObjectiveCount => objectives.Count;

		public IEnumerable<Objective> GetObjectives() => objectives;

		public IEnumerable<Reward> GetRewards() => rewards;

		public bool HasObjective(string objectiveRef) => objectives.Any(objective => objective.reference == objectiveRef);

		public static Quest GetByName(string questName)
		{
			foreach(var quest in Resources.LoadAll<Quest>(""))
			{
				if(quest.name == questName) return quest;
			}

			return null;
		}
	}
}