using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
	[CreateAssetMenu(fileName = "new Quest", menuName = "RPG/Quests/New Quest", order = 0)]
	public class Quest : ScriptableObject
	{
		[SerializeField] private List<string> objectives = new List<string>();

		public string Title => name;
		public int ObjectiveCount => objectives.Count;

		public IEnumerable<string> GetObjectives() => objectives;

		public bool HasObjective(string objective) => objectives.Contains(objective);

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