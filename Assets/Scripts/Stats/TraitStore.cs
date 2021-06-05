using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Stats
{
	public class TraitStore : MonoBehaviour
	{
		public event Action<Trait, int> OnStagedPointsChanged;
		private Dictionary<Trait, int> _assignedPoints = new Dictionary<Trait, int>();
		private Dictionary<Trait, int> _stagedPoints = new Dictionary<Trait, int>();

		private BaseStats _baseStats;

		private void Awake() => _baseStats = GetComponent<BaseStats>();

		public int UnassignedPoints => GetAssignablePoints() - GetTotalProposedPoints();

		public int GetTotalProposedPoints() => _assignedPoints.Values.Sum() + _stagedPoints.Values.Sum();

		public int GetProposedPoints(Trait trait) => GetPoints(trait) + GetStagedPoints(trait);
		public int GetPoints(Trait trait) => _assignedPoints.ContainsKey(trait) ? _assignedPoints[trait] : 0;
		public int GetStagedPoints(Trait trait) => _stagedPoints.ContainsKey(trait) ? _stagedPoints[trait] : 0;

		public void AssignPoints(Trait trait, int points)
		{
			if (!CanAssignPoints(trait, points)) return;
			_stagedPoints[trait] = GetStagedPoints(trait) + points;
			OnStagedPointsChanged?.Invoke(trait, points);
		}

		public bool CanAssignPoints(Trait trait, int points)
		{
			if (GetStagedPoints(trait) + points < 0) return false;
			return UnassignedPoints >= points;
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

		public int GetAssignablePoints() => (int) _baseStats.GetStat(Stat.TraitPoints);
	}
}