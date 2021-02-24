using System.Collections.Generic;
using System.Linq;

namespace RPG.Quests
{
	public class QuestStatus
	{
		public Quest Quest {get;}

		private List<string> _completedObjectives = new List<string>();

		[System.Serializable]
		private class QuestStatusRecord
		{
			public string questName;
			public List<string> completedObjectives;
		}

		public QuestStatus(Quest trackedQuest) => Quest = trackedQuest;

		public QuestStatus(object objectState)
		{
			if(objectState is QuestStatusRecord state)
			{
				Quest = Quest.GetByName(state.questName);
				_completedObjectives = state.completedObjectives;
			}
		}

		public bool IsComplete => Quest.GetObjectives().All(objective => _completedObjectives.Contains(objective.reference));

		public int CompletedCount => _completedObjectives.Count;

		public bool IsObjectiveComplete(string objective) => _completedObjectives.Contains(objective);

		public void CompleteObjective(string objective)
		{
			if(!Quest.HasObjective(objective)) return;
			_completedObjectives.Add(objective);
		}

		public object CaptureState()
		{
			var state = new QuestStatusRecord {questName = Quest.name, completedObjectives = _completedObjectives};
			return state;
		}
	}
}