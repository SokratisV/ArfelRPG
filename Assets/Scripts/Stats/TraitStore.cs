using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
	public class TraitStore : MonoBehaviour
	{
		public event Action<Trait, int> OnStagedPointsChanged;
		private Dictionary<Trait, int> _assignedPoints = new Dictionary<Trait, int>();
		private Dictionary<Trait, int> _stagedPoints = new Dictionary<Trait, int>();
		private int _unassignedPoints = 10;

		public int UnassignedPoints => _unassignedPoints;

		public int GetProposedPoints(Trait trait) => GetPoints(trait) + GetStagedPoints(trait);

		public int GetPoints(Trait trait) => _assignedPoints.ContainsKey(trait) ? _assignedPoints[trait] : 0;
		public int GetStagedPoints(Trait trait) => _stagedPoints.ContainsKey(trait) ? _stagedPoints[trait] : 0;

		public void AssignPoints(Trait trait, int points)
		{
			if (!CanAssignPoints(trait, points)) return;
			_stagedPoints[trait] = GetStagedPoints(trait) + points;
			_unassignedPoints -= points;
			OnStagedPointsChanged?.Invoke(trait, points);
		}

		public bool CanAssignPoints(Trait trait, int points)
		{
			if (GetStagedPoints(trait) + points < 0) return false;
			return _unassignedPoints >= points;
		}

		public void Commit()
		{
			foreach (var trait in _stagedPoints.Keys)
			{
				_assignedPoints[trait] = GetProposedPoints(trait);
			}

			_stagedPoints.Clear();
			OnStagedPointsChanged?.Invoke(Trait.Constitution, 0);
		}
	}
}